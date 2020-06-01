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


namespace JobTracker.Login
{
    public partial class FrmJTLogin : Form
    {
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
                }
            }
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
