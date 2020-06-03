﻿using JobTracker.JobTrackingForm;
using JobTracker.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobTracker.JobTrackingMDIForm
{
    public partial class JobAndTrackingMDI : Form
    {
        #region "Global variable"
        public string EmailTable;
        public string TableUpdateRecord = "0";
        public string TableInsertRecord = "0";
        public string TableDeleteRecord = "0";
        public string SenderEmailAddress;
        public string SenderEmailPassword;
        public bool SendEmailSuccessful;
        public Form frm = new Form();
        public int ScreenWidth;
        public int ScreenHeight;
        public Int64 JobID;
        public bool admintools;
        public char ReportStatus;
        public Int16 ColorId;
        public bool CalEmailRem;
        public string InvoiceEmailAddress;
        public double TotalVECostAmount;
        public event LoginChangeEventHandler LoginChange;

        public delegate void LoginChangeEventHandler(object sender, EventArgs e);
        #endregion
        public JobAndTrackingMDI()
        {
            InitializeComponent();
        }
        private void Manager_Click(System.Object sender, System.EventArgs e)
        {
            // CreateFromandtab(JobStatus.Instance);
        }

        private void JobAndTrackingMDI_Load(object sender, EventArgs e)
        {
            ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            ScreenHeight = Screen.PrimaryScreen.Bounds.Height;

            //GetSenderEmailaddress();
            //if (DataVarifReminderShedule() == "H")
            //    //TimerGet.Interval = 3600000;
            //    lblVersion.Text = "Version:-" + My.Application.Info.Version.ToString;
            //this.Text = "Job Tracker (" + lblVersion.Text + ")";
            //NtyicnJT.Text = "Job Traking (JT " + lblVersion.Text.Trim + ")";
            //NtyicnJT.BalloonTipText = "JT Activated";
            //NtyicnJT.ShowBalloonTip(3000);
            //EnableTimer();
            //Manager_Click(sender, e);

            //UpdateCheckNewVersion();
            //if (ConnectionStringSetting.IsLocalDatabase == true)
            //    MessageBox.Show("This Time you connected Local data base", "DataBase Connection ");
        }
        public object LoginformObject { get; set; }
        private void JobAndTrackingMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            // CType(LoginformObject, frmJTLogin).Show()
            if (LoginformObject != null)
            {
                //(FrmJTLogin)LoginformObject.ShowInTaskbar = true;
                //(FrmJTLogin)LoginformObject.Close();
            }
            e.Cancel = false;
            //NtyicnJT.Visible = false;
        }

        private void NtyicnJT_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == System.Windows.Forms.MouseButtons.Right)
            //{
            //    Drawing.Point point = new Drawing.Point(Control.MousePosition);
            //    CMSNotify.Show(point);
            //}
            //else if (e.Button == MouseButtons.Left)
            //{
            //    this.ShowInTaskbar = true;
            //    this.WindowState = FormWindowState.Maximized;


            //    if (!ActiveMdiChild == null)
            //        this.ActiveMdiChild.WindowState = FormWindowState.Maximized;
            //}
        }

        private void TimerDateTime_Tick(object sender, EventArgs e)
        {

        }

        private void TimerGet_Tick(object sender, EventArgs e)
        {

        }

        private void BackWorkerEmail_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
