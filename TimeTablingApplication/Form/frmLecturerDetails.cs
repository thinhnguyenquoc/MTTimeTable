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
    public partial class frmLecturerDetails : Form
    {
        string MaGV;
        string TenGV="";
        string MaBM="";
        string GhiChu="";
        bool GVNgoai=false;
        string ChuyenMon="";
        string TGTG="";
        int TongTGTG =0;
        int iCase;//case=1: new, case=2: update
        DataConnection dataConnection;
        DataRow row;
        DataTable dtTime;
        public frmLecturerDetails(string MaGV, int iCase, DataRow row, DataTable dtTime)
        {
            InitializeComponent();
            this.MaGV = MaGV;
            this.iCase = iCase;
            dataConnection = new DataConnection();
            this.row = row;
            this.dtTime = dtTime;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmLecturerDetails_Load(object sender, EventArgs e)
        {
            cbBoMon.DropDownStyle = ComboBoxStyle.DropDownList;
            DataSet ds = dataConnection.OpenDataSet("SELECT * FROM BOMON");
            cbBoMon.DataSource = ds.Tables[0];
            cbBoMon.DisplayMember = "TenBM";
            cbBoMon.ValueMember = "MaBM";
            
            if (iCase == 1)//new
            {
                txtMaGV.Text = MaGV;
                txtMaGV.Enabled = false;
                txtChuyenMon.Text = "";
                txtGhiChu.Text = "";
                txtTenGV.Text = "";
                txtTGTG.Text = "";
                txtChuyenMon.Enabled = false;
                button3.Enabled = false;
                txtTGTG.Enabled = false;
                button4.Enabled = false;
            }
            if (iCase == 2)//update
            {
                txtMaGV.Text = MaGV;
                txtMaGV.Enabled = false;
                txtGhiChu.Text = row["GhiChu"].ToString();
                txtChuyenMon.Text = row["ChuyenMon"].ToString();
                txtTenGV.Text = row["TenGV"].ToString();
                txtTGTG.Text = row["TGTG"].ToString();
                if (bool.Parse(row["GVNgoai"].ToString()) == true)
                    rbYes.Checked = true;
                else
                    rbNo.Checked = true;
                cbBoMon.SelectedValue = row["BoMon"].ToString();
            }
        }

    

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (GVNgoai == false)
                GVNgoai = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (GVNgoai == true)
                GVNgoai = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            List<string> result = new List<string>();
            using (frmExperts frmExp = new frmExperts(result))
                frmExp.ShowDialog();
            this.Show();
            string strResult = "";
            if (result.Count != 0)
            {
                foreach (string s in result)
                    strResult += s + ",";
                strResult = strResult.Substring(0, strResult.Length - 1);
                txtChuyenMon.Text = strResult;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            List<string> result= new List<string>();
            using (frmLecturerTime frmLecTime = new frmLecturerTime(result,dtTime))
                frmLecTime.ShowDialog();
            this.Show();
            string strResult = "";
            foreach (string s in result)
                strResult += s + ",";
            if (strResult!=string.Empty) strResult = strResult.Substring(0, strResult.Length - 1);
            txtTGTG.Text = strResult;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TenGV = txtTenGV.Text;
            if (TenGV == "")
            { MessageBox.Show("Vui lòng điền tên giảng viên!"); return; }
            if (GVNgoai == null)
            { MessageBox.Show("Vui lòng chọn thông tin giảng viên ngoài trường!"); return; }
            
            MaBM = cbBoMon.SelectedValue.ToString();
            GhiChu = txtGhiChu.Text;
            string strSQL="";
            if (iCase == 1)
            {
                strSQL += "INSERT INTO GIANGVIEN_MAU VALUES ('";
                strSQL += MaGV + "','";
                strSQL += TenGV + "','";
                strSQL += MaBM + "','";
                strSQL += GhiChu + "',";
                strSQL += GVNgoai + ",'','',0,0,'')";
                
                
                row["GhiChu"] = GhiChu;
                
                row["TenGV"] = TenGV;
                
                row["GVNgoai"] = GVNgoai;
                row["BoMon"] = MaBM;
            }
            else
            {
                ChuyenMon = txtChuyenMon.Text;
                TGTG = txtTGTG.Text;
                string[] split = TGTG.Split(',');
                int count = 0;
                foreach (string s in split)
                    if (s != "") count++;
                TongTGTG = count;
            
                strSQL += "UPDATE GIANGVIEN_MAU SET TenGV='" + TenGV +
                    "',BoMon='" + MaBM + "',GhiChu='" + GhiChu +
                    "',GVNgoai=" + GVNgoai + ",ChuyenMon='" + ChuyenMon +
                    "',TGTG='" + TGTG + "',TongTGTG=" + TongTGTG
                    + " WHERE MaGV='" + MaGV + "'";
                row["GhiChu"] = GhiChu;
                row["ChuyenMon"] = ChuyenMon;
                row["TenGV"] = TenGV;
                row["TGTG"] = TGTG;
                row["GVNgoai"] = GVNgoai;
                row["BoMon"] = MaBM;
            }
            dataConnection.ExecuteUpdateSQL(strSQL);
            
            this.Dispose();
        }

    }
}
