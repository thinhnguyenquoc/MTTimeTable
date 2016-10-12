namespace TimeTablingApplication
{
    partial class frmSolutionResult
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
            this.dgvLecturer = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvRoom = new System.Windows.Forms.DataGridView();
            this.dgvJury = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLecturer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJury)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvLecturer
            // 
            this.dgvLecturer.AllowUserToAddRows = false;
            this.dgvLecturer.AllowUserToDeleteRows = false;
            this.dgvLecturer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLecturer.Location = new System.Drawing.Point(0, 29);
            this.dgvLecturer.MultiSelect = false;
            this.dgvLecturer.Name = "dgvLecturer";
            this.dgvLecturer.ReadOnly = true;
            this.dgvLecturer.Size = new System.Drawing.Size(294, 229);
            this.dgvLecturer.TabIndex = 0;
            this.dgvLecturer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLecturer_CellContentClick);
            this.dgvLecturer.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLecturer_CellContentDoubleClick);
            this.dgvLecturer.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLecturer_CellDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Danh sách giảng viên tham gia";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 271);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Danh sách phòng";
            // 
            // dgvRoom
            // 
            this.dgvRoom.AllowUserToAddRows = false;
            this.dgvRoom.AllowUserToDeleteRows = false;
            this.dgvRoom.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoom.Location = new System.Drawing.Point(0, 291);
            this.dgvRoom.MultiSelect = false;
            this.dgvRoom.Name = "dgvRoom";
            this.dgvRoom.ReadOnly = true;
            this.dgvRoom.Size = new System.Drawing.Size(294, 161);
            this.dgvRoom.TabIndex = 3;
            this.dgvRoom.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRoom_CellDoubleClick);
            // 
            // dgvJury
            // 
            this.dgvJury.AllowUserToAddRows = false;
            this.dgvJury.AllowUserToDeleteRows = false;
            this.dgvJury.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvJury.Location = new System.Drawing.Point(313, 29);
            this.dgvJury.MultiSelect = false;
            this.dgvJury.Name = "dgvJury";
            this.dgvJury.ReadOnly = true;
            this.dgvJury.Size = new System.Drawing.Size(898, 367);
            this.dgvJury.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(320, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Danh sách hội đồng";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(464, 402);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Thay đổi thời gian";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(637, 402);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(134, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Thay đổi giảng viên";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(464, 431);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(134, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "In thư mời";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(637, 431);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(134, 23);
            this.button4.TabIndex = 8;
            this.button4.Text = "Lập lịch từ đầu";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(313, 402);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(120, 23);
            this.button5.TabIndex = 9;
            this.button5.Text = "In lịch bảo vệ";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // frmSolutionResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 499);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dgvJury);
            this.Controls.Add(this.dgvRoom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvLecturer);
            this.Name = "frmSolutionResult";
            this.Text = "frmSolutionResult";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSolutionResult_FormClosing);
            this.Load += new System.EventHandler(this.frmSolutionResult_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLecturer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJury)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLecturer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvRoom;
        private System.Windows.Forms.DataGridView dgvJury;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}