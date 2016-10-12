using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TimeTablingApplication
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmSolutionDetails frmSolDe = new frmSolutionDetails();
            this.Hide();
            frmSolDe.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmChooseSolution frmChoSol = new frmChooseSolution();
            this.Hide();
            frmChoSol.ShowDialog();
            this.Close();
        }
    }
}
