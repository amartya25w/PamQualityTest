namespace ArconConnector.Forms
{
    partial class FrmServiceConnRetry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmServiceConnRetry));
            this.btnRetry = new System.Windows.Forms.Button();
            this.txtErrorInfo = new System.Windows.Forms.TextBox();
            this.btnTerminateApplication = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRetry
            // 
            this.btnRetry.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnRetry.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRetry.Location = new System.Drawing.Point(476, 143);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(159, 24);
            this.btnRetry.TabIndex = 64;
            this.btnRetry.Text = "Retry Connection";
            this.btnRetry.UseVisualStyleBackColor = true;
            this.btnRetry.Click += new System.EventHandler(this.BtnRetry_Click);
            // 
            // txtDBErrorInfo
            // 
            this.txtErrorInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtErrorInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtErrorInfo.ForeColor = System.Drawing.Color.Black;
            this.txtErrorInfo.Location = new System.Drawing.Point(90, 7);
            this.txtErrorInfo.MaxLength = 20;
            this.txtErrorInfo.Multiline = true;
            this.txtErrorInfo.Name = "txtDBErrorInfo";
            this.txtErrorInfo.ReadOnly = true;
            this.txtErrorInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtErrorInfo.Size = new System.Drawing.Size(545, 115);
            this.txtErrorInfo.TabIndex = 70;
            this.txtErrorInfo.UseSystemPasswordChar = true;
            // 
            // btnTerminateApplication
            // 
            this.btnTerminateApplication.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnTerminateApplication.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTerminateApplication.Location = new System.Drawing.Point(311, 143);
            this.btnTerminateApplication.Name = "btnTerminateApplication";
            this.btnTerminateApplication.Size = new System.Drawing.Size(159, 24);
            this.btnTerminateApplication.TabIndex = 71;
            this.btnTerminateApplication.Text = "Terminate Application";
            this.btnTerminateApplication.UseVisualStyleBackColor = true;
            this.btnTerminateApplication.Click += new System.EventHandler(this.BtnTerminateApplication_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = Resources.Resource.Connection_Error;
            this.pictureBox1.Location = new System.Drawing.Point(6, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(76, 67);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 65;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(643, 134);
            this.pictureBox2.TabIndex = 72;
            this.pictureBox2.TabStop = false;
            // 
            // frmMSSQLConnectionRetry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(643, 176);
            this.ControlBox = false;
            this.Controls.Add(this.btnTerminateApplication);
            this.Controls.Add(this.txtErrorInfo);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnRetry);
            this.Controls.Add(this.pictureBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmServiceConnRetry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ARCON PAM Connection Error";
            this.Load += new System.EventHandler(this.FrmServiceConnRetry_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmServiceConnRetry_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRetry;
        private System.Windows.Forms.TextBox txtErrorInfo;
        private System.Windows.Forms.Button btnTerminateApplication;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        
    }
}