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
    public partial class frmChooseThesis : Form
    {
        DataConnection dataConnection;
        DataTable dt;
        public frmChooseThesis(DataTable dt)
        {
            InitializeComponent();
            dataConnection = new DataConnection();
            this.dt = dt;
        }

        private void frmChooseThesis_Load(object sender, EventArgs e)
        {
            string s = "";
            if (dt.Rows.Count == 0)
                s = "-1'";
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s += dt.Rows[i]["MaDT"].ToString() + "','";
                }
                s = s.Substring(0, s.Length - 2);
            }
            DataSet ds = dataConnection.OpenDataSet("SELECT * FROM DETAI_MAU WHERE MaDT NOT IN('" + s + ")");
            dataGridView1.DataSource = ds.Tables[0];
            DataGridViewCheckBoxColumn checkCol = new DataGridViewCheckBoxColumn()
            {
                Name = "Checked Column",
                FalseValue = 0,
                TrueValue = 1,
                Visible = true
            };
            dataGridView1.Columns.Insert(0, checkCol);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    dt.Rows.Add(row.Cells["MaDT"].Value.ToString(),
                        row.Cells["TenDT"].Value.ToString(),
                        row.Cells["SVTH"].Value.ToString(),
                        row.Cells["GVHD"].Value.ToString(),
                        row.Cells["BoMon"].Value.ToString());
                }
            }
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
