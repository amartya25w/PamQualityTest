using System;

namespace ArconConnector.ExeType
{
    partial class FrmExeConnector
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
            this.SuspendLayout();
            // 
            // FrmExeConnector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 120);
            this.Name = "FrmExeConnector";
            this.Text = "ARCOS PAM Connector";
            this.Load += new System.EventHandler(this.FrmExeConnector_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmExeConnector_FormClosing);
            this.ResumeLayout(false);

            this._ApplicationIdle = new Winforms.Components.ApplicationIdle();
            this._ApplicationIdle.Idle += new System.EventHandler(this.ApplicationIdle_Idle);
            this._ApplicationIdle.Tick += new System.EventHandler<Winforms.Components.ApplicationIdleData.TickEventArgs>(this.ApplicationIdle_Tick);
        }

        
        #endregion
    }
}

