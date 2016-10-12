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
    public partial class frmExperts : Form
    {
        List<string> result;
        public frmExperts(List<string> result)
        {
            InitializeComponent();
            this.result = result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmExperts_Load(object sender, EventArgs e)
        {
            DataConnection dataConnection = new DataConnection();
            DataSet ds = dataConnection.OpenDataSet("SELECT MaDT,TenDT FROM DETAI_MAU");
            dataGridView1.DataSource = ds.Tables[0];
            
            DataGridViewComboBoxColumn c = new DataGridViewComboBoxColumn();
            c.HeaderText = "Khả năng chuyên môn";
            c.Name = "ChuyenMon";
            c.Items.Add("Chủ tịch/Thư ký");
            c.Items.Add("GVHD");
            c.Items.Add("Không tham gia/Đồng hướng dẫn");
            c.Items.Add("Phản biện");
            
            dataGridView1.Columns.Add(c);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
                if (row.Cells["ChuyenMon"].Value != null)
                    count++;
            if (count != dataGridView1.Rows.Count)
            { MessageBox.Show("Vui lòng chọn đầy đủ thông tin!"); return; }
            for (int i=0;i<count;i++)
                result.Add(dataGridView1.Rows[i].Cells["ChuyenMon"].RowIndex.ToString());
            this.Dispose();
        }
    }
}
