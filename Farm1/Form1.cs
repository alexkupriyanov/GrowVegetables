using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Farm1
{
    public partial class Form1 : Form
    {
        Dictionary<CheckBox, Cell> field = new Dictionary<CheckBox, Cell>();

        private Points score = new Points();

        public Form1()
        {
            InitializeComponent();
            foreach (CheckBox cb in flowLayoutPanel1.Controls)
            {
                field.Add(cb, new Cell());
            }
            UpdateMoney();
        }

        private int GrowingScore = -1;

        private int GreenScore = 0;

        private int NearbyScore = 2;

        private int MatureScore = 3;

        private int ClearPrice = 1;

        private int PlantPrice = 1;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Checked) StartGrow(cb);
            else Cut(cb);
        }

        private void Cut(CheckBox cb)
        {
            CellState status = field[cb].GetStatus();
            switch (status)
            {
                case CellState.Growing:
                    score.AddPoint(GrowingScore);
                    break;
                case CellState.Green:
                    score.AddPoint(GreenScore);
                    break;
                case CellState.Nearby:
                    score.AddPoint(NearbyScore);
                    break;
                case CellState.Mature:
                    score.AddPoint(MatureScore);
                    break;
                case CellState.Rotten:
                    score.SubPoint(ClearPrice);
                    break;

            }
            field[cb].Cut();
            UpdateBox(cb);     
            UpdateMoney();
        }

        private void StartGrow(CheckBox cb)
        {
            field[cb].StartGrowing();
            UpdateBox(cb);
            score.SubPoint(PlantPrice);
            UpdateMoney();
        }

        int date = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            date++;
            label1.Text = "Day: " + date;

            foreach (CheckBox cb in flowLayoutPanel1.Controls)
            {
                field[cb].Step();
                UpdateBox(cb);
            }
        }

        private void UpdateBox(CheckBox cb)
        {
            switch (field[cb].state)
            {
                case CellState.Empty: cb.BackColor = Color.White; break;
                case CellState.Growing: cb.BackColor = Color.Black; break;
                case CellState.Green: cb.BackColor = Color.Green; break;
                case CellState.Nearby: cb.BackColor = Color.Yellow; break;
                case CellState.Mature: cb.BackColor = Color.Red; break;
                case CellState.Rotten: cb.BackColor = Color.Brown; break;
            }
        }

        private void UpdateMoney()
        {
            label2.Text = "Money: " + score.Sum;
        }
    }

    enum CellState { Empty, Growing, Green, Nearby, Mature, Rotten};

    class Cell
    {
        private int progress = 0;
        const int prGrowing = 15;
        const int prGreen = 65;
        const int prNearby = 85;
        const int prMature = 100;

        public CellState state = CellState.Empty;
        
        public void Step()
        {
            if (state != CellState.Empty && state != CellState.Rotten)
            {
                progress++;
                if (progress < prGrowing) state = CellState.Growing;
                else if (progress < prGreen) state = CellState.Green;
                else if (progress < prNearby) state = CellState.Nearby;
                else if (progress < prMature) state = CellState.Mature;
                else state=CellState.Rotten;
            }
        }

        public void StartGrowing()
        {
            state = CellState.Growing;
        }

        public void Cut()
        {
            state = CellState.Empty;
            progress = 0;            
        }

        public CellState GetStatus()
        {
            if (progress < prGrowing) return CellState.Growing;
            else if (progress < prGreen) return CellState.Green;
            else if (progress < prNearby) return CellState.Nearby;
            else if (progress < prMature) return CellState.Mature;
            else return CellState.Rotten;
        }
    }
     
    class Points
    {
        private int sum = 10;

        public int Sum { get => sum; }
        
        public void AddPoint(int value)
        {
            sum += value;
        }

        public void SubPoint(int value)
        {
            sum -= value;
        }
    }
}
