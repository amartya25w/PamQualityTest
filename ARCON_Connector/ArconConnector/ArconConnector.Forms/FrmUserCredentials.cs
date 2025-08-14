using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArconConnector.Forms
{
    public partial class FrmUserCredentials : Form
    {
        private BusinessEntities.BaseConnector _BaseConnector;
        int int_ServiceID = 0;
        string str_Username = "";
        public FrmUserCredentials()
        {
            InitializeComponent();
        }

        public FrmUserCredentials(BusinessEntities.BaseConnector objBaseConnector)
        {
            InitializeComponent();
            _BaseConnector = objBaseConnector;
        }

        public FrmUserCredentials(String pDomainName, String pUserName)
        {
            InitializeComponent();
            txtDomain.Text = pDomainName;
            txtUserName.Text = pUserName;
        }

        //User for PAMIT-I5219 PROMPT_USER(E-Brain)
        public FrmUserCredentials(String pDomainName, String pUserName, Int32 pServiceID)
        {

            InitializeComponent();
            txtDomain.Text = pDomainName;
            txtUserName.Text = pUserName;
            int_ServiceID = pServiceID;
        }

        public FrmUserCredentials(String pDomainName, String pUserName, Boolean pIsHideOtherOptions)
        {
            InitializeComponent();
            txtDomain.Text = pDomainName;
            txtUserName.Text = pUserName;
            
            chkIsDoNotPerformSSO.Visible = !pIsHideOtherOptions;
            chkWindowsAuthentication.Visible = !pIsHideOtherOptions;
        }

        private void frmUserCredentials_Service_Load(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }

        public void PocessLogin()
        {   
                this.ShowDialog();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                str_Username = txtUserName.Text.Trim();
                if (txtPassword.Text.Length <= 0 && txtPassword.Enabled==true)
                {
                    MessageBox.Show("Password Can Not Be Left Blank.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (str_Username != null || str_Username != "")
                {

                    //// PAMIT-I5219 (E-BRAIN)
                    //SqlParameter[] sqlParam = new SqlParameter[2];
                    //sqlParam[0] = new SqlParameter("@ServiceId", SqlDbType.BigInt);
                    //sqlParam[0].Value = int_ServiceID;
                    //sqlParam[1] = new SqlParameter("@Username", SqlDbType.VarChar, 500);
                    //sqlParam[1].Value = str_Username;

                    //try
                    //{
                    //    DataTable lDatatable;
                    //    lDatatable = CommonFunctionsCM.ReturnDataTable(CommandType.StoredProcedure, "usp_ServiceUsernameUpdate", sqlParam);
                    //}
                    //catch(Exception ex) { }

                    _cancelCloseForm = false;
                    this.Close();

                }
                else
                {
                    MessageBox.Show("User Name Can Not Be Left Blank.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }

        private void chkWindowsAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtUserName.Enabled = !chkWindowsAuthentication.Checked;
                txtPassword.Enabled = !chkWindowsAuthentication.Checked;
                txtDomain.Enabled = !chkWindowsAuthentication.Checked;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }

        private Boolean _cancelCloseForm = true;
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are You Sure, Do Want To Terminate Application?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _cancelCloseForm = false;
                    this.Close();
                }                
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }

        private void frmUserCredentials_Service_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                e.Cancel = _cancelCloseForm;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkIsDoNotPerformSSO_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                chkWindowsAuthentication.Enabled = !chkIsDoNotPerformSSO.Checked;
                txtUserName.Enabled = !chkIsDoNotPerformSSO.Checked;
                txtPassword.Enabled = !chkIsDoNotPerformSSO.Checked;
                txtDomain.Enabled = !chkIsDoNotPerformSSO.Checked;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
