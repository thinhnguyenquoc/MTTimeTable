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
using System.IO;
using iTextSharp.text.pdf;
using Excel = Microsoft.Office.Interop.Excel;

namespace TimeTablingApplication
{
    public partial class Form2 : Form
    {
        clsSolution mySol;
        DataConnection dataConnection;
        public Form2()
        {
            InitializeComponent();
            mySol = new clsSolution();
            dataConnection = new DataConnection();

        }

        private void btInputFileName_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFDialog = new OpenFileDialog();
            openFDialog.ShowDialog();
            txtInputFileName.Text = openFDialog.FileName;
        }

        private void btRun_Click(object sender, EventArgs e)
        {

            mySol.ReadSolutionFromFile(txtInputFileName.Text);
            WriteLectDatabase();
            DataSet ds= dataConnection.OpenDataSet("SELECT * FROM GIANGVIEN");
            dataGridView1.DataSource = ds.Tables[0];
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
                strSQL+="',TGTG='";
                for (int j = 0; j < mySol.arrLecturer[i].arrAvailTime.Count; j++)                
                    strSQL += mySol.arrLecturer[i].arrAvailTime[j] + ",";
                strSQL += "',TongTGTG=";
                strSQL += mySol.arrLecturer[i].numAvailTime + ",BTG=";
                strSQL += mySol.arrLecturer[i].iSessionJoined + "";
                strSQL += " WHERE MaGV='" + (mySol.arrLecturer[i].IdLect + 1) + "'";
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
                strSQL += "')";
                dataConnection.ExecuteUpdateSQL(strSQL);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
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
        private void button1_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Cells[1, 1] = "tiếng việt";
            xlWorkBook.SaveAs("D:\\CHAU\\myexcel.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
            //PrintInvitation();
        }
        private void PrintInvitation()
        {
            FileStream fs = new FileStream("D:\\Chapter1_Example1.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();
            doc.Add(new Paragraph("DANH SÁCH HỘI ĐỒNG THAM DỰ"));
            PdfPTable table = new PdfPTable(8);
            table.AddCell("STT");
            table.AddCell("Họ tên sinh viên");
            table.AddCell("Tên đề tài");
            table.AddCell("Cán bộ hướng dẫn chính");
            table.AddCell("Giờ bắt đầu");
            table.AddCell("Ngày hội đồng");
            table.AddCell("Phòng");
            table.AddCell("Tư cách");
            doc.Add(table);
            doc.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strInput = "D:\\DanhsachCBCNV(13-14)_new.xls";
            string strInput2 = "D:\\DS_HV_nop_decuong_dot2_2014_final.xls";
            string strOutput = "D:\\Chau_MCB.xls";
            DataTable input = new DataTable();
            DataTable input2 = new DataTable();
            List<string> maCB = new List<string>();
            ReadInput1(strInput, input);
            ReadInput2(strInput2, input, input2,maCB);
            Print(strOutput, maCB, input, input2);
          
        }
        private void ReadInput1(string strInput, DataTable input)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            Excel.Range range;
            string str;
            int rCnt = 0;//for row
            int cCnt = 0;//for column

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(strInput, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;//read all sheet
            
            input.Columns.Add("STT");
            input.Columns.Add("MaCB");
            input.Columns.Add("Họ");
            input.Columns.Add("Tên");
            for (rCnt = 6; rCnt <= range.Rows.Count; rCnt++)//begin at row 6
            {
                List<string> information = new List<string>();

                for (cCnt = 1; cCnt <= 4; cCnt++)//read 7 columns
                {

                    if ((range.Cells[rCnt, cCnt] as Excel.Range).Value2 != null)
                        str = (range.Cells[rCnt, cCnt] as Excel.Range).Value2.ToString();
                    else
                        str = "";
                    information.Add(str);

                }
                //Add a lecturer into dtLect
                input.Rows.Add(information[1], information[2] + " " + information[3]);
            }


            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
            
        }
        private void ReadInput2(string strInput2, DataTable input, DataTable input2, List<string> maCB)
        {
            //read input 2
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            Excel.Range range;
            string str;
            int rCnt = 0;//for row
            int cCnt = 0;//for column


            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(strInput2, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;//read all sheet
            
            input2.Columns.Add("họ và tên");

            for (rCnt = 7; rCnt <= range.Rows.Count; rCnt++)//begin at row 6
            {
                List<string> information = new List<string>();

                for (cCnt = 1; cCnt <= 12; cCnt++)//read 7 columns
                {

                    if ((range.Cells[rCnt, cCnt] as Excel.Range).Value2 != null)
                        str = (range.Cells[rCnt, cCnt] as Excel.Range).Value2.ToString();
                    else
                        str = "";
                    information.Add(str);

                }
                //Add a lecturer into dtLect
                input2.Rows.Add(information[11].Substring(information[11].IndexOf(' ') + 1));
            }


            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
            
            foreach (DataRow row2 in input2.Rows)//tên cán bộ trong file mẫu
            {
                int check = -1;
                foreach (DataRow row1 in input.Rows)//tên cán bộ cần điền
                {
                    if (string.Compare(row2[0].ToString(), row1[1].ToString()) == 0)
                    {
                        maCB.Add(row1[0].ToString().Substring(row1[0].ToString().Length - 4) + "00");
                        check = 1;
                        break;
                    }

                }
                if (check == -1) maCB.Add("");
            }

        }
        private void Print(string strOutput, List<string> maCB, DataTable input, DataTable input2)
        {
            //print output
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            
            xlWorkSheet.Cells[1, 1] = "macb";
            xlWorkSheet.Cells[1, 2] = "Tên cán bộ";

            for (int i = 0; i < input2.Rows.Count; i++)
            {

                xlWorkSheet.Cells[i + 2, 0] = maCB[i].ToString();
                xlWorkSheet.Cells[i + 2, 1] = input.Rows[i][1].ToString();

            
            }
            xlWorkBook.SaveAs(strOutput, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

        }
    }
}
