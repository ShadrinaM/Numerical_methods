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
    public partial class WindowLabUCHP : Form
    {
        private Menu mainForm;

        public WindowLabUCHP(Menu menushka)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = menushka;
            this.FormClosed += WindowLabUCHP_FormClosed;
            LabUCHP();
        }
        private void WindowLabUCHP_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }

        void LabUCHP()
        {
            
        }

    }
}
