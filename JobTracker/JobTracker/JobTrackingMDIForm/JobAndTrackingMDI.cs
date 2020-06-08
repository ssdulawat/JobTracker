using DevComponents.DotNetBar;
using JobTracker.JobTrackingForm;
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
            CreateFromandtab(JobStatus.Instance);
        }


        private void JobAndTrackingMDI_Load(object sender, EventArgs e)
        {
            ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            ScreenHeight = Screen.PrimaryScreen.Bounds.Height;

            //GetSenderEmailaddress();
            //if (DataVarifReminderShedule() == "H")               
            //    lblVersion.Text = "Version:-" + My.Application.Info.Version.ToString;
            //this.Text = "Job Tracker (" + lblVersion.Text + ")";
            //NtyicnJT.Text = "Job Traking (JT " + lblVersion.Text.Trim + ")";
            //NtyicnJT.BalloonTipText = "JT Activated";
            //NtyicnJT.ShowBalloonTip(3000);
            //EnableTimer();
            Manager_Click(sender, e);

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
            try
            {
                lblDate.Text = "DATE:-" + string.Format(DateTime.Now.ToString("MM-dd-yyyy"));
                lblTime.Text = "TIME:-" + string.Format(DateTime.Now.ToString("hh:mm:ss tt"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //MsgBox(ex.Message);
            }
        }

        private void TimerGet_Tick(object sender, EventArgs e)
        {

        }

        private void BackWorkerEmail_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void TabctrlFrm_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            try
            {
                foreach (Form frm in this.MdiChildren)
                {
                    if (frm.IsMdiContainer != true)
                    {
                        if (tabctrlFrm.SelectedTab.Text == frm.Text)
                        {
                            frm.BringToFront();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }

        private void TabctrlFrm_TabItemClose(object sender, DevComponents.DotNetBar.TabStripActionEventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.IsMdiContainer != true)
                {
                    if (tabctrlFrm.SelectedTab.Text == frm.Text)
                    {
                        //tabctrlFrm.Tabs.RemoveAt(tabctrlFrm.Tabs.IndexOf(tabctrlFrm.Tabs.Item(frm.Text)));
                        frm.Close();
                        break;
                    }
                }
            }

        }

        public void CreateFromandtab(Form Newfrm)
        {
            TabItem newtab = new TabItem();
            newtab.Name = Newfrm.Text;
            newtab.Text = Newfrm.Text;
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.IsMdiContainer != true)
                {
                    if (frm.Text == Newfrm.Text)
                    {
                        Newfrm.BringToFront();
                        // tabctrlFrm.SelectedTab = tabctrlFrm.Tabs.Item(Newfrm.Text);
                        return;
                    }
                }
            }
            tabctrlFrm.Tabs.Add(newtab);
            tabctrlFrm.SelectedTab = newtab;
            tabctrlFrm.Visible = true;
            Newfrm.MdiParent = this;
            Newfrm.WindowState = FormWindowState.Maximized;
            Newfrm.Show();
        }

    }
}
