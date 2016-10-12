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
    public partial class frmLecturerReplace : Form
    {
        clsSolution mySol;
        DataTable dtLect;
        int MaHD;
        int spec;
        public frmLecturerReplace(string MaHD, int spec, clsSolution mySol, DataTable dtLect)
        {
            InitializeComponent();
            this.MaHD = int.Parse(MaHD) - 1;
            this.spec = spec;
            this.mySol = mySol;
            this.dtLect = dtLect;
        }

        private void frmLecturerReplace_Load(object sender, EventArgs e)
        {
            List<int> arrLecturerCanReplace= new List<int>();
            if (spec == 31)
            {
                label1.Text += mySol.arrJury[MaHD].arrReviewers[0].details.Name;
                arrLecturerCanReplace = mySol.FindReviewerToReplace(MaHD, 0);
            }
            if (spec == 32)
            {
                label1.Text += mySol.arrJury[MaHD].arrReviewers[1].details.Name;
                arrLecturerCanReplace = mySol.FindReviewerToReplace(MaHD, 1);
            }
            if (spec == 4)
            {
                label1.Text += mySol.arrJury[MaHD].arrOtherMembers[0].details.Name;
                arrLecturerCanReplace = mySol.FindChairToReplace(MaHD);
            }
            if (spec == 5)
            {
                label1.Text += mySol.arrJury[MaHD].arrOtherMembers[1].details.Name;
                arrLecturerCanReplace = mySol.FindSecretaryToReplace(MaHD);
            }
            DataTable dtCopy = dtLect.Clone();
            foreach (int i in arrLecturerCanReplace)
            {
                DataRow row = dtCopy.NewRow();
                for (int j = 0; j < dtCopy.Columns.Count; j++)
                {
                    row[j] = dtLect.Rows[i][j].ToString();
                }
                dtCopy.Rows.Add(row);
            }
            dataGridView1.DataSource = dtCopy;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 1)
            { MessageBox.Show("Vui lòng chọn thông tin!"); return; }
            int IDLect_old=-1;
            int IDLectReplace = int.Parse(dataGridView1.SelectedCells[0].OwningRow.Cells["MaGV"].Value.ToString()) - 1;
            if (spec == 31)
            {
                IDLect_old = mySol.arrJury[MaHD].arrReviewers[0].IdLect;
                mySol.arrJury[MaHD].arrReviewers[0] = mySol.arrLecturer[IDLectReplace];
            }
            if (spec == 32)
            {
                IDLect_old = mySol.arrJury[MaHD].arrReviewers[1].IdLect;
                mySol.arrJury[MaHD].arrReviewers[1] = mySol.arrLecturer[IDLectReplace];
            }
            if (spec == 4)
            {
                IDLect_old = mySol.arrJury[MaHD].arrOtherMembers[0].IdLect;
                mySol.arrJury[MaHD].arrOtherMembers[0] = mySol.arrLecturer[IDLectReplace];
            }
            if (spec == 5)
            {
                IDLect_old = mySol.arrJury[MaHD].arrOtherMembers[1].IdLect;
                mySol.arrJury[MaHD].arrOtherMembers[1] = mySol.arrLecturer[IDLectReplace];
            }
            if (mySol.arrLecturer[IDLect_old].IsExternalLect != mySol.arrLecturer[IDLectReplace].IsExternalLect)
            {
                if (mySol.arrLecturer[IDLect_old].IsExternalLect == true)//lecturer used to be externa;
                    mySol.arrJury[MaHD].iNumExtLect--;
                else
                    mySol.arrJury[MaHD].iNumExtLect++;
            }
            this.Dispose();
        }
    }
}
