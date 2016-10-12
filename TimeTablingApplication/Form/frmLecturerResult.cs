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
    public partial class frmLecturerResult : Form
    {
        clsSolution mySol;
        int MaGV;
        DataTable dtTime, dtRoom;
        public frmLecturerResult(string MaGV,clsSolution mySol, DataTable dtTime, DataTable dtRoom)
        {
            InitializeComponent();
            this.MaGV = int.Parse(MaGV)-1;
            this.mySol = mySol;
            this.dtTime = dtTime;
            this.dtRoom = dtRoom;
        }
        public void LoadForm()
        {
            List<int> arrProfIsIn = new List<int>();
            List<int> arrSpecialize = new List<int>();//1:advisor, 3:reviewer,4:chair,5:secretary
            for (int i = 0; i < mySol.iJury; i++)
            {
                if (mySol.arrJury[i].IsIn(MaGV) != -1)
                {
                    arrProfIsIn.Add(i);
                    arrSpecialize.Add(mySol.arrJury[i].IsIn(MaGV));
                }
            }
            dgvLect.Columns.Add("MaHD", "Mã hội đồng");
            dgvLect.Columns.Add("SVTH", "Sinh viên thực hiện");
            dgvLect.Columns.Add("TenHD", "Tên đề tài");
            dgvLect.Columns.Add("GVHD", "CBHD chính");
            dgvLect.Columns.Add("Ngay", "Ngày");
            dgvLect.Columns.Add("Gio", "Giờ");
            dgvLect.Columns.Add("Phong", "Phòng");
            dgvLect.Columns.Add("TuCach", "Tư cách");
            if (arrProfIsIn.Count>0)
                dgvLect.Rows.Add(arrProfIsIn.Count);
            for (int i = 0; i < arrProfIsIn.Count; i++)
            {
                dgvLect.Rows[i].Cells["MaHD"].Value = (mySol.arrJury[arrProfIsIn[i]].IdJury + 1).ToString();
                dgvLect.Rows[i].Cells["TenHD"].Value = mySol.arrJury[arrProfIsIn[i]].details.Name;
                if (arrSpecialize[i] == 1)//advisor
                    dgvLect.Rows[i].Cells["TuCach"].Value = "GVHD";
                if (arrSpecialize[i] == 3)//reviewer
                    dgvLect.Rows[i].Cells["TuCach"].Value = "Phản Biện";
                if (arrSpecialize[i] == 4)//chair
                    dgvLect.Rows[i].Cells["TuCach"].Value = "Chủ Tịch";
                if (arrSpecialize[i] == 5)//secretary
                    dgvLect.Rows[i].Cells["TuCach"].Value = "Thư Ký";
                dgvLect.Rows[i].Cells["Ngay"].Value = dtTime.Rows[mySol.arrJury[arrProfIsIn[i]].IdPeriod]["Ngay"].ToString();
                dgvLect.Rows[i].Cells["Gio"].Value = dtTime.Rows[mySol.arrJury[arrProfIsIn[i]].IdPeriod]["Gio"].ToString();
                dgvLect.Rows[i].Cells["Phong"].Value = dtRoom.Rows[mySol.arrJury[arrProfIsIn[i]].IdRoom]["TenPhong"].ToString();
                dgvLect.Rows[i].Cells["SVTH"].Value = mySol.arrJury[arrProfIsIn[i]].details.StudentName;
                dgvLect.Rows[i].Cells["GVHD"].Value = mySol.arrJury[arrProfIsIn[i]].advisor.details.Name;
            }

        }
        private void frmLecturerResult_Load(object sender, EventArgs e)
        {
            LoadForm();
        }
    }
}
