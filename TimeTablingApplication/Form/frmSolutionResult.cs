using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTextSharp;
using iTextSharp.text;
using Excel = Microsoft.Office.Interop.Excel;

namespace TimeTablingApplication
{
    public partial class frmSolutionResult : Form
    {
        string MaGP;
        DataConnection dataConnection;
        DataTable dtLect, dtJury, dtRoom,dtTime;
        DataTable dtJuryOriginal;
        clsSolution mySol;
        public frmSolutionResult(string MaGP)
        {
            InitializeComponent();
            this.MaGP = MaGP;
            dataConnection = new DataConnection();
            dtJuryOriginal = new DataTable();
            dtJury = new DataTable();
            dtLect = new DataTable();
            dtRoom = new DataTable();
            dtTime = new DataTable();
            mySol = new clsSolution();
        }

        private void dgvLecturer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmSolutionResult_Load(object sender, EventArgs e)
        {
            mySol.ReadSolutionFromDatabase(MaGP, dataConnection);
            DataTable dtTemp = dataConnection.OpenDataSet("SELECT MaHD,MaDT,GVHD,GVPB1,GVPB2,ChuTich,ThuKy,ThoiGian,GVNgoai,Phong FROM HOIDONG WHERE MaGP='" + MaGP + "'").Tables[0];
            DataTable dtCopy = dtTemp.Clone();
            dtCopy.Columns["MaHD"].DataType = typeof(int);
            foreach (DataRow r in dtTemp.Rows)
                dtCopy.ImportRow(r);
            DataView dv = dtCopy.DefaultView;
            dv.Sort = "MaHD asc";
            dtJury = dv.ToTable();
            dtJuryOriginal = dtJury.Copy();
            dgvJury.DataSource = dtJury;
            dgvJury.Columns["MaDT"].HeaderText = "Tên đề tài";
            dgvJury.Columns["GVNgoai"].HeaderText = "Số giảng viên ngoài";

            dgvJury.Columns["GVNgoai"].Visible = false;
            dtTemp.Clear();
            dtCopy.Clear();
            dtTemp = dataConnection.OpenDataSet("SELECT MaGV,TenGV,BoMon,GhiChu FROM GIANGVIEN WHERE MaGP='" + MaGP + "'").Tables[0];
            dtCopy = dtTemp.Clone();
            dtCopy.Columns["MaGV"].DataType = typeof(int);
            foreach (DataRow r in dtTemp.Rows)
                dtCopy.ImportRow(r);
            dv = dtCopy.DefaultView;
            dv.Sort = "MaGV asc";
            dtLect = dv.ToTable();
            dgvLecturer.DataSource = dtLect;

            dtTemp.Clear();
            dtCopy.Clear();
            dtTemp = dataConnection.OpenDataSet("SELECT MaPhong,TenPhong FROM PHONG WHERE MaGP='" + MaGP + "'").Tables[0];
            dtCopy = dtTemp.Clone();
            dtCopy.Columns["MaPhong"].DataType = typeof(int);
            foreach (DataRow r in dtTemp.Rows)
                dtCopy.ImportRow(r);
            dv = dtCopy.DefaultView;
            dv.Sort = "MaPhong asc";
            dtRoom = dv.ToTable();
            dgvRoom.DataSource = dtRoom;

            dtTemp.Clear();
            dtCopy.Clear();
            dtTemp = dataConnection.OpenDataSet("SELECT MaTG,Ngay,Gio FROM THOIGIAN WHERE MaGP='" + MaGP + "'").Tables[0];
            dtCopy = dtTemp.Clone();
            dtCopy.Columns["MaTG"].DataType = typeof(int);
            foreach (DataRow r in dtTemp.Rows)
                dtCopy.ImportRow(r);
            dv = dtCopy.DefaultView;
            dv.Sort = "MaTG asc";
            dtTime = dv.ToTable();
            
            foreach (DataGridViewRow row in dgvJury.Rows)
            {
                int MaHD = int.Parse(row.Cells["MaHD"].Value.ToString()) - 1;
                row.Cells["MaDT"].Value = mySol.arrJury[MaHD].details.Name;
                row.Cells["GVHD"].Value = mySol.arrJury[MaHD].advisor.details.Name;
                row.Cells["GVPB1"].Value = mySol.arrJury[MaHD].arrReviewers[0].details.Name;
                row.Cells["GVPB2"].Value = mySol.arrJury[MaHD].arrReviewers[1].details.Name;
                row.Cells["ChuTich"].Value = mySol.arrJury[MaHD].arrOtherMembers[0].details.Name;
                row.Cells["ThuKy"].Value = mySol.arrJury[MaHD].arrOtherMembers[1].details.Name;
                row.Cells["ThoiGian"].Value = dtTime.Rows[mySol.arrJury[MaHD].IdPeriod]["Ngay"].ToString() + " " + dtTime.Rows[mySol.arrJury[MaHD].IdPeriod]["Gio"].ToString();
                row.Cells["Phong"].Value = dtRoom.Rows[mySol.arrJury[MaHD].IdRoom]["TenPhong"].ToString();
            }

           
        }

