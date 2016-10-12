namespace TimeTablingApplication
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtInputFileName = new System.Windows.Forms.TextBox();
            this.btInputFileName = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btRun = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.btChoose = new System.Windows.Forms.Button();
            this.btMove = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.btDetail = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // txtInputFileName
            // 
            this.txtInputFileName.Location = new System.Drawing.Point(12, 29);
            this.txtInputFileName.Name = "txtInputFileName";
            this.txtInputFileName.Size = new System.Drawing.Size(100, 20);
            this.txtInputFileName.TabIndex = 0;
            this.txtInputFileName.TextChanged += new System.EventHandler(this.txtInputFileName_TextChanged);
            // 
            // btInputFileName
            // 
            this.btInputFileName.Location = new System.Drawing.Point(128, 29);
            this.btInputFileName.Name = "btInputFileName";
            this.btInputFileName.Size = new System.Drawing.Size(75, 23);
            this.btInputFileName.TabIndex = 1;
            this.btInputFileName.Text = "Browse";
            this.btInputFileName.UseVisualStyleBackColor = true;
            this.btInputFileName.Click += new System.EventHandler(this.btInputFileName_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Input File";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btRun
            // 
            this.btRun.Location = new System.Drawing.Point(128, 58);
            this.btRun.Name = "btRun";
            this.btRun.Size = new System.Drawing.Size(75, 23);
            this.btRun.TabIndex = 3;
            this.btRun.Text = "Run";
            this.btRun.UseVisualStyleBackColor = true;
            this.btRun.Click += new System.EventHandler(this.btRun_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(223, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(566, 159);
            this.dataGridView1.TabIndex = 4;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(223, 196);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(566, 225);
            this.dataGridView2.TabIndex = 5;
            this.dataGridView2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellClick);
            // 
            // btChoose
            // 
            this.btChoose.Location = new System.Drawing.Point(128, 96);
            this.btChoose.Name = "btChoose";
            this.btChoose.Size = new System.Drawing.Size(75, 23);
            this.btChoose.TabIndex = 6;
            this.btChoose.Text = "Choose";
            this.btChoose.UseVisualStyleBackColor = true;
            this.btChoose.Click += new System.EventHandler(this.btChoose_Click);
            // 
            // btMove
            // 
            this.btMove.Location = new System.Drawing.Point(128, 142);
            this.btMove.Name = "btMove";
            this.btMove.Size = new System.Drawing.Size(75, 23);
            this.btMove.TabIndex = 7;
            this.btMove.Text = "Move";
            this.btMove.UseVisualStyleBackColor = true;
            this.btMove.Visible = false;
            this.btMove.Click += new System.EventHandler(this.btMove_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(128, 178);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 8;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Visible = false;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(128, 216);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 9;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btDetail
            // 
            this.btDetail.Location = new System.Drawing.Point(12, 58);
            this.btDetail.Name = "btDetail";
            this.btDetail.Size = new System.Drawing.Size(75, 23);
            this.btDetail.TabIndex = 10;
            this.btDetail.Text = "Details";
            this.btDetail.UseVisualStyleBackColor = true;
            this.btDetail.Click += new System.EventHandler(this.btDetail_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 433);
            this.Controls.Add(this.btDetail);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btMove);
            this.Controls.Add(this.btChoose);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btRun);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btInputFileName);
            this.Controls.Add(this.txtInputFileName);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInputFileName;
        private System.Windows.Forms.Button btInputFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btRun;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button btChoose;
        private System.Windows.Forms.Button btMove;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btDetail;
    }
}

