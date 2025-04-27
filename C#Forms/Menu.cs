using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C_Forms
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void StartTask3_2_Click(object sender, EventArgs e)
        {
            WindowLab3_2 window = new WindowLab3_2(this);
            window.Show();
            this.Hide();
        }
        private void StartTask4_Click(object sender, EventArgs e)
        {
            WindowLab4 window = new WindowLab4(this);
            window.Show();
            this.Hide();
        }
        private void StartTask5_2_Click(object sender, EventArgs e)
        {
            WindowLab5_2 window = new WindowLab5_2(this);
            window.Show();
            this.Hide();
        }

        private void StartTask5_1_Click(object sender, EventArgs e)
        {
            WindowLab5_1 window = new WindowLab5_1(this);
            window.Show();
            this.Hide();
        }

        private void StartTask6_Click(object sender, EventArgs e)
        {
            WindowLab6 window = new WindowLab6(this);
            window.Show();
            this.Hide();
        }

        private void StartTask7_1_Click(object sender, EventArgs e)
        {
            WindowLab7_1 window = new WindowLab7_1(this);
            window.Show();
            this.Hide();
        }

        private void StartTask8_Click(object sender, EventArgs e)
        {
            WindowLab8 window = new WindowLab8(this);
            window.Show();
            this.Hide();
        }

        private void StartTask7_2_Click(object sender, EventArgs e)
        {
            WindowLab7_2 window = new WindowLab7_2(this);
            window.Show();
            this.Hide();
        }

        private void StartTaskUCHP_Click(object sender, EventArgs e)
        {
            WindowLabUCHP window = new WindowLabUCHP(this);
            window.Show();
            this.Hide();
        }

        private void StartTask1_Click(object sender, EventArgs e)
        {
            WindowLab1 window = new WindowLab1(this);
            window.Show();
            this.Hide();
        }
    }
}