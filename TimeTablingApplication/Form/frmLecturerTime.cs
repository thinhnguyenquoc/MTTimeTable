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
    public partial class frmLecturerTime : Form
    {
        List<string> result;
        DataTable dtTime;
        public frmLecturerTime(List<string> result, DataTable dtTime)
        {
            InitializeComponent();
            this.result = result;
            this.dtTime = dtTime;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmLecturerTime_Load(object sender, EventArgs e)
        {
            
            dataGridView1.DataSource = dtTime;
            DataGridViewCheckBoxColumn c = new DataGridViewCheckBoxColumn();
            c.Name = "CoTheThamGia";
            c.HeaderText = "CoTheThamGia";
            c.TrueValue = "0";
            c.FalseValue = "1";
            
            dataGridView1.Columns.Add(c);
            foreach (DataGridViewRow row in dataGridView1.Rows)
                row.Cells["CoTheThamGia"].Value = "0";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
                result.Add(row.Cells["CoTheThamGia"].Value.ToString());

            this.Dispose();
        }
    }
}
