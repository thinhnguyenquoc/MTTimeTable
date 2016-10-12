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
    public partial class frmScheduling : Form
    {
        DataConnection dataConnection;
        clsSolution mySol;
        public frmScheduling()
        {
            InitializeComponent();
            dataConnection = new DataConnection();
            mySol = new clsSolution();
        }

        private void frmScheduling_Load(object sender, EventArgs e)
        {
            //Create data for mySol
            DataSet dsLect = dataConnection.OpenDataSet("SELECT * FROM GIANGVIEN WHERE MaGV IN ('1','2','3','4','5','6','7','8')");
            dataGridView1.DataSource = dsLect.Tables[0];
            DataSet dsSubject = dataConnection.OpenDataSet("SELECT * FROM DETAI WHERE MaDT IN ('1','2','3','4','5')");
            dataGridView2.DataSource = dsSubject.Tables[0];
            DataSet dsRoom = dataConnection.OpenDataSet("SELECT * FROM PHONG");
            dataGridView3.DataSource = dsRoom.Tables[0];
            DataSet dsTemp = dataConnection.OpenDataSet("SELECT * FROM GIANGVIEN WHERE GVNgoai=False AND MaGV IN ('1','2','3','4','5','6','7','8')");
            mySol.iIntLect = dsTemp.Tables[0].Rows.Count;
            dsTemp = dataConnection.OpenDataSet("SELECT * FROM GIANGVIEN WHERE GVNgoai=True AND MaGV IN ('1','2','3','4','5','6','7','8')");
            mySol.iExtLect = dsTemp.Tables[0].Rows.Count;
            dsTemp = dataConnection.OpenDataSet("SELECT * FROM HOIDONG");
            mySol.iJury = dsTemp.Tables[0].Rows.Count;
            dsTemp = dataConnection.OpenDataSet("SELECT * FROM THOIGIAN");
            mySol.Y = dsTemp.Tables[0].Rows.Count;
            //read data for each lecturer
            for (int i = 0; i < mySol.iIntLect + mySol.iExtLect; i++)
            {
                clsLecturer lecturer = new clsLecturer();
                lecturer.IdLect = int.Parse(dsLect.Tables[0].Rows[i]["MaGV"].ToString())-1;
                //read IsExternalLect
                lecturer.IsExternalLect = bool.Parse(dsLect.Tables[0].Rows[i]["GVNgoai"].ToString());
                //read arrSpecialize
                string myLine = dsLect.Tables[0].Rows[i]["ChuyenMon"].ToString();
                string[] split = myLine.Split(',');
                foreach (string s in split)
                {
                    if (s != string.Empty)
                        lecturer.arrSpecialize.Add(int.Parse(s));
                }
                myLine = dsLect.Tables[0].Rows[i]["TGTG"].ToString();//read arrAvailTime
                split = myLine.Split(',');
                foreach (string s in split)
                {
                    if (s != string.Empty)
                        lecturer.arrAvailTime.Add(int.Parse(s));
                }
                lecturer.numAvailTime = int.Parse(dsLect.Tables[0].Rows[i]["TongTGTG"].ToString());
                lecturer.Y = mySol.Y;
                lecturer.iSessionJoined = int.Parse(dsLect.Tables[0].Rows[i]["BTG"].ToString());
                lecturer.details = new LecturerDetails(lecturer.IdLect, dsLect.Tables[0].Rows[i]["TenGV"].ToString());
                mySol.arrLecturer.Add(lecturer);
            }
            for (int i = 0; i < mySol.iJury; i++)
                mySol.arrIsChoosePeriod.Add(false);
            //read data for each jury
            DataSet dsJury = dataConnection.OpenDataSet("SELECT * FROM HOIDONG");
            for (int i = 0; i < mySol.iJury; i++)
            {
                clsJury jury = new clsJury();
                jury.IdJury = int.Parse(dsJury.Tables[0].Rows[i]["MaHD"].ToString())-1;
                
                //read advisor, reviewers, others
                jury.advisor = mySol.arrLecturer[int.Parse(dsJury.Tables[0].Rows[i]["GVHD"].ToString())-1];
                jury.arrReviewers.Add(mySol.arrLecturer[int.Parse(dsJury.Tables[0].Rows[i]["GVPB1"].ToString())-1]);
                jury.arrReviewers.Add(mySol.arrLecturer[int.Parse(dsJury.Tables[0].Rows[i]["GVPB2"].ToString())-1]);
                jury.arrOtherMembers.Add(mySol.arrLecturer[int.Parse(dsJury.Tables[0].Rows[i]["ChuTich"].ToString())-1]);
                jury.arrOtherMembers.Add(mySol.arrLecturer[int.Parse(dsJury.Tables[0].Rows[i]["ThuKy"].ToString())-1]);
                jury.iNumExtLect = int.Parse(dsJury.Tables[0].Rows[i]["GVNgoai"].ToString());
                jury.IdRoom = int.Parse(dsJury.Tables[0].Rows[i]["Phong"].ToString())-1;
                jury.IdPeriod = int.Parse(dsJury.Tables[0].Rows[i]["ThoiGian"].ToString())-1;
                if (jury.IdPeriod>=0) mySol.arrIsChoosePeriod[jury.IdJury] = true;
                //read available time for jury
                string myLine = dsJury.Tables[0].Rows[i]["TGTC"].ToString();
                string[] split = myLine.Split(',');
                foreach (string s in split)
                    if (s != string.Empty)
                        jury.arrAvailPeriod.Add(bool.Parse(s));
                string name = "";
                string stu="";
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.HeaderText == "MaDT" && (int.Parse(cell.Value.ToString())-1) == jury.IdJury)
                        {
                            name = cell.OwningRow.Cells["TenDT"].Value.ToString();
                            stu = cell.OwningRow.Cells["SVTH"].Value.ToString();
                            break;
                        }
                    }
                    if (name != string.Empty)
                        break;
                }
                jury.details = new JuryDetails(jury.IdJury, name, stu);


                mySol.arrJury.Add(jury);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (frmLecturer frmLect = new frmLecturer(mySol, dataConnection, (DataTable)dataGridView1.DataSource))
            frmLect.ShowDialog();
            this.Show();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (frmSolution frmSol = new frmSolution(mySol, dataConnection))
                frmSol.ShowDialog();
            this.Show();
        }
    }
}
