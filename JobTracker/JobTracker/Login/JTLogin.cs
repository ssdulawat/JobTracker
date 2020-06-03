using JobTracker.JobTrackingMDIForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccessLayer;


namespace JobTracker.Login
{
    public partial class FrmJTLogin : Form
    {
        UserLogin dAL = new UserLogin();
        public JobAndTrackingMDI MdiParentCall;
        public bool CallFromMdi;
        public FrmJTLogin()
        {
            InitializeComponent();
        }

        private void BtnLoginJT_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtJTUserName.Text) || string.IsNullOrEmpty(txtJTPassword.Text))
                {
                    MessageBox.Show("Invalid UserName Or Password!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                //if (!string.IsNullOrEmpty(txtJTUserName.Text) || !string.IsNullOrEmpty(txtJTPassword.Text))
                //{
                string UserName = txtJTUserName.Text.Trim();
                string Password = txtJTPassword.Text.Trim();
                var UserDetail = new List<DataAccessLayer.Model.LoginAuthentication>();
                UserDetail = dAL.GetUsers(UserName, Password);

                if (UserDetail.Count > 0)
                {
                    foreach (var item in UserDetail)
                    {
                        if (item.UserType == "A" && cbIsTestDb.Checked)
                            MessageBox.Show("Must have admin privileges!", "Message");
                        else
                        {
                            //My.Settings.timeSheetLoginName = item.UserName;
                            //My.Settings.timeSheetLoginUserID = item.UserType;
                            //My.Settings.timeSheetLoginUserType = "User";
                            //My.Settings.IsTestDatabase = cbIsTestDb.Checked;
                            //My.Settings.PretimeSheetLoginName = "Null";
                            //My.Settings.PretimeSheetLoginUserID = "Null";
                            //My.Settings.PretimeSheetLoginUserType = "Null";

                            this.ShowInTaskbar = false;
                            this.Hide();

                            txtJTPassword.Text = "";/* TODO ERROR: Skipped SkippedTokensTrivia */
                            txtJTUserName.Text = "";

                        }

                        JobAndTrackingMDI mdi = new JobAndTrackingMDI();
                        // 'Check if the login form open from mdi form
                        if (MdiParentCall != null)
                            mdi = MdiParentCall;
                        if (item.UserType == "A")
                        {
                            // My.Settings.timeSheetLoginUserID = dt.Rows(0)(1).ToString()
                            //My.Settings.timeSheetLoginUserType = "Admin";
                            this.ShowInTaskbar = false;
                            this.Hide();

                            mdi.LoginformObject = this;
                            mdi.lblLogin.Text = "Admin LogOut";
                            //mdi.InvoiceToolStripMenuItem.Enabled = true;
                            //mdi.AdminToolStripMenuItem.Enabled = true;
                            //mdi.BackUpDataabaseToolStripMenuItem.Enabled = true;
                            //mdi.PMInfoToolStripMenuItem.Enabled = true;
                            //mdi.PMTMListItemToolStripMenuItem.Enabled = true;
                            mdi.Show();
                            txtJTPassword.Text = "";
                            txtJTUserName.Text = "";
                        }
                        else
                        {
                            mdi.lblLogin.Text = "LogOut";
                            mdi.Show();
                        }
                        // If login form open from mdi 
                        if (CallFromMdi)
                            Close();

                    }

                }
                else
                    MessageBox.Show("Incorrect User name & Password", "Message");

            }

            //}

            catch (Exception)
            {

                throw;
            }

        }

        private void BtnLoginCancelJT_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void FrmJTLogin_Load(object sender, EventArgs e)
        {
            //string status = "Null";
            Process my_proc = Process.GetCurrentProcess();
            string my_name = my_proc.ProcessName;

            if ((Process.GetProcessesByName(my_name).Length > 1))
            {
                this.Hide();
                MessageBox.Show("Application is Already Running");
                Application.Exit();
            }
        }
    }
}
