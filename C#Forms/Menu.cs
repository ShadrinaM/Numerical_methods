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
    }
}