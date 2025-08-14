namespace ArconConnector.Forms
{
    partial class FrmTunnel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTunnel));
            this.grpCreateprofile = new System.Windows.Forms.GroupBox();
            this.lblProfileName = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpCreateprofile.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpCreateprofile
            // 
            this.grpCreateprofile.Controls.Add(this.lblProfileName);
            this.grpCreateprofile.Controls.Add(this.btnClose);
            this.grpCreateprofile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpCreateprofile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCreateprofile.Location = new System.Drawing.Point(0, -4);
            this.grpCreateprofile.Name = "grpCreateprofile";
            this.grpCreateprofile.Size = new System.Drawing.Size(478, 51);
            this.grpCreateprofile.TabIndex = 6;
            this.grpCreateprofile.TabStop = false;
            // 
            // lblProfileName
            // 
            this.lblProfileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfileName.ForeColor = System.Drawing.Color.Navy;
            this.lblProfileName.Location = new System.Drawing.Point(12, 16);
            this.lblProfileName.Name = "lblProfileName";
            this.lblProfileName.Size = new System.Drawing.Size(378, 25);
            this.lblProfileName.TabIndex = 3;
            this.lblProfileName.Text = "Please Wait, Connecting To The Remote Server";
            this.lblProfileName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(396, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 27);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FrmTunnel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 47);
            this.Controls.Add(this.grpCreateprofile);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTunnel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ARCOS VPN Client";
            this.grpCreateprofile.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox grpCreateprofile;
        private System.Windows.Forms.Label lblProfileName;
        private System.Windows.Forms.Button btnClose;

    }
}

