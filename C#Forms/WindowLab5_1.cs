using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C_Forms
{
    public partial class WindowLab5_1 : Form
    {
        private Menu mainForm;

        public WindowLab5_1(Menu menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = menushka;
            this.FormClosed += WindowLab5_1_FormClosed;
            Lab51();
        }
        private void WindowLab5_1_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }

        void Lab51()
        {
            
        }

    }
}
