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
    public partial class frmTime : Form
    {
        int JuryID;
        clsSolution mySol;
        DataTable dtTime;
        public frmTime(string JuryID,clsSolution mySol, DataTable dtTime)
        {
            InitializeComponent();
            this.JuryID = int.Parse(JuryID)-1;
            this.mySol = mySol;
            this.dtTime = dtTime;
        }

        private void frmTime_Load(object sender, EventArgs e)
        {
            //find a list of available period to move to
            
            List<int> listPeriodCanInsert = new List<int>();
            for (int i = 0; i < mySol.Y; i++)
            {
                if (mySol.arrJury[JuryID].arrAvailPeriod[i]==true)
                if (mySol.CheckIfPeriodIsOK(JuryID, i) == true)
                    listPeriodCanInsert.Add(i);
            }
            DataTable dtTime_Copy = dtTime.Clone();
            foreach (int i in listPeriodCanInsert)
            {
                DataRow row = dtTime_Copy.NewRow();
                for (int j = 0; j < dtTime_Copy.Columns.Count; j++)
                {
                    row[j] = dtTime.Rows[i][j].ToString();
                }
                dtTime_Copy.Rows.Add(row);
            }
            dataGridView1.DataSource = dtTime_Copy;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 1)
            { MessageBox.Show("Vui lòng chọn thông tin!"); return ; }
            int PeriodID =int.Parse(dataGridView1.SelectedCells[0].OwningRow.Cells["MaTG"].Value.ToString())-1;
            mySol.arrJury[JuryID].IdPeriod = PeriodID;
            this.Dispose();
        }

    }
}
