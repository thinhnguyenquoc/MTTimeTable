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
    public partial class frmSolutionDetails : Form
    {
        string MaGP;
        string TenGP;
        string Khoa;
        string ChuyenNganh;
        int HocKy;
        int NamHoc;
        string GhiChu;
        public frmSolutionDetails()
        {
            InitializeComponent();
        }

        private void frmSolutionDetails_Load(object sender, EventArgs e)
        {
            DataConnection dataConnection = new DataConnection();
            DataSet ds = dataConnection.OpenDataSet("SELECT MaGP FROM GIAIPHAP");
            int iMaGPFinal = 1;
            if (ds.Tables[0].Rows.Count > 0)
            {
                List<string> sMaGP = new List<string>();
                foreach (DataRow row in ds.Tables[0].Rows)
                    sMaGP.Add(row["MaGP"].ToString());
                List<int> iMaGP = new List<int>();
                foreach (string s in sMaGP)
                    iMaGP.Add(int.Parse(s));
                for (int i = 0; i < iMaGP.Count; i++)
                    for (int j = i; j < iMaGP.Count; j++)
                    {
                        if (iMaGP[i] < iMaGP[j])
                        {
                            int t = iMaGP[i];
                            iMaGP[i] = iMaGP[j];
                            iMaGP[j] = t;
                        }
                    }
                iMaGPFinal = iMaGP[0] + 1;
            }
            MaGP = (iMaGPFinal).ToString();
            txtMaGP.Text = MaGP;
            txtMaGP.Enabled = false;
            txtKhoa.Text = "Khoa KH&KTMT";
            txtChuyenNganh.Text = "KHMT";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int re;
            if (int.TryParse(txtNamHoc.Text,out re) == false)
            { MessageBox.Show("Năm học phải là số!"); return; }
            TenGP = txtTenGP.Text;
            Khoa = txtKhoa.Text;
            ChuyenNganh = txtChuyenNganh.Text;
            if (cbHocKy.SelectedItem!=null)
                HocKy = int.Parse(cbHocKy.SelectedItem.ToString());
            if (txtNamHoc.Text!=string.Empty)
                NamHoc= int.Parse(txtNamHoc.Text);
            GhiChu = txtGhiChu.Text;
            if (TenGP == string.Empty || Khoa == string.Empty || ChuyenNganh == string.Empty ||
                cbHocKy.SelectedItem==null || txtNamHoc.Text == string.Empty)
            { MessageBox.Show("Vui lòng điền đầy đủ thông tin!"); return; }
            string strSQL = "INSERT INTO GIAIPHAP VALUES('";
            strSQL += MaGP + "','";
            strSQL += TenGP + "','";
            strSQL += Khoa + "','";
            strSQL += ChuyenNganh + "',";
            strSQL += HocKy + ",";
            strSQL += NamHoc + ",'";
            strSQL += GhiChu + "')";
            DataConnection dataConnection = new DataConnection();
            dataConnection.ExecuteUpdateSQL(strSQL);

            frmCreateNewSolution frmCreSol = new frmCreateNewSolution(this.MaGP);
            this.Hide();
            frmCreSol.ShowDialog();
            this.Close();
            
        }
    }
}
