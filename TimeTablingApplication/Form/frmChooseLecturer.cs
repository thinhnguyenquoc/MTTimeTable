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
    public partial class frmChooseLecturer : Form
    {
        DataConnection dataConnection;
        DataTable dt;
        public frmChooseLecturer(DataTable dt)
        {
            InitializeComponent();
            dataConnection = new DataConnection();
            this.dt = dt;
        }

        private void ChooseLecturer_Load(object sender, EventArgs e)
        {
            string s="";
            if (dt.Rows.Count == 0)
                s = "-1'";
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s += dt.Rows[i]["MaGV"].ToString() + "','";
                }
                s = s.Substring(0, s.Length - 2);
            }
            DataSet ds = dataConnection.OpenDataSet("SELECT MaGV,TenGV,BoMon,GhiChu,GVNgoai FROM GIANGVIEN_MAU WHERE MaGV NOT IN('"+s+")");
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
                    dt.Rows.Add(row.Cells["MaGV"].Value.ToString(),
                        row.Cells["TenGV"].Value.ToString(),
                        row.Cells["BoMon"].Value.ToString(),
                        row.Cells["GhiChu"].Value.ToString(),
                        row.Cells["GVNgoai"].Value.ToString());
                       
                }
                    
            }
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            DataSet ds = dataConnection.OpenDataSet("SELECT MaGV FROM GIANGVIEN_MAU");
            List<string> sMaGV = new List<string>();
            foreach (DataRow row in ds.Tables[0].Rows)
                sMaGV.Add(row["MaGV"].ToString());
            List<int> iMaGV = new List<int>();
            foreach (string s in sMaGV)
                iMaGV.Add(int.Parse(s));
            for (int i = 0; i < iMaGV.Count; i++)
                for (int j = i; j < iMaGV.Count; j++)
                {
                    if (iMaGV[i] < iMaGV[j])
                    {
                        int t = iMaGV[i];
                        iMaGV[i] = iMaGV[j];
                        iMaGV[j] = t;
                    }
                }
            string MaGV = (iMaGV[0] + 1).ToString();
           // using (frmLecturerDetails frmLecDe = new frmLecturerDetails(MaGV, 1, dataGridView1.Rows[dataGridView1.Rows.Count-1],dtTime))
            //    frmLecDe.ShowDialog();
            this.Show();
            dataGridView1.Refresh();
        }
    }
}
