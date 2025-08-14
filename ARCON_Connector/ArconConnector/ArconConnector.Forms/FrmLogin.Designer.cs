using ArconConnector.Forms.Resources;

namespace ArconConnector.Forms
{
    partial class FrmLogin
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
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.errLogin = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlDomainNames = new System.Windows.Forms.Panel();
            this.cboDomainName = new System.Windows.Forms.ComboBox();
            this.btnUnlockSession = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTrademark = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblFormHeading = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRefreshDomains = new System.Windows.Forms.Button();
            this.btnTerminateSession = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.errLogin)).BeginInit();
            this.pnlDomainNames.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Navy;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(212, 248);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.Black;
            this.txtPassword.Location = new System.Drawing.Point(128, 173);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(331, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUserName
            // 
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.ForeColor = System.Drawing.Color.Black;
            this.txtUserName.Location = new System.Drawing.Point(128, 138);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(331, 20);
            this.txtUserName.TabIndex = 0;
            // 
            // errLogin
            // 
            this.errLogin.ContainerControl = this;
            // 
            // pnlDomainNames
            // 
            this.pnlDomainNames.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDomainNames.Controls.Add(this.cboDomainName);
            this.pnlDomainNames.Location = new System.Drawing.Point(128, 208);
            this.pnlDomainNames.Name = "pnlDomainNames";
            this.pnlDomainNames.Size = new System.Drawing.Size(261, 22);
            this.pnlDomainNames.TabIndex = 22;
            // 
            // cboDomainName
            // 
            this.cboDomainName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboDomainName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDomainName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboDomainName.FormattingEnabled = true;
            this.cboDomainName.Location = new System.Drawing.Point(0, 0);
            this.cboDomainName.Name = "cboDomainName";
            this.cboDomainName.Size = new System.Drawing.Size(259, 21);
            this.cboDomainName.TabIndex = 12;
            // 
            // btnUnlockSession
            // 
            this.btnUnlockSession.BackColor = System.Drawing.Color.Navy;
            this.btnUnlockSession.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUnlockSession.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUnlockSession.ForeColor = System.Drawing.Color.White;
            this.btnUnlockSession.Location = new System.Drawing.Point(128, 248);
            this.btnUnlockSession.Name = "btnUnlockSession";
            this.btnUnlockSession.Size = new System.Drawing.Size(75, 23);
            this.btnUnlockSession.TabIndex = 23;
            this.btnUnlockSession.Text = "Login";
            this.btnUnlockSession.UseVisualStyleBackColor = false;
            this.btnUnlockSession.Visible = false;
            this.btnUnlockSession.Click += new System.EventHandler(this.BtnUnlockSession_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::ArconConnector.Forms.Resources.Resource.Backcolor_Blue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblTrademark);
            this.panel1.Controls.Add(this.lblCopyright);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(506, 113);
            this.panel1.TabIndex = 29;
            // 
            // lblTrademark
            // 
            this.lblTrademark.BackColor = System.Drawing.Color.Transparent;
            this.lblTrademark.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrademark.ForeColor = System.Drawing.Color.White;
            this.lblTrademark.Location = new System.Drawing.Point(366, 94);
            this.lblTrademark.Name = "lblTrademark";
            this.lblTrademark.Size = new System.Drawing.Size(138, 16);
            this.lblTrademark.TabIndex = 37;
            this.lblTrademark.Text = "®";
            this.lblTrademark.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.BackColor = System.Drawing.Color.Transparent;
            this.lblCopyright.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyright.ForeColor = System.Drawing.Color.White;
            this.lblCopyright.Location = new System.Drawing.Point(3, 95);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(67, 13);
            this.lblCopyright.TabIndex = 36;
            this.lblCopyright.Text = "Copyright ©";
            this.lblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Navy;
            this.panel2.Controls.Add(this.lblFormHeading);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(504, 21);
            this.panel2.TabIndex = 33;
            // 
            // lblFormHeading
            // 
            this.lblFormHeading.AutoSize = true;
            this.lblFormHeading.BackColor = System.Drawing.Color.Transparent;
            this.lblFormHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormHeading.ForeColor = System.Drawing.Color.White;
            this.lblFormHeading.Location = new System.Drawing.Point(4, 3);
            this.lblFormHeading.Name = "lblFormHeading";
            this.lblFormHeading.Size = new System.Drawing.Size(140, 13);
            this.lblFormHeading.TabIndex = 30;
            this.lblFormHeading.Text = "Log On to ARCON PAM";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::ArconConnector.Forms.Resources.Resource.ARCONPAM;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(128, 37);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(213, 54);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(35, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "User ID :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(35, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Password :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(35, 212);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Domain Name :";
            // 
            // btnRefreshDomains
            // 
            this.btnRefreshDomains.BackColor = System.Drawing.Color.Navy;
            this.btnRefreshDomains.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRefreshDomains.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshDomains.ForeColor = System.Drawing.Color.White;
            this.btnRefreshDomains.Location = new System.Drawing.Point(395, 208);
            this.btnRefreshDomains.Name = "btnRefreshDomains";
            this.btnRefreshDomains.Size = new System.Drawing.Size(64, 23);
            this.btnRefreshDomains.TabIndex = 33;
            this.btnRefreshDomains.Text = "Refresh";
            this.btnRefreshDomains.UseVisualStyleBackColor = false;
            this.btnRefreshDomains.Click += new System.EventHandler(this.BtnRefreshDomains_Click);
            // 
            // btnTerminateSession
            // 
            this.btnTerminateSession.BackColor = System.Drawing.Color.Navy;
            this.btnTerminateSession.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnTerminateSession.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTerminateSession.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTerminateSession.ForeColor = System.Drawing.Color.White;
            this.btnTerminateSession.Location = new System.Drawing.Point(212, 248);
            this.btnTerminateSession.Name = "btnTerminateSession";
            this.btnTerminateSession.Size = new System.Drawing.Size(108, 23);
            this.btnTerminateSession.TabIndex = 34;
            this.btnTerminateSession.Text = "Terminate Session";
            this.btnTerminateSession.UseVisualStyleBackColor = false;
            this.btnTerminateSession.Visible = false;
            this.btnTerminateSession.Click += new System.EventHandler(this.BtnTerminateSession_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox2.Location = new System.Drawing.Point(0, 112);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(506, 180);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 24;
            this.pictureBox2.TabStop = false;
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(506, 292);
            this.Controls.Add(this.btnTerminateSession);
            this.Controls.Add(this.btnRefreshDomains);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnUnlockSession);
            this.Controls.Add(this.pnlDomainNames);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLogin";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log On to ARCON PAM";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmLogin_Closing);
            this.Load += new System.EventHandler(this.FrmLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errLogin)).EndInit();
            this.pnlDomainNames.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.ErrorProvider errLogin;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlDomainNames;
        private System.Windows.Forms.ComboBox cboDomainName;
        private System.Windows.Forms.Button btnUnlockSession;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblFormHeading;
        private System.Windows.Forms.Button btnRefreshDomains;
        private System.Windows.Forms.Button btnTerminateSession;
        private System.Windows.Forms.Label lblTrademark;
        private System.Windows.Forms.Label lblCopyright;
    }
}