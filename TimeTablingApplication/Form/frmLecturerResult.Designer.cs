namespace TimeTablingApplication
{
    partial class frmLecturerResult
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
            this.dgvLect = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLect)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvLect
            // 
            this.dgvLect.AllowUserToAddRows = false;
            this.dgvLect.AllowUserToDeleteRows = false;
            this.dgvLect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLect.Location = new System.Drawing.Point(0, 0);
            this.dgvLect.Name = "dgvLect";
            this.dgvLect.ReadOnly = true;
            this.dgvLect.Size = new System.Drawing.Size(889, 261);
            this.dgvLect.TabIndex = 0;
            // 
            // frmLecturerResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 262);
            this.Controls.Add(this.dgvLect);
            this.Name = "frmLecturerResult";
            this.Text = "frmLecturerResult";
            this.Load += new System.EventHandler(this.frmLecturerResult_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgvLect;
    }
}