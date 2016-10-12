using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
namespace TimeTablingApplication
{
    public partial class frmCreateNewSolution : Form
    {
        [DllImport("D://MyDLL.dll",CallingConvention=CallingConvention.Cdecl)]
        public static extern void myDllGenerateSolution(string inputFileFullPath);
        [DllImport("D://MyDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void myDllGenerateSolutionF(string inputFileFullPath);
        clsSolution mySol;
        DataTable dtLect;//datatable lecturer
        DataTable dtThesis;
        string MaGP;
        DataConnection dataConnection;
        DataTable dtTime;
        string strPath;
        public frmCreateNewSolution(string MaGP)
        {
            InitializeComponent();
            mySol = new clsSolution();
            dataConnection = new DataConnection();
            this.MaGP = MaGP;
            //Lecturer
            dtLect = new DataTable();
            dtLect.Columns.Add("MaGV");
            dtLect.Columns.Add("TenGV");
            dtLect.Columns.Add("BoMon");
            dtLect.Columns.Add("GhiChu");
            dtLect.Columns.Add("GVNgoai",typeof(bool));
            dtLect.Columns.Add("ChuyenMon");
            dtLect.Columns.Add("TGTG");
            dtLect.Columns.Add("TongTGTG");
            dtLect.Columns.Add("BTG");
            //Thesis
            dtThesis = new DataTable();
            dtThesis.Columns.Add("MaDT");
            dtThesis.Columns.Add("TenDT");
            dtThesis.Columns.Add("SVTH");
            dtThesis.Columns.Add("GVHD");
            dtThesis.Columns.Add("BoMon");
            //Time
            dtTime = new DataTable();
            dtTime.Columns.Add("MaTG");
            dtTime.Columns.Add("Ngay");
            dtTime.Columns.Add("Gio");
        }
        public frmCreateNewSolution(DataTable dtLect, DataTable dtThesis, DataTable dtTime)
        {
            this.dataConnection = new DataConnection();
            this.MaGP = FindSolutionCode();
            this.mySol = new clsSolution();
            this.dtLect = dtLect;
            this.dtThesis = dtThesis;
            this.dtTime = dtTime;
        }
        private string FindSolutionCode()
        {
            DataSet ds = dataConnection.OpenDataSet("SELECT MaGP FROM GIAIPHAP");
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
            return (iMaGP[0] + 1).ToString();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            //this.Hide();
            string MaGV_new = dtLect.Rows.Count.ToString();
            dtLect.Rows.Add(MaGV_new);
            
            using (frmLecturerDetails frmNewLect = new frmLecturerDetails(MaGV_new,1,dtLect.Rows[dtLect.Rows.Count-1],dtTime))
                frmNewLect.ShowDialog();
            this.Show();
            dataGridView1.DataSource = dtLect;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 1)
            { MessageBox.Show("Vui lòng chọn thông tin cần xóa"); return; }
            dtLect.Rows[dataGridView1.SelectedCells[0].RowIndex].Delete();
            dtLect.AcceptChanges();
            dataGridView1.Refresh();
        }

        private void CreateNewSolution_Load(object sender, EventArgs e)
        {
            dataGridView3.DataSource = dtTime;
            
            dataGridView1.DataSource = dtLect;
            
            dataGridView2.DataSource = dtThesis;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 1)
            { MessageBox.Show("Vui lòng chọn 1 giảng viên!"); return; }
            this.Hide();
            using (frmLecturerDetails frmLecDe = new frmLecturerDetails(dataGridView1.SelectedCells[0].OwningRow.Cells["MaGV"].Value.ToString(), 2,dtLect.Rows[int.Parse(dataGridView1.SelectedCells[0].OwningRow.Cells["MaGV"].Value.ToString())-1],dtTime))
                frmLecDe.ShowDialog();
            this.Show();
            dataGridView1.DataSource = dtLect;
            dataGridView1.Refresh();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //this.Hide();
            using (frmChooseThesis frmThesis = new frmChooseThesis(dtThesis))
                frmThesis.ShowDialog();
            dataGridView2.DataSource = dtThesis;

        }
        private void UpdateInformation()//reset data to create a whole new solution
        {
            dataGridView1.Sort(dataGridView1.Columns["GVNgoai"], ListSortDirection.Ascending);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["MaGV"].Value = i + 1;
            }
            //dataGridView1.Sort(dataGridView1.Columns["MaGV"], ListSortDirection.Ascending);
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                dataGridView2.Rows[i].Cells["MaDT"].Value = i + 1;
            }
            /*dataGridView2.Sort(dataGridView2.Columns["MaDT"], ListSortDirection.Ascending);
            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                dataGridView3.Rows[i].Cells["MaTG"].Value = i + 1;
            }
            //dataGridView3.Sort(dataGridView3.Columns["MaTG"], ListSortDirection.Ascending);
            */        
          }
        private void InsertIntoDatabase()//before scheduling
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string strSQL = "INSERT INTO GIANGVIEN VALUES('";
                strSQL += dataGridView1.Rows[i].Cells["MaGV"].Value.ToString() + "','";
                strSQL += dataGridView1.Rows[i].Cells["TenGV"].Value.ToString() + "','";
                strSQL += dataGridView1.Rows[i].Cells["BoMon"].Value.ToString() + "','";
                strSQL += dataGridView1.Rows[i].Cells["GhiChu"].Value.ToString() + "',";
                strSQL += bool.Parse(dataGridView1.Rows[i].Cells["GVNgoai"].Value.ToString()) + ",'','',0,0,'";
                strSQL += MaGP + "')";
                dataConnection.ExecuteUpdateSQL(strSQL);
            }
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                string strSQL = "INSERT INTO DETAI VALUES ('";
                strSQL += dataGridView2.Rows[i].Cells["MaDT"].Value.ToString() + "','";
                strSQL += dataGridView2.Rows[i].Cells["TenDT"].Value.ToString() + "','";
                strSQL += dataGridView2.Rows[i].Cells["SVTH"].Value.ToString() + "','";
                strSQL += dataGridView2.Rows[i].Cells["GVHD"].Value.ToString() + "','";
                strSQL += dataGridView2.Rows[i].Cells["BoMon"].Value.ToString() + "','";
                strSQL += MaGP + "')";
                dataConnection.ExecuteUpdateSQL(strSQL);
            }
            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                string strSQL = "INSERT INTO THOIGIAN VALUES ('";
                strSQL += dataGridView3.Rows[i].Cells["MaTG"].Value.ToString() + "','";
                strSQL += dataGridView3.Rows[i].Cells["Ngay"].Value.ToString() + "','";
                strSQL += dataGridView3.Rows[i].Cells["Gio"].Value.ToString() + "','";
                strSQL += MaGP + "')";
                dataConnection.ExecuteUpdateSQL(strSQL);
            }
        }
        private void UpdateDatabase()//after scheduling
        {
            string strPathOutput=strPath.Substring(0,strPath.LastIndexOf("\\"))+"\\output\\Solution_"+MaGP+"_input.txt.out";
            if (File.Exists(strPathOutput) == true)
            {
                mySol.ReadSolutionFromFile(strPathOutput);
                WriteJuryDatabase();
                WriteLectDatabase();
                WriteRoomDatabase();
            }
            else
                MessageBox.Show("Lập lịch thất bại!");
        }
        //Write PHONG
        private void WriteRoomDatabase()
        {
            DataTable dtTemp= dataConnection.OpenDataSet("SELECT * FROM PHONG_MAU").Tables[0];
            DataTable dtCopy = dtTemp.Clone();
            dtCopy.Columns["MaPhong"].DataType = typeof(int);
            foreach (DataRow r in dtTemp.Rows)
                dtCopy.ImportRow(r);
            DataView dv= dtCopy.DefaultView;
            dv.Sort="MaPhong asc";
            DataTable dtRoom = dv.ToTable();
            if (dtRoom.Rows.Count < mySol.numRoom)
            { MessageBox.Show("Không đủ phòng!"); return; }
            for (int i = 0; i < mySol.numRoom; i++)
            {
                string strSQL = "INSERT INTO PHONG VALUES ('";
                strSQL += (int.Parse(dtRoom.Rows[i]["MaPhong"].ToString())).ToString() + "','";
                strSQL += dtRoom.Rows[i]["TenPhong"].ToString() + "','";
                strSQL += MaGP + "')";
                dataConnection.ExecuteUpdateSQL(strSQL);
            }
        }
        //Write GIANGVIEN
        private void WriteLectDatabase()
        {
            for (int i = 0; i < mySol.arrLecturer.Count; i++)
            {
                string strSQL = "UPDATE GIANGVIEN SET GVNgoai=";
                strSQL += mySol.arrLecturer[i].IsExternalLect + ",ChuyenMon='";
                for (int j = 0; j < mySol.arrLecturer[i].arrSpecialize.Count; j++)
                    strSQL += mySol.arrLecturer[i].arrSpecialize[j] + ",";
                strSQL = strSQL.Substring(0, strSQL.Length - 1);//delete the last comma
                strSQL += "',TGTG='";
                for (int j = 0; j < mySol.arrLecturer[i].arrAvailTime.Count; j++)
                    strSQL += mySol.arrLecturer[i].arrAvailTime[j] + ",";
                strSQL = strSQL.Substring(0, strSQL.Length - 1);//delete the last comma
                strSQL += "',TongTGTG=";
                strSQL += mySol.arrLecturer[i].numAvailTime + ",BTG=";
                strSQL += mySol.arrLecturer[i].iSessionJoined + "";
                strSQL += " WHERE MaGV='" + (mySol.arrLecturer[i].IdLect + 1) 
                    + "' AND MaGP='"+MaGP+"'";
                dataConnection.ExecuteUpdateSQL(strSQL);

            }

        }
        //Write HOIDONG
        private void WriteJuryDatabase()
        {
            for (int i = 0; i < mySol.arrJury.Count; i++)
            {
                string strSQL = "INSERT INTO HOIDONG VALUES ('";
                strSQL += (mySol.arrJury[i].IdJury + 1) + "','";
                strSQL += MaGP + "','";
                strSQL += mySol.arrJury[i].IdJury + 1 + "','";
                strSQL += mySol.arrJury[i].advisor.IdLect + 1 + "','";
                strSQL += mySol.arrJury[i].arrReviewers[0].IdLect + 1 + "','";
                strSQL += mySol.arrJury[i].arrReviewers[1].IdLect + 1 + "','";
                strSQL += mySol.arrJury[i].arrOtherMembers[0].IdLect + 1 + "','";
                strSQL += mySol.arrJury[i].arrOtherMembers[1].IdLect + 1 + "','";
                strSQL += mySol.arrJury[i].IdPeriod + 1 + "','";
                strSQL += mySol.arrJury[i].IdRoom + 1 + "','";
                strSQL += mySol.arrJury[i].iNumExtLect + "','";
                for (int j = 0; j < mySol.arrJury[i].arrAvailPeriod.Count; j++)
                    strSQL += mySol.arrJury[i].arrAvailPeriod[j] + ",";
                strSQL = strSQL.Substring(0, strSQL.Length - 1);
                strSQL += "')";
                dataConnection.ExecuteUpdateSQL(strSQL);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Update();
            dataGridView1.Refresh();
            if (Validating() == true)
            {
                UpdateInformation();
                
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.Description = "Chọn nơi lưu trữ file backup!";
               
                fd.ShowDialog();
                strPath = fd.SelectedPath;
                if (strPath == string.Empty)
                {
                    MessageBox.Show("Vui lòng chọn nơi lưu trữ!");
                    //  fd.ShowDialog();
                }
                else
                {
                    InsertIntoDatabase();
                    if (strPath.Substring(strPath.Length - 2, 2) == "\\")
                        strPath = strPath.Substring(0, strPath.Length - 2);

                    if (Directory.Exists(strPath + "input") == false)
                        Directory.CreateDirectory(strPath + "input");
                    strPath += "input";
                    if (Directory.Exists(strPath + "\\output") == false)
                        Directory.CreateDirectory(strPath + "\\output");
                    strPath += "\\Solution_" + MaGP + "_input.txt";
                    WriteFileInput(strPath);
                    myDllGenerateSolution(strPath);
                    UpdateDatabase();
                    this.Hide();
                    frmSolutionResult frmSolRes = new frmSolutionResult(MaGP);
                    frmSolRes.ShowDialog();
                    this.Close();
                }
            }

        }
        private void WriteFileInput(string sFilePath)
        {
            StreamWriter myFile = new StreamWriter(sFilePath);

            if (myFile == null)
                MessageBox.Show("cannot open output file");
            int internalLect=0;
            int externalLect=0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (bool.Parse(row.Cells["GVNgoai"].Value.ToString())==false)
                    internalLect++;
                else
                    externalLect++;
            }
            int iJury=dataGridView2.Rows.Count;
            int Y=dataGridView3.Rows.Count;
            myFile.WriteLine(internalLect + "// number of internal lecturers: IT_1, ..., IT_m1");
            myFile.WriteLine(externalLect + "// number of external lecturers: IT_m1+1, ..., IT_m");
            myFile.WriteLine( iJury+ "// number of jury: J1, ..., Jn");
            myFile.WriteLine("0 "+Y + "//");
            myFile.WriteLine("// matrix ");
            for (int i = 0; i < iJury; i++)
            {
                myFile.Write("\tJ" + (i+1));
            }
            myFile.WriteLine();
            for (int i = 0; i < internalLect+externalLect; i++)
            {
                myFile.Write("IT" + (i + 1));
                for (int j = 0; j < iJury; j++)
                {
                    string[] split= dataGridView1.Rows[i].Cells["ChuyenMon"].Value.ToString().Split(',');
                    myFile.Write("\t" + split[j]);
                }
                myFile.WriteLine();
            }
            myFile.WriteLine("// unavailabity");
            for (int i = 0; i < internalLect+externalLect; i++)
            {
                myFile.Write("IT" + (i + 1));
                int busy = 0;
                string[] split = dataGridView1.Rows[i].Cells["TGTG"].Value.ToString().Split(',');
                for (int j = 0; j < split.Length; j++)
                {
                    if (split[j] == "1")
                    { myFile.Write(" " + (j+1)); busy++; }
                }
                if (busy == 0) myFile.Write(" ");
                myFile.WriteLine();
            }
            //myFile.WriteLine();
            myFile.Close();
        }
        private void UpdateDataGridView()//after scheduling
        {
            dtLect=dataConnection.OpenDataSet("SELECT * FROM GIANGVIEN WHERE MaGP='"+MaGP+"'").Tables[0];
            dataGridView1.DataSource = dtLect;
            //dtThesis=dataConnection.OpenDataSet(
        }
        private bool Validating()
        {
             int internalLect=0;
            int externalLect=0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (bool.Parse(row.Cells["GVNgoai"].Value.ToString())==false)
                    internalLect++;
                else
                    externalLect++;
            }
            int iJury=dataGridView2.Rows.Count;
            int Y=dataGridView3.Rows.Count;
            if (externalLect == 0)
            { MessageBox.Show("Thiếu dữ liệu về giảng viên ngoài trường!"); return false; }
            if (internalLect == 0)
            { MessageBox.Show("Thiếu dữ liệu về giảng viên trong trường!"); return false; }
            if (iJury == 0)
            { MessageBox.Show("Thiếu dữ liệu về đề tài!"); return false; }
            if (Y == 0)
            { MessageBox.Show("Thiếu dữ liệu về thời gian!"); return false; }
            
            foreach (DataGridViewRow row in dataGridView1.Rows)
                if (row.Cells["ChuyenMon"].Value.ToString().Split(',').Length != iJury)
                { MessageBox.Show("Giảng viên " + row.Cells["TenGV"].Value.ToString() + " thiếu dữ liệu chuyên môn!"); return false; }
            foreach (DataGridViewRow row in dataGridView1.Rows)
                if (row.Cells["TGTG"].Value.ToString().Split(',').Length != Y)
                { MessageBox.Show("Giảng viên " + row.Cells["TenGV"].Value.ToString() + " thiếu dữ liệu thời gian tham gia!"); return false; }

            return true;
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedCells.Count != 1)
            { MessageBox.Show("Vui lòng chọn thông tin cần xóa"); return; }
            dtThesis.Rows[dataGridView2.SelectedCells[0].RowIndex].Delete();
            dtThesis.AcceptChanges();
            dataGridView2.Refresh();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFDialog = new OpenFileDialog();
            openFDialog.Filter = "Excel files (.xls)|*.xls|All files (*.*)|*.*";
            openFDialog.FilterIndex = 1;
            openFDialog.Multiselect = false;
            

            // Process input if the user clicked OK.
            if (openFDialog.ShowDialog() == DialogResult.OK)
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Range range;

                string str;
                int rCnt = 0;//for row
                int cCnt = 0;//for column

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFDialog.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                range = xlWorkSheet.UsedRange;//read all sheet

                for (rCnt = 2; rCnt <= range.Rows.Count; rCnt++)//begin at row 2
                {
                    List<string> information = new List<string>();
                    for (cCnt = 1; cCnt <= 7; cCnt++)//read 7 columns
                    {
                        str = (range.Cells[rCnt, cCnt] as Excel.Range).Value2.ToString();
                        information.Add(str);
                    }
                    //Add a thesis into dataThesis
                    dtThesis.Rows.Add(information[0],information[4], information[1]+" "+ information[2],information[5]+" "+information[6], information[3]);
                }
                dataGridView2.DataSource = dtThesis;
                xlWorkBook.Close(true, null, null);
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
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            dataGridView1.Update();
            dataGridView1.Refresh();
            if (Validating() == true)
            {
                UpdateInformation();
                InsertIntoDatabase();
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.Description = "Chọn nơi lưu trữ file backup!";

                fd.ShowDialog();
                strPath = fd.SelectedPath;
                while (strPath == string.Empty)
                {
                    MessageBox.Show("Vui lòng chọn nơi lưu trữ!");
                    fd.ShowDialog();
                }
                if (Directory.Exists(strPath + "\\output") == false)
                    Directory.CreateDirectory(strPath + "\\output");
                strPath += "\\input.txt";
                WriteFileInput(strPath);
            }

        }

        private void button13_Click(object sender, EventArgs e)
        {
            //string strPathOutput = strPath.Substring(0, strPath.Substring(0, strPath.LastIndexOf("\\")).LastIndexOf("\\")) + "\\output\\input.txt.out";
            string strPathOutput="D:\\output\\input.txt.out";
            if (File.Exists(strPathOutput) == true)
            {
                mySol.ReadSolutionFromFile(strPathOutput);
                WriteJuryDatabase();
                WriteLectDatabase();
                WriteRoomDatabase();
                
                this.Hide();
                frmSolutionResult frmSolRes = new frmSolutionResult(MaGP);
                frmSolRes.ShowDialog();
                this.Close();
            }
            else
                MessageBox.Show("Lập lịch thất bại!");
            
        }

        private void button14_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFDialog = new OpenFileDialog();
            openFDialog.Filter = "Excel files (.xls)|*.xls|All files (*.*)|*.*";
            openFDialog.FilterIndex = 1;
            openFDialog.Multiselect = false;


            // Process input if the user clicked OK.
            if (openFDialog.ShowDialog() == DialogResult.OK)
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Range range;

                string str;
                int rCnt = 0;//for row
                int cCnt = 0;//for column

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFDialog.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                range = xlWorkSheet.UsedRange;//read all sheet

                for (rCnt = 2; rCnt <= range.Rows.Count; rCnt++)//begin at row 2
                {
                    List<string> information = new List<string>();
                    bool external = false;
                    for (cCnt = 1; cCnt <= 8; cCnt++)//read  columns
                    {
                        if (cCnt == 6)//column true false
                        {
                            external = bool.Parse((range.Cells[rCnt, cCnt] as Excel.Range).Value2.ToString());
                        }
                        else
                        {
                            if ((range.Cells[rCnt, cCnt] as Excel.Range).Value2 != null)
                                str = (range.Cells[rCnt, cCnt] as Excel.Range).Value2.ToString();
                            else
                                str = "";
                            information.Add(str);
                        }
                    }
                    //Add a lecturer into dtLect
                    dtLect.Rows.Add(information[0], information[1]+" "+ information[2], information[3], information[4], external, information[5],information[6]);
                }
                dataGridView1.DataSource = dtLect;
                
                xlWorkBook.Close(true, null, null);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFDialog = new OpenFileDialog();
            openFDialog.Filter = "Excel files (.xls)|*.xls|All files (*.*)|*.*";
            openFDialog.FilterIndex = 1;
            openFDialog.Multiselect = false;


            // Process input if the user clicked OK.
            if (openFDialog.ShowDialog() == DialogResult.OK)
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Range range;

                string str;
                int rCnt = 0;//for row
                int cCnt = 0;//for column

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFDialog.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                range = xlWorkSheet.UsedRange;//read all sheet

                for (rCnt = 2; rCnt <= range.Rows.Count; rCnt++)//begin at row 2
                {
                    List<string> information = new List<string>();

                    for (cCnt = 1; cCnt <= 3; cCnt++)//read 3 columns
                    {
                        if ((range.Cells[rCnt, cCnt] as Excel.Range).Value2 != null)
                            str = (range.Cells[rCnt, cCnt] as Excel.Range).Value2.ToString();
                        else
                            str = "";
                        information.Add(str);

                    }
                    //Add a time slot into dtTime  4
                    dtTime.Rows.Add(information[0], information[1], information[2]); ;
                }
                dataGridView3.DataSource = dtTime;

                xlWorkBook.Close(true, null, null);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            UpdateInformation();
            int iTime = 0;
            iTime = dataGridView3.Rows.Count;
            int iThesis = 0;
            iThesis = dataGridView2.Rows.Count;
            string sChuyenMon = "";
            for (int i = 0; i < iThesis; i++)
            {
                sChuyenMon += "3,";
            }
            sChuyenMon = sChuyenMon.Substring(0, sChuyenMon.Length - 1);
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["ChuyenMon"].Value = sChuyenMon;
            }
            dataGridView1.Refresh();
            string sTGTG = "";
            for (int i = 0; i < iTime; i++)
            {
                sTGTG += "0,";
            }
            sTGTG = sTGTG.Substring(0, sTGTG.Length - 1);
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["TGTG"].Value = sTGTG;
            }
            //update advisor
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                string sGVHD = row.Cells["GVHD"].Value.ToString();
                int iMaDT =int.Parse(row.Cells["MaDT"].Value.ToString());
                foreach (DataGridViewRow r in dataGridView1.Rows)
                    if (r.Cells["TenGV"].Value.ToString() == sGVHD)
                    {
                        string sCM=r.Cells["ChuyenMon"].Value.ToString();
                        StringBuilder sb = new StringBuilder(sCM);
                        sb[2 * iMaDT - 2] = '1';
                        r.Cells["ChuyenMon"].Value = sb.ToString();
                    }
            }




            dataGridView1.Refresh();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            dataGridView1.Update();
            dataGridView1.Refresh();
            if (Validating() == true)
            {
                UpdateInformation();
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.Description = "Chọn nơi lưu trữ file backup!";

                fd.ShowDialog();
                strPath = fd.SelectedPath;
                if (strPath == string.Empty)
                {
                    MessageBox.Show("Vui lòng chọn nơi lưu trữ!");

                }
                else
                {
                    InsertIntoDatabase();
                    if (strPath.Substring(strPath.Length - 2, 2) == "\\")
                        strPath = strPath.Substring(0, strPath.Length - 2);

                    if (Directory.Exists(strPath + "input") == false)
                        Directory.CreateDirectory(strPath + "input");
                    strPath += "input";
                    if (Directory.Exists(strPath + "\\output") == false)
                        Directory.CreateDirectory(strPath + "\\output");
                    strPath += "\\Solution_" + MaGP + "_input.txt";
                    WriteFileInput(strPath);
                    myDllGenerateSolutionF(strPath);
                    UpdateDatabase();
                    this.Hide();
                    frmSolutionResult frmSolRes = new frmSolutionResult(MaGP);
                    frmSolRes.ShowDialog();
                    this.Close();
                }
            }
        } 
    }
}