        private void dgvLecturer_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dgvLecturer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string MaGV = dgvLecturer.SelectedCells[0].OwningRow.Cells["MaGV"].Value.ToString();
            frmLecturerResult frmLectRes = new frmLecturerResult(MaGV, mySol, dtTime, dtRoom);
            frmLectRes.ShowDialog();
        }

        private void dgvRoom_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string MaPhong = dgvRoom.SelectedCells[0].OwningRow.Cells["MaPhong"].Value.ToString();
            frmRoomResult frmRoomRes = new frmRoomResult(MaPhong, mySol, dtTime, dtRoom);
            frmRoomRes.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvJury.SelectedCells.Count != 1)
            { MessageBox.Show("Vui lòng chọn 1 hội đồng!"); return; }
            string IDJuryNeedToMove = dgvJury.SelectedCells[0].OwningRow.Cells["MaHD"].Value.ToString();
            frmTime frmTime = new frmTime(IDJuryNeedToMove, mySol, dtTime);
            frmTime.Show();
            dgvJury.SelectedCells[0].OwningRow.Cells["ThoiGian"].Value = dtTime.Rows[mySol.arrJury[int.Parse(IDJuryNeedToMove) - 1].IdPeriod]["Ngay"].ToString() + " " + dtTime.Rows[mySol.arrJury[int.Parse(IDJuryNeedToMove) - 1].IdPeriod]["Gio"].ToString();
            
            dgvJury.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int spec = -1;
            if (dgvJury.SelectedCells.Count != 1)
            { MessageBox.Show("Vui lòng chọn 1 giảng viên!"); return; }
            if (dgvJury.SelectedCells[0].OwningColumn.HeaderText == "GVHD")
            { MessageBox.Show("Không thể thay đổi giảng viên hướng dẫn!"); return; }
            if (dgvJury.SelectedCells[0].OwningColumn.HeaderText == "GVPB1")
                spec = 31;
            if (dgvJury.SelectedCells[0].OwningColumn.HeaderText == "GVPB2")
                spec = 32;
            if (dgvJury.SelectedCells[0].OwningColumn.HeaderText == "ChuTich")
                spec = 4;
            if (dgvJury.SelectedCells[0].OwningColumn.HeaderText == "ThuKy")
                spec = 5;
            if (spec==-1)
            { MessageBox.Show("Vui lòng chọn 1 giảng viên!"); return; }
            string MaHD = dgvJury.SelectedCells[0].OwningRow.Cells["MaHD"].Value.ToString();
            frmLecturerReplace frmLectRep = new frmLecturerReplace(MaHD, spec, mySol, dtLect);
            frmLectRep.ShowDialog();
            if (spec == 31)
                dgvJury.SelectedCells[0].Value=mySol.arrJury[int.Parse(MaHD)-1].arrReviewers[0].details.Name;
            if (spec == 32)
                dgvJury.SelectedCells[0].Value=mySol.arrJury[int.Parse(MaHD)-1].arrReviewers[1].details.Name;
            if (spec == 4)
                dgvJury.SelectedCells[0].Value=mySol.arrJury[int.Parse(MaHD)-1].arrOtherMembers[0].details.Name;
            if (spec == 5)
                dgvJury.SelectedCells[0].Value = mySol.arrJury[int.Parse(MaHD) - 1].arrOtherMembers[1].details.Name;
            dgvJury.SelectedCells[0].OwningRow.Cells["GVNgoai"].Value = mySol.arrJury[int.Parse(MaHD) - 1].iNumExtLect.ToString();
            dgvJury.Refresh();
        }
        private void SaveChanges()
        {
            for (int i = 0; i < dtJuryOriginal.Rows.Count; i++)
            {
                if (dtJuryOriginal.Rows[i]["GVPB1"].ToString().Equals((mySol.arrJury[i].arrReviewers[0].IdLect + 1).ToString()) == false ||
                    dtJuryOriginal.Rows[i]["GVPB2"].ToString().Equals((mySol.arrJury[i].arrReviewers[1].IdLect + 1).ToString()) == false ||
                    dtJuryOriginal.Rows[i]["ChuTich"].ToString().Equals((mySol.arrJury[i].arrOtherMembers[0].IdLect + 1).ToString()) == false ||
                    dtJuryOriginal.Rows[i]["ThuKy"].ToString().Equals((mySol.arrJury[i].arrOtherMembers[1].IdLect + 1).ToString()) == false ||
                    dtJuryOriginal.Rows[i]["ThoiGian"].ToString().Equals((mySol.arrJury[i].IdPeriod + 1).ToString()) == false)
                {
                    string strSQL = "UPDATE HOIDONG SET GVPB1='";
                    strSQL += (mySol.arrJury[i].arrReviewers[0].IdLect+1) + "',GVPB2='";
                    strSQL += (mySol.arrJury[i].arrReviewers[1].IdLect+1) + "',ChuTich='";
                    strSQL += (mySol.arrJury[i].arrOtherMembers[0].IdLect+1) + "',ThuKy='";
                    strSQL += (mySol.arrJury[i].arrOtherMembers[1].IdLect+1) + "',ThoiGian='";
                    strSQL += (mySol.arrJury[i].IdPeriod+1) + "' WHERE MaHD='";
                    strSQL += (i+1) + "' AND MaGP='" + MaGP + "'";
                    dataConnection.ExecuteUpdateSQL(strSQL);
                }
            }
        }
        private bool CheckIfThereIsChanges()
        {
            for (int i = 0; i < dtJuryOriginal.Rows.Count; i++)
            {
                if (dtJuryOriginal.Rows[i]["GVPB1"].ToString().Equals((mySol.arrJury[i].arrReviewers[0].IdLect+1).ToString())==false ||
                    dtJuryOriginal.Rows[i]["GVPB2"].ToString().Equals((mySol.arrJury[i].arrReviewers[1].IdLect+1).ToString())==false ||
                    dtJuryOriginal.Rows[i]["ChuTich"].ToString().Equals((mySol.arrJury[i].arrOtherMembers[0].IdLect+1).ToString())==false ||
                    dtJuryOriginal.Rows[i]["ThuKy"].ToString().Equals((mySol.arrJury[i].arrOtherMembers[1].IdLect+1).ToString())==false ||
                    dtJuryOriginal.Rows[i]["ThoiGian"].ToString().Equals((mySol.arrJury[i].IdPeriod+1).ToString())==false)
                    return true;
            }
            return false;            
        }
        private void frmSolutionResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CheckIfThereIsChanges() == true)
            {
                DialogResult result = MessageBox.Show("Lưu thay đổi?", "Thoát",
                 MessageBoxButtons.YesNoCancel,
                 MessageBoxIcon.Stop);
                if (result == DialogResult.Cancel)
                { e.Cancel = true; return; }
                if (result == DialogResult.No)
                { this.Dispose(); }
                if (result == DialogResult.Yes)
                {
                    SaveChanges();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PrintInvitation();
        }
        private void PrintInvitation()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            string strPath="";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                strPath=sfd.FileName;

                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                int countRow = 1;
                int i = 0;
                int j = 0;
                for (int lec = 0; lec < dgvLecturer.Rows.Count; lec++)
                {
                    frmLecturerResult frmlecRe = new frmLecturerResult(dgvLecturer.Rows[lec].Cells["MaGV"].Value.ToString(), mySol, dtTime, dtRoom);
                    frmlecRe.LoadForm();
                    xlWorkSheet.Cells[countRow, 1] = "Giảng viên:";
                    xlWorkSheet.Cells[countRow, 2] = dgvLecturer.Rows[lec].Cells["TenGV"].Value.ToString();
                    countRow++;
                    xlWorkSheet.Cells[countRow, 1] = "STT";
                    xlWorkSheet.Cells[countRow, 2] = "Sinh viên thực hiện";
                    xlWorkSheet.Cells[countRow, 3] = "Tên đề tài";
                    xlWorkSheet.Cells[countRow, 4] = "CBHD chính";
                    xlWorkSheet.Cells[countRow, 5] = "Ngày";
                    xlWorkSheet.Cells[countRow, 6] = "Giờ";
                    xlWorkSheet.Cells[countRow, 7] = "Phòng";
                    xlWorkSheet.Cells[countRow, 7] = "Tư cách";
                    for (i = 0; i <= frmlecRe.dgvLect.RowCount - 1; i++)
                    {
                        for (j = 0; j <= frmlecRe.dgvLect.ColumnCount - 1; j++)
                        {
                            DataGridViewCell cell = frmlecRe.dgvLect[j, i];
                            if (j != 0)
                                xlWorkSheet.Cells[countRow + 1, j + 1] = cell.Value;
                            else
                                xlWorkSheet.Cells[countRow + 1, j + 1] = i + 1;
                        }
                        countRow++;
                    }
                    frmlecRe.Dispose();
                    countRow++;
                    countRow++;
                }
                xlWorkBook.SaveAs(strPath, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

            }
            
        }

    private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

    private void button4_Click(object sender, EventArgs e)
    {
        DataTable dtLect_over = new DataTable();
        dtLect_over = dataConnection.OpenDataSet("SELECT MaGV,TenGV,BoMon,GhiChu,GVNgoai,ChuyenMon FROM GIANGVIEN WHERE MaGP='" + MaGP + "'").Tables[0];
        DataTable dtThesis_over = new DataTable();
        dtThesis_over = dataConnection.OpenDataSet("SELECT MaDT,TenDT,SVTH,GVHD,BoMon FROM DETAI WHERE MaGP='" + MaGP + "'").Tables[0];
        DataTable dtTime_over = new DataTable();
        dtTime_over = dataConnection.OpenDataSet("SELECT MaTG,Ngay,Gio FROM THOIGIAN WHERE MaGP='" +MaGP+ "'").Tables[0];
        frmCreateNewSolution frmCreSol = new frmCreateNewSolution(dtLect_over, dtThesis_over, dtTime_over);
        frmCreSol.ShowDialog();
        this.Close();
    }

    private void button5_Click(object sender, EventArgs e)
    {
        SaveFileDialog sfd = new SaveFileDialog();
        string strPath = "";
        if (sfd.ShowDialog() == DialogResult.OK)
        {
            strPath = sfd.FileName;

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int countRow = 1;
            xlWorkSheet.Cells[1, 1] = "STT";
            xlWorkSheet.Cells[1, 2] = "Tên đề tài";
            xlWorkSheet.Cells[1, 3] = "GVHD chính";
            xlWorkSheet.Cells[1, 4] = "GVPB 1";
            xlWorkSheet.Cells[1, 5] = "GVPB 2";
            xlWorkSheet.Cells[1, 6] = "Chủ tịch";
            xlWorkSheet.Cells[1, 7] = "Thư ký";
            xlWorkSheet.Cells[1, 8] = "Thời gian";
            xlWorkSheet.Cells[1, 10] = "Phòng";
            
            for (int i = 0; i < dgvJury.Rows.Count; i++)
            {
                for (int j = 0; j < dgvJury.Columns.Count; j++)
                {
                    DataGridViewCell cell = dgvJury[j, i];
                    if (j != 8)//GVNgoai
                    {
                        if (j != 0)//STT
                            xlWorkSheet.Cells[countRow + 1, j + 1] = cell.Value;
                        else
                            xlWorkSheet.Cells[countRow + 1, j + 1] = i + 1;
                    }
                }
                        countRow++;
            }
            xlWorkBook.SaveAs(strPath, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

        }
    }
    }
}
