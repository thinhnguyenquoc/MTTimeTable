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
    public partial class frmRoomResult : Form
    {
        clsSolution mySol;
        int MaPhong;
        DataTable dtTime, dtRoom;
        public frmRoomResult(string MaPhong, clsSolution mySol, DataTable dtTime, DataTable dtRoom)
        {
            InitializeComponent();
            this.MaPhong = int.Parse(MaPhong) - 1;
            this.mySol = mySol;
            this.dtTime = dtTime;
            this.dtRoom = dtRoom;
        }

        private void frmRoomResult_Load(object sender, EventArgs e)
        {
            List<int> arrJuryInRoom = new List<int>();
            for (int i = 0; i < mySol.iJury; i++)
            {
                if (mySol.arrJury[i].IdRoom == MaPhong)
                    arrJuryInRoom.Add(i);
            }
            dgvRoom.Columns.Add("MaHD", "Mã hội đồng");
            dgvRoom.Columns.Add("TenDT", "Tên đề tài");
            dgvRoom.Columns.Add("SVTH", "Sinh viên thực hiện");
            dgvRoom.Columns.Add("Ngay", "Ngày");
            dgvRoom.Columns.Add("Gio", "Giờ");
            dgvRoom.Rows.Add(arrJuryInRoom.Count);
            for (int i = 0; i < arrJuryInRoom.Count; i++)
            {
                dgvRoom.Rows[i].Cells["MaHD"].Value = (mySol.arrJury[arrJuryInRoom[i]].IdJury+1).ToString();
                dgvRoom.Rows[i].Cells["TenDT"].Value = mySol.arrJury[arrJuryInRoom[i]].details.Name;
                dgvRoom.Rows[i].Cells["SVTH"].Value = mySol.arrJury[arrJuryInRoom[i]].details.StudentName;
                dgvRoom.Rows[i].Cells["Ngay"].Value = dtTime.Rows[mySol.arrJury[arrJuryInRoom[i]].IdPeriod]["Ngay"].ToString();
                dgvRoom.Rows[i].Cells["Gio"].Value = dtTime.Rows[mySol.arrJury[arrJuryInRoom[i]].IdPeriod]["Gio"].ToString();
            }
        }
    }
}
