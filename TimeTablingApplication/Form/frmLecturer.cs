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
    public partial class frmLecturer : Form
    {
        clsSolution mySol;
        DataConnection dataConnection;
        DataTable dt;
        public frmLecturer(clsSolution mySol, DataConnection dataConnection, DataTable dt)
        {
            InitializeComponent();
            this.mySol = mySol;
            this.dataConnection = dataConnection;
            this.dt = dt;
        }

        private void frmLecturer_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

       
    }
}
