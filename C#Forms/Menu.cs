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

        private void StartTask4_Click(object sender, EventArgs e)
        {
            WindowLab4 window = new WindowLab4(this);
            window.Show();
            this.Hide();
        }
    }
}