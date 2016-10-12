using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace TimeTablingApplication
{
    public partial class Form1 : Form
    {
      //  [DllImport("D://MasterCourse//luan van//dang lam//Debug//MyDLL.dll")]
        //public static extern bool CheckIfPeriodIsOK(clsSolution solution, int juryID, int Period);
        clsSolution mySol;
        int juryNeedToMove;
        DataTable tblJury;
        DataTable tblTime;
        public Form1()
        {
            InitializeComponent();
            //clsSolution finalSolution = getSolution();
            mySol = new clsSolution();
          //  ReadListLecturer("D://danhsachdetai.txt");
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
            tblJury = new DataTable();
            tblJury = mySol.GetDataTableForJury(tblJury);
            dataGridView1.DataSource = tblJury;

            tblTime = new DataTable();
            tblTime = mySol.GetDataTableForTime(tblTime);
            dataGridView2.DataSource = tblTime;
            
        }

        private void btChoose_Click(object sender, EventArgs e)
        {
            //cannot handle 2 juries at the same time
            if (dataGridView2.SelectedCells.Count != 1)
            { MessageBox.Show("Choose 1 cell"); return; }
            //the first column is not about juries
            //empty cell is not acceptable
            if (dataGridView2.SelectedCells[0].ColumnIndex == 0
                ||dataGridView2.SelectedCells[0].Value==null
                ||dataGridView2.SelectedCells[0].Value.ToString()==String.Empty)
            { MessageBox.Show("Cannot choose this cell"); return; }
            //find a list of available period to move to
            int juryID = (int)dataGridView2.SelectedCells[0].Value;
            List<int> listPeriodCanInsert = new List<int>();
            for (int i = 0; i < mySol.Y; i++)
            {
                if (mySol.CheckIfPeriodIsOK(juryID,i) == true)
                    listPeriodCanInsert.Add(i);
            }
            //color available period to move
            foreach (int k in listPeriodCanInsert)
            {
                for (int i = 0; i < dataGridView2.RowCount; i++)
                {
                    if (i == k)//time match
                    {
                        for (int j = 1; j < dataGridView2.ColumnCount; j++)//consider all rooms availble at that time
                        {
                            int check = -1;
                            if (dataGridView2.Rows[i].Cells[j].Value == null || dataGridView2.Rows[i].Cells[j].Value.ToString() == String.Empty)
                            {
                                dataGridView2.Rows[i].Cells[j].Style.BackColor = Color.Yellow;
                                check = 0;
                            }
                            if (check == -1)
                            {
                                dataGridView2.Columns.Add("room extra", "Room " + (mySol.numRoom + 1).ToString());
                                dataGridView2.Rows[i].Cells[j+1].Style.BackColor = Color.Yellow;
                            }
                        }
                    }
                    
                }
                //free cell contains jury ID
                dataGridView2.ClearSelection();
                juryNeedToMove = juryID;
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.SelectedCells.Count == 1
                && dataGridView2.SelectedCells[0].Style.BackColor == Color.Yellow)
            {
                if (btMove.Enabled == false)
                    btMove.Enabled = true;
                if (btMove.Visible == false)
                    btMove.Visible = true;
                if (btCancel.Enabled == false)
                    btCancel.Enabled = true;
                if (btCancel.Visible == false)
                    btCancel.Visible = true;
            }
            else
            {
                btMove.Visible = false;
                btCancel.Visible = false;
            }
        }

        private void btMove_Click(object sender, EventArgs e)
        {
            mySol.arrJury[juryNeedToMove].IdPeriod = dataGridView2.SelectedCells[0].RowIndex;
            mySol.arrJury[juryNeedToMove].IdRoom = dataGridView2.SelectedCells[0].ColumnIndex-1;
            foreach (DataGridViewRow row in dataGridView2.Rows)
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Style.BackColor = Color.White;
            //move cell
            tblTime = new DataTable();
            tblTime = mySol.GetDataTableForTime(tblTime);
            dataGridView2.DataSource = tblTime;
            
            dataGridView2.ClearSelection();
            btMove.Visible = false;
            btCancel.Visible = false;
            
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.Rows)
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Style.BackColor = Color.White;
            dataGridView2.ClearSelection();
            btCancel.Enabled = false;
            btMove.Enabled = false;
        }
        private void WriteSolution()
        {

        }
        private void btSave_Click(object sender, EventArgs e)
        {
            string outputFileName = txtInputFileName.Text + ".sol";
            mySol.WriteSolution(outputFileName);
        }

        public void ReadListLecturer(string path)
        {
            StreamReader myReader = new StreamReader(path,Encoding.Unicode);
            String myLine = myReader.ReadToEnd();
            
            myReader.Close();
            MessageBox.Show(myLine);
            StreamWriter w = new StreamWriter(path + ".my");
            w.WriteLine(myLine);
            w.Close();
        }

        private void btDetail_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 1)
            { MessageBox.Show("Choose 1 cell!"); return; }
            if (dataGridView1.SelectedCells[0].ColumnIndex >5)
            { MessageBox.Show("Cannot view details of this"); return; }
            if (dataGridView1.SelectedCells[0].ColumnIndex == 0)
            {
                int ID = (int)dataGridView1.SelectedCells[0].Value;
                string show = "Đề tài " + ID.ToString() + "\n Tên đề tài: ";
                show += mySol.arrJury[ID-1].details.Name + "\n SVTH: " + mySol.arrJury[ID-1].details.StudentName;
                MessageBox.Show(show);
            }
            else
            {
                int ID = (int)dataGridView1.SelectedCells[0].Value;
                string show = "Giảng viên " + ID.ToString() + "\n Tên giảng viên: ";
                show += mySol.arrLecturer[ID-1].details.Name;
                MessageBox.Show(show);
            }
        }

        private void txtInputFileName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
