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
    public partial class frmChooseSolution : Form
    {
        public frmChooseSolution()
        {
            InitializeComponent();
        }

        private void frmChooseSolution_Load(object sender, EventArgs e)
        {
            DataConnection dataConnection = new DataConnection();
            DataTable dt = dataConnection.OpenDataSet("SELECT * FROM GIAIPHAP").Tables[0];
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 1)
            {
                MessageBox.Show("Vui lòng chọn giải pháp cần mở!");
                return;
            }
            frmSolutionResult frmSolRe = new frmSolutionResult(dataGridView1.SelectedCells[0].OwningRow.Cells["MaGP"].Value.ToString());
            this.Hide();
            frmSolRe.ShowDialog();
            this.Close();
        }
    }
}
