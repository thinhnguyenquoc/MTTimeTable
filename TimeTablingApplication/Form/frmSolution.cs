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
    public partial class frmSolution : Form
    {
        clsSolution mySol;
        DataConnection dataConnection;
        public frmSolution(clsSolution mySol, DataConnection dataConnection)
        {
            InitializeComponent();
            this.mySol = mySol;
            this.dataConnection = dataConnection;
        }

        private void frmSolution_Load(object sender, EventArgs e)
        {
            DataSet ds = dataConnection.OpenDataSet("SELECT * FROM HOIDONG");
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns["TGTC"].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 1)
            { MessageBox.Show("Vui lòng chọn thông tin cần chỉnh sửa!"); return; }
            if (dataGridView1.SelectedCells[0].OwningColumn.HeaderText=="MaHD"
                || dataGridView1.SelectedCells[0].OwningColumn.HeaderText == "MaDT"
                || dataGridView1.SelectedCells[0].OwningColumn.HeaderText == "GVNgoai"
                ||dataGridView1.SelectedCells[0].Value==null
                ||dataGridView1.SelectedCells[0].Value.ToString()==String.Empty)
            { MessageBox.Show("Thông tin này không thể chỉnh sửa!"); return; }
            //Change time to held the jury
            if (dataGridView1.SelectedCells[0].OwningColumn.HeaderText == "Ngay"||
                dataGridView1.SelectedCells[0].OwningColumn.HeaderText == "Gio")
            {
                this.Hide();
               // using (frmTime frmTime = new frmTime(int.Parse(dataGridView1.SelectedCells[0].OwningRow.Cells["MaDT"].Value.ToString()), mySol, dataConnection))
                //    frmTime.ShowDialog();

                this.Show();
                dataGridView1.SelectedCells[0].Value = mySol.arrJury[int.Parse(dataGridView1.SelectedCells[0].OwningRow.Cells["MaDT"].Value.ToString())].IdPeriod;
                dataGridView1.Refresh();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells[0].OwningColumn.HeaderText == "MaDT")
            {
                int ID = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
                string show = "Đề tài " + ID.ToString() + "\n Tên đề tài: ";
                show += mySol.arrJury[ID - 1].details.Name + "\n SVTH: " + mySol.arrJury[ID - 1].details.StudentName;
                MessageBox.Show(show);
            }
            if (dataGridView1.SelectedCells[0].OwningColumn.HeaderText == "GVHD"
                || dataGridView1.SelectedCells[0].OwningColumn.HeaderText == "GVPB1"
                || dataGridView1.SelectedCells[0].OwningColumn.HeaderText == "GVPB2"
                || dataGridView1.SelectedCells[0].OwningColumn.HeaderText == "ChuTich"
                || dataGridView1.SelectedCells[0].OwningColumn.HeaderText == "ThuKy")
            {
                int ID = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
                string show = "Giảng viên " + ID.ToString() + "\n Tên giảng viên: ";
                show += mySol.arrLecturer[ID - 1].details.Name;
                MessageBox.Show(show);
            }
            if (dataGridView1.SelectedCells[0].OwningColumn.HeaderText == "Ngay"||
                dataGridView1.SelectedCells[0].OwningColumn.HeaderText == "Gio")
            {
                DataSet dset = dataConnection.OpenDataSet("SELECT * FROM THOIGIAN WHERE MaTG='" + dataGridView1.SelectedCells[0].Value.ToString() + "'");
                string show = "Thời gian tổ chức: " + dset.Tables[0].Rows[0]["Ngay"].ToString() + " " + dset.Tables[0].Rows[0]["Gio"].ToString();
                MessageBox.Show(show);
                dset.Dispose();
            }
            if (dataGridView1.SelectedCells[0].OwningColumn.HeaderText == "Phong")
            {
                DataSet dset = dataConnection.OpenDataSet("SELECT * FROM PHONG WHERE MaPhong='" + dataGridView1.SelectedCells[0].Value.ToString() + "'");
                string show = "Phòng: " +dset.Tables[0].Rows[0]["TenPhong"].ToString();
                MessageBox.Show(show);
                dset.Dispose();
            }
            
        }
    }
}
