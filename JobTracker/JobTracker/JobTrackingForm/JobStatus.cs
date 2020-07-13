using Common;
using ComponentFactory.Krypton.Toolkit;
using DataAccessLayer;
using DataAccessLayer.Model;
using JobTracker.JobTrackingMDIForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace JobTracker.JobTrackingForm
{
    public partial class JobStatus : Form
    {
        ManagerRepository repo = new ManagerRepository();
        #region Declaration
        public DataTable dtJL = new DataTable();
        public DataTable dtPreReq = new DataTable();
        public string AutoJB;
        public DataTable dtPermit = new DataTable();
        public JobAndTrackingMDI mdio;
        public DataTable dtNotes = new DataTable();
        public int firstLoad = 0;
        public string cbxJobListDescriptionEvent;
        public string cbxSearchTrackCommentEvent;
        public DataGridViewComboBoxColumn cbxClient = new DataGridViewComboBoxColumn();
        public DataGridViewComboBoxColumn cmbInvoiceClient = new DataGridViewComboBoxColumn();
        public DataGridViewComboBoxColumn cbxContacts = new DataGridViewComboBoxColumn();
        public string ContactsName = string.Empty;
        public int ContactsRowIndex = -1;
        public int selectedJobListID;
        public string CheckString;
        public ComboBox cb = new ComboBox();
        public string GridSymbole;
        public DataGridViewComboBoxCell cmbTCTackName;
        public static JobStatus _Instance;
        public string CopyPath;
        public string FolderName;
        public bool isDisable;
        public bool ManagerLoad = true;
        public Int64 JobID;
        public bool selectRecord_Joblist = false;
        public Int32 Colorid;
        public string UserName;
        public string UserType;
        public bool CheckUser;
        public string ColorColumn;
        private const string TIMESERVICEFEE = "TimeServiceFee;";
        #endregion

        #region Properties
        public static JobStatus Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new JobStatus();
                }
                return _Instance;
            }

        }

        public string GetCopyFolderName
        {
            get { return CopyPath; }
            set { CopyPath = value; }
        }
        public string GetFolderName
        {
            get
            {
                return FolderName;
            }
            set
            {
                FolderName = value;
            }
        }
        private Color mBodyColor;
        public Color BodyColor
        {
            get
            {
                // Do some work
                return mBodyColor;
            }
            set
            {
                mBodyColor = value;
            }
        }

        public bool isDisabled
        {
            get
            {
                return isDisable;
            }
            set
            {
                isDisable = value;
            }
        }

        private int processcount;

        #endregion

        #region Events
        public JobStatus()
        {
            InitializeComponent();
        }

        private void JobStatus_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                ProgressBar1.Visible = false;
                label11.Visible = false;
                mdio.MdiParent = MdiParent;
                ManagerLoad = true;
                btnImportTimeSheetData.Visible = false;
                BtnHistoryClick.Visible = true;

                //DefaultValueSetup();
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "JobStatus_Load", ex.Message);
            }
        }

        private void TimerLoad_Tick(object sender, EventArgs e)
        {
            try
            {
                if (ManagerLoad)
                {
                    try
                    {
                        selectRecord_Joblist = true;
                        chkPreRequirment.Checked = false;

                        SetColumns();
                        fillGridJobList();
                        //DriveListBox1.DataSource = "D:";
                        //var MainGrid = new List<ManagerData>();
                        //MainGrid = new GetManagerData();

                        //grvJobList.DataSource = MainGrid;
                        selectRecord_Joblist = false;
                    }
                    catch (Exception ex)
                    {
                        cErrorLog.WriteLog("JobStatus", "TimerLoad_Tick", ex.Message);
                    }
                }
                if (selectRecord_Joblist == false)
                {
                    if (processcount == 1)
                    {
                        SetColumnPreRequirment();
                        FillGridPreRequirment();

                        //var PreRequirement = new List<PreRequirement>();
                        //PreRequirement = new GetPreRequirement();
                        //grvPreRequirments.DataSource = PreRequirement;

                    }
                    if (processcount == 2)
                    {
                        SetColumnPermit();
                        FillGridPermitRequiredInspection();
                        //var PermitRequiredInspection = new List<PermitsRequirement>();
                        //PermitRequiredInspection = new GetPermitsRequirement();
                        //grvPreRequirments.DataSource = PermitRequiredInspection;
                    }
                    if (processcount == 3)
                    {
                        SetColumnNotes();
                        FillGridNotesCommunication();
                        //var NotesCommunication = new List<NotesComunication>();
                        //NotesCommunication = new GetNotesComunication();
                        //grvPreRequirments.DataSource = NotesCommunication;
                    }
                    if (processcount == 4)
                        //   SetBadClient();
                        if (processcount == 5)
                        {
                            // Fillcombo();
                            // ApplyPageLoadSetting();
                            if ((grvJobList.Rows.Count != 0))
                            {
                                //  ChangeDirJobNumber(grvJobList.Rows.Count - 1);
                                //   ChangeTraficLight(grvJobList.Rows.Count - 1);
                            }
                            timerLoad.Stop();
                            timerLoad.Enabled = false;
                        }
                    processcount = processcount + 1;
                }

                ManagerLoad = false;
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "TimerLoad_Tick", ex.Message);
            }
        }

        private void grvPermitsRequiredInspection_CellClick(System.Object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (grvPermitsRequiredInspection.Columns[e.ColumnIndex].Name == "cmbBillState")
                {

                    if (mdio.lblLogin.Text == "Admin Login")
                    {
                        grvPermitsRequiredInspection.Columns[e.ColumnIndex].ReadOnly = true;
                    }
                    else
                    {
                        grvPermitsRequiredInspection.Columns[e.ColumnIndex].ReadOnly = false;
                    }
                }
            }
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (grvPermitsRequiredInspection.Columns[e.ColumnIndex].Name == "GrdbtnPrequisition")
                {
                    try
                    {
                        //Attempt to update the datasource.
                        int cnt = e.RowIndex;
                        if (Convert.ToInt32(grvPermitsRequiredInspection.Rows[cnt].Cells["JobTrackingID"].Value.ToString()) == 0)
                        {
                            InsertPermits();
                            return;
                        }
                        btnInsertPermit.Text = "Insert";
                        btnDeletePermit.Enabled = true;
                        if (string.IsNullOrEmpty(grvPermitsRequiredInspection.Rows[grvPermitsRequiredInspection.CurrentRow.Index].Cells["Track"].Value.ToString()))
                        {
                            KryptonMessageBox.Show("Track field are compulsory", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (string.IsNullOrEmpty(grvPermitsRequiredInspection.Rows[grvPermitsRequiredInspection.CurrentRow.Index].Cells["TrackSub"].Value.ToString()))
                        {
                            KryptonMessageBox.Show("TrackSub field are compulsory", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        try
                        {
                            SqlCommand cmd = new SqlCommand("update  Jobtracking set JobListID= @JobListID,TaskHandler=@TaskHandler,Track=@Track,Status= @Status,Submitted=@Submitted, Obtained=@Obtained,Expires=@Expires, FinalAction =@FinalAction, BillState=@BillState , AddDate=@AddDate,NeedDate= @NeedDate,TrackSub=@TrackSub,Comments=@Comments,IsChange=@IsChange,ChangeDate=@ChangeDate,TrackSubID=@TrackSubID,InvOvr=@InvOvr  where   JobTrackingID=    @JobTrackingID");
                            List<SqlParameter> Param = new List<SqlParameter>();
                            Param.Add(new SqlParameter("@IsChange", 1));
                            Param.Add(new SqlParameter("@ChangeDate", Convert.ToDateTime(DateTime.Now).ToString("MM/dd/yyyy")));
                            //Param.Add(new SqlParameter("@JobListID", DirectCast(grvPreRequirments.Rows[cnt].Cells[14), System.Windows.Forms.DataGridViewComboBoxCell].Value)
                            Param.Add(new SqlParameter("@JobListID", grvPermitsRequiredInspection.Rows[cnt].Cells["JobListID"].Value.ToString()));
                            Param.Add(new SqlParameter("@TaskHandler", grvPermitsRequiredInspection.Rows[cnt].Cells["cmbTaskHandler"].Value.ToString()));
                            Param.Add(new SqlParameter("@Track", grvPermitsRequiredInspection.Rows[cnt].Cells["cmbTrack"].Value.ToString()));
                            Param.Add(new SqlParameter("@Submitted", grvPermitsRequiredInspection.Rows[cnt].Cells["Submitted"].Value.ToString()));
                            Param.Add(new SqlParameter("@BillState", grvPermitsRequiredInspection.Rows[cnt].Cells["cmbBillState"].Value.ToString()));
                            Param.Add(new SqlParameter("@TrackSub", grvPermitsRequiredInspection.Rows[cnt].Cells["TrackSub"].Value.ToString()));
                            Param.Add(new SqlParameter("@Comments", grvPermitsRequiredInspection.Rows[cnt].Cells["Comments"].Value.ToString()));
                            Param.Add(new SqlParameter("@Status", grvPermitsRequiredInspection.Rows[cnt].Cells["cmbStatus"].Value.ToString()));
                            Param.Add(new SqlParameter("@Obtained", grvPermitsRequiredInspection.Rows[cnt].Cells["Obtained"].Value.ToString()));
                            Param.Add(new SqlParameter("@Expires", grvPermitsRequiredInspection.Rows[cnt].Cells["Expires"].Value.ToString()));
                            Param.Add(new SqlParameter("@FinalAction", grvPermitsRequiredInspection.Rows[cnt].Cells["FinalAction"].Value.ToString()));
                            Param.Add(new SqlParameter("@AddDate", grvPermitsRequiredInspection.Rows[cnt].Cells["AddDate"].Value.ToString()));
                            Param.Add(new SqlParameter("@NeedDate", grvPermitsRequiredInspection.Rows[cnt].Cells["NeedDate"].Value.ToString()));
                            Param.Add(new SqlParameter("@JobTrackingID", grvPermitsRequiredInspection.Rows[cnt].Cells["JobTrackingID"].Value.ToString()));
                            Param.Add(new SqlParameter("@TrackSubID", grvPermitsRequiredInspection.Rows[cnt].Cells["TrackSubID"].Value.ToString()));
                            Param.Add(new SqlParameter("@InvOvr", grvPermitsRequiredInspection.Rows[cnt].Cells["InvOvr"].Value.ToString()));

                            if (repo.db.Database.ExecuteSqlCommand(cmd.CommandText, Param.ToArray()) > 0)
                            {

                                grvPermitsRequiredInspection.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                                grvPermitsRequiredInspection.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(159, 207, 255);
                                KryptonMessageBox.Show("Update Successfully", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (System.Exception eLoad)
                        {
                            //Add your error handling code here.
                            //Display error message, if any.
                            KryptonMessageBox.Show(eLoad.Message, "Manager");
                        }
                        //FillGridPermitRequiredInspection()
                        // If grvPermitsRequiredInspection.Rows.Count > 0 Then
                        grvPermitsRequiredInspection.CurrentCell = grvPermitsRequiredInspection.Rows[cnt].Cells["Comments"];
                        grvPermitsRequiredInspection.Rows[cnt].Selected = true;
                        // End Ifremo
                        // System.Windows.Forms.MessageBox.Show("Record Updated!", "Message")

                    }
                    catch (System.Exception eUpdate)
                    {
                        //Add your error handling code here.
                        //Display error message, if any.
                        KryptonMessageBox.Show(eUpdate.Message, "Manager");
                    }
                }
            }
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (grvPermitsRequiredInspection.Columns[e.ColumnIndex].Name == "cmbTrack")
                {
                    //FillPermitGridTrackSubCmb(e.ColumnIndex, e.RowIndex)
                }
            }
        }

        private void grvPreRequirments_CellBeginEdit(System.Object sender, System.Windows.Forms.DataGridViewCellCancelEventArgs e)
        {
            if (grvPreRequirments.Columns[e.ColumnIndex].Name == "cmbTaskHandler")
            {
                if (isDiable(((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == true)
                {
                    DataGridViewComboBoxCell cmbTMCell = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell tempVar = (DataGridViewComboBoxCell)((DataGridView)sender)[e.ColumnIndex, e.RowIndex];
                    tempVar.DataSource = repo.GetcolPMM();
                    tempVar.DisplayMember = "cTrack";
                }
                else
                {
                    DataGridViewComboBoxCell cmbTMCell = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell tempVar2 = (DataGridViewComboBoxCell)((DataGridView)sender)[e.ColumnIndex, e.RowIndex];
                    tempVar2.DataSource = repo.GetPreRequirementcolTM_D();
                    tempVar2.DisplayMember = "cTrack";
                }
            }
            if (e.ColumnIndex > -1 || e.RowIndex > -1)
            {
                CheckString = string.Empty;
                if (Convert.ToInt16(grvPreRequirments.Rows[grvPreRequirments.Rows.Count - 1].Cells["JobListID"].Value.ToString()) == 0)
                {
                    if (grvPreRequirments.CurrentRow.Index == grvPreRequirments.Rows.Count - 1)
                    {
                        return;
                    }
                    KryptonMessageBox.Show("First Save then select for update", "Master List Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
                    return;
                }
                CheckString = grvPreRequirments.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            FillPreRequireGridTrackSubCmb(e.RowIndex);
        }

        private void grvPreRequirments_CellEndEdit(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            FillPreRequireGridTrackSubCmb(e.RowIndex);
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (grvPreRequirments.Columns[e.ColumnIndex].Name == "TrackSub")
                {
                    try
                    {
                        grvPreRequirments.Rows[e.RowIndex].Cells["TrackSubID"].Value = repo.db.Database.SqlQuery<int>("SELECT Id FROM  MasterTrackSubItem WHERe TrackSubName='" + grvPreRequirments.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() + "'").SingleOrDefault();
                    }
                    catch (Exception ex)
                    {
                        cErrorLog.WriteLog("JobStatus", "grvPreRequirments_CellEndEdit", ex.Message);
                    }
                }

                if (grvPreRequirments.Columns[e.ColumnIndex].Name == "InvOvr")
                {
                    try
                    {
                        Regex regexint = new Regex("\\d([1-9]|[$]|[\\.\\d+])\\d");
                        Regex regexdec = new Regex("(^(\\$)(0|([1-9][0-9]*))(\\.[0-9]{1,6})?$)|(^(0{0,1}|([1-9][0-9]*))(\\.[0-9]{1,6})?$)");

                        Match mint = regexint.Match(grvPreRequirments.Rows[e.RowIndex].Cells["InvOvr"].Value.ToString());
                        Match mDec = regexdec.Match(grvPreRequirments.Rows[e.RowIndex].Cells["InvOvr"].Value.ToString());

                        if ((mint.Success & mDec.Success) != false)
                        {

                        }
                        else
                        {
                            grvPreRequirments.Rows[e.RowIndex].Cells["InvOvr"].Value = "";
                            MessageBox.Show("Please Enter Number Only");
                        }
                    }
                    catch (Exception ex)
                    {
                        cErrorLog.WriteLog("JobStatus", "grvPreRequirments_CellEndEdit", ex.Message);
                    }
                }
            }
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (grvPreRequirments.Columns[e.ColumnIndex].Name == "cmbTrack")
                {
                    FillPreRequireGridTrackSubCmb(e.RowIndex);
                }
            }
            try
            {
                if (e.ColumnIndex > -1 || e.RowIndex > -1)
                {
                    if (Convert.ToInt16(grvPreRequirments.Rows[grvPreRequirments.Rows.Count - 1].Cells["JobListID"].Value) == 0)
                    {
                        return;
                    }
                    if (grvPreRequirments.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != CheckString)
                    {
                        grvPreRequirments.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Pink;
                        grvPreRequirments.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Pink;
                        CheckString = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "grvPreRequirments_CellEndEdit", ex.Message);
            }
        }

        private void grvPermitsRequiredInspection_DataError(System.Object sender, System.Windows.Forms.DataGridViewDataErrorEventArgs e)
        {
            if (grvPreRequirments.Columns[e.ColumnIndex].Name == "cmbTaskHandler")
            {
                if (isDiable(((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == true)
                {
                    DataGridViewComboBoxCell cmbTMCell = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell tempVar = (DataGridViewComboBoxCell)((DataGridView)sender)[e.ColumnIndex, e.RowIndex];
                    tempVar.DataSource = repo.GetPreRequirementcolTM();
                    tempVar.DisplayMember = "cTrack";
                }
            }
        }

        private void grvJobList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // CHANGE THE SUB GIRD OF JOB LIST
            try
            {

                selectedJobListID = Convert.ToInt32(grvJobList.Rows[e.RowIndex].Cells["JobListID"].Value);
                isDisabled = Convert.ToBoolean(grvJobList.Rows[e.RowIndex].Cells["IsDisable"].Value);

                if (e.ColumnIndex > -1 & e.RowIndex > -1)
                {
                    if (grvJobList.Columns[e.ColumnIndex].Name == "EmailAddress")
                    {
                    }
                }
                if (e.ColumnIndex > -1 & e.RowIndex > -1)
                {
                    if (grvJobList.Columns[e.ColumnIndex].Name == "Description")
                    {
                    }
                }
                FillGridPreRequirment();
                FillGridPermitRequiredInspection();
                FillGridNotesCommunication();
                if ((isDisabled))
                    disableJob(true);
                else
                    disableJob(false);
                //Todo
                //ChangeDirJobNumber(e.RowIndex);

                // Manage Trafic Light
                //todo
                //ChangeTraficLight(e.RowIndex);
                lblCompanyNo.Text = "Client No:- " + grvJobList.Rows[e.RowIndex].Cells["CompanyNo"].Value.ToString();
                //FillTimeSheeData(sender, e);
                //FillVECostButtonColor();
                //CalculateRevenu calcRevenu = new CalculateRevenu();
            }

            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "grvJobList_CellClick", ex.Message);
            }

            int cnt = e.RowIndex;
            if (e.ColumnIndex > -1 & e.RowIndex > -1)
            {
                if (grvJobList.Columns[e.ColumnIndex].Name == "GrdJobBtnUpdate")
                {
                    if (Convert.ToInt32(grvJobList.Rows[cnt].Cells["JobListID"].Value.ToString()) == 0)
                    {
                        InsertJobList();
                        return;
                    }
                    btnAdd.Text = "Insert";
                    btnDelete.Enabled = true;
                    try
                    {
                        // Attempt to update the datasource.
                        // validate the required value
                        if ((!ValidateRateServTypeValue(cnt)))
                            return;
                        try
                        {
                            //EFDbContext DAL = new EFDbContext();
                            SqlCommand cmd = new SqlCommand("update  JobList set JobNumber= @JobNumber,CompanyID=@CompanyID,DateAdded=@DateAdded,Description= @Description,Handler=@Handler, Address=@Address,Borough=@Borough ,InvoiceClient=@InvoiceClient ,InvoiceContact=@InvoiceContact, InvoiceEmailAddress=@InvoiceEmailAddress, InvoiceACContacts=@InvoiceACContacts, InvoiceACEmail=@InvoiceACEmail, PMrv=@PMrv, ContactsID=@ContactsID, IsChange=@IsChange , ChangeDate=@ChangeDate,OwnerName=@OwnerName,OwnerAddress=@OwnerAddress,OwnerPhone=@OwnerPhone,OwnerFax=@OwnerFax,ACContacts=@ACContacts,ACEmail=@ACEmail,Clienttext=@Clienttext,ContactsEmails=@ContactsEmails, IsDisable=@IsDisable,IsInvoiceHold=@IsInvoiceHold, RateVersionId=@RateVersionId,ServRate=@ServRate, AdminInvoice=@AdminInvoice, TypicalInvoiceType=@TypicalInvoiceType where   JobListID=@JobListID");
                            List<SqlParameter> Param = new List<SqlParameter>();
                            Param.Add(new SqlParameter("@IsChange", 1));
                            Param.Add(new SqlParameter("@ChangeDate", string.Format("MM/dd/yyyy", DateTime.Now)));
                            Param.Add(new SqlParameter("@JobListID", grvJobList.Rows[cnt].Cells["JobListID"].Value.ToString()));
                            Param.Add(new SqlParameter("@JobNumber", grvJobList.Rows[cnt].Cells["JobNumber"].Value.ToString()));
                            Param.Add(new SqlParameter("@DateAdded", grvJobList.Rows[cnt].Cells["DateAdded"].Value.ToString()));
                            Param.Add(new SqlParameter("@Description", grvJobList.Rows[cnt].Cells["Description"].Value.ToString()));
                            Param.Add(new SqlParameter("@Address", grvJobList.Rows[cnt].Cells["Address"].Value.ToString()));
                            Param.Add(new SqlParameter("@Handler", grvJobList.Rows[cnt].Cells["cmbHandler"].Value.ToString()));
                            Param.Add(new SqlParameter("@Borough", grvJobList.Rows[cnt].Cells["Borough"].Value.ToString()));
                            Param.Add(new SqlParameter("@PMrv", grvJobList.Rows[cnt].Cells["cmbPMrv"].Value.ToString()));
                            Param.Add(new SqlParameter("@OwnerName", grvJobList.Rows[cnt].Cells["OwnerName"].Value.ToString()));
                            Param.Add(new SqlParameter("@OwnerAddress", grvJobList.Rows[cnt].Cells["OwnerAddress"].Value.ToString()));
                            Param.Add(new SqlParameter("@OwnerPhone", grvJobList.Rows[cnt].Cells["OwnerPhone"].Value.ToString()));
                            Param.Add(new SqlParameter("@OwnerFax", grvJobList.Rows[cnt].Cells["OwnerFax"].Value.ToString()));
                            Param.Add(new SqlParameter("@ACContacts", grvJobList.Rows[cnt].Cells["ACContacts"].Value.ToString()));
                            Param.Add(new SqlParameter("@ACEmail", grvJobList.Rows[cnt].Cells["ACEmail"].Value.ToString()));
                            Param.Add(new SqlParameter("@Clienttext", grvJobList.Rows[cnt].Cells["Clienttext"].Value.ToString()));
                            Param.Add(new SqlParameter("@ContactsEmails", grvJobList.Rows[cnt].Cells["EmailAddress"].Value.ToString()));
                            Param.Add(new SqlParameter("@IsDisable", grvJobList.Rows[cnt].Cells["IsDisable"].Value));
                            Param.Add(new SqlParameter("@IsInvoiceHold", grvJobList.Rows[cnt].Cells["IsInvoiceHold"].Value));
                            Param.Add(new SqlParameter("@InvoiceClient", grvJobList.Rows[cnt].Cells["InvoiceClient"].Value.ToString()));
                            Param.Add(new SqlParameter("@InvoiceContact", grvJobList.Rows[cnt].Cells["InvoiceContact"].Value.ToString()));
                            Param.Add(new SqlParameter("@InvoiceEmailAddress", grvJobList.Rows[cnt].Cells["InvoiceEmailAddress"].Value.ToString()));
                            Param.Add(new SqlParameter("@InvoiceACContacts", grvJobList.Rows[cnt].Cells["InvoiceACContacts"].Value.ToString()));
                            Param.Add(new SqlParameter("@InvoiceACEmail", grvJobList.Rows[cnt].Cells["InvoiceACEmail"].Value.ToString()));
                            Param.Add(new SqlParameter("@RateVersionId", grvJobList.Rows[cnt].Cells["RateVersionId"].Value));
                            Param.Add(new SqlParameter("@ServRate", grvJobList.Rows[cnt].Cells["ServRate"].Value));
                            Param.Add(new SqlParameter("@AdminInvoice", grvJobList.Rows[cnt].Cells["AdminInvoice"].Value));
                            Param.Add(new SqlParameter("@TypicalInvoiceType", grvJobList.Rows[cnt].Cells["cmbTypicalInvoiceType"].Value));
                            if (grvJobList.Rows[cnt].Cells["Client#"].Value.ToString() == "")
                                Param.Add(new SqlParameter("@CompanyID", 0));
                            else
                                Param.Add(new SqlParameter("@CompanyID", (DataGridViewComboBoxCell)grvJobList.Rows[cnt].Cells["Client#"].Value));
                            int ContactsID;
                            if (grvJobList.Rows[cnt].Cells["ContactsID"].Value.ToString() == "")
                            {
                                Param.Add(new SqlParameter("@ContactsID", 0));
                                ContactsID = 0;
                            }
                            else
                            {
                                ContactsID = Convert.ToInt32(grvJobList.Rows[cnt].Cells["ContactsID"].Value.ToString());
                                new SqlParameter("@ContactsID", grvJobList.Rows[cnt].Cells["ContactsID"].Value.ToString());
                            }

                            if (repo.db.Database.ExecuteSqlCommand(cmd.CommandText, Param.ToArray()) > 0)
                            {
                                grvJobList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                                grvJobList.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(159, 207, 255);
                                KryptonMessageBox.Show("Update Successfully", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }

                        catch (Exception eLoad)
                        {
                            KryptonMessageBox.Show(eLoad.Message, "Manager");
                            cErrorLog.WriteLog("JobStatus", "grvJobList_CellClick", eLoad.Message);
                        }
                        grvJobList.CurrentCell = grvJobList.Rows[cnt].Cells["Address"];
                        grvJobList.Rows[cnt].Selected = true;
                    }
                    catch (Exception eUpdate)
                    {
                        KryptonMessageBox.Show(eUpdate.Message, "Manager");
                        cErrorLog.WriteLog("JobStatus", "grvJobList_CellClick", eUpdate.Message);
                    }
                }
            }
            grvJobList_CellEnter(sender, e);
        }

        private void grvJobList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if ((e.ColumnIndex > -1 & e.RowIndex > -1))
                {
                    DataGridViewComboBoxCell ContactCmb = new DataGridViewComboBoxCell();
                    DataTable DataTableContact = new DataTable();
                    if (grvJobList.Columns[e.ColumnIndex].Name == "Clienttext")
                    {
                    }
                    else if ((grvJobList.Columns[e.ColumnIndex].Name == "Contacts"))
                    {
                        //try
                        //{
                        //    int i =repo.GetValueMemberID(Convert.ToInt32(grvJobList.Rows[grvJobList.CurrentRow.Index].Cells["Client#"].Value), grvJobList.Rows[grvJobList.CurrentRow.Index].Cells["Contacts"].Value.ToString().Trim());
                        //    if (i == 0)
                        //    {
                        //        grvJobList.Rows[grvJobList.CurrentRow.Index].Cells["ContactsID"].Value = "";
                        //        return;
                        //    }

                        //    grvJobList.Rows[e.RowIndex].Cells["ContactsID"].Value = i;
                        //}
                        //catch (Exception ex)
                        //{

                        //}
                    }
                    // invoice contact and invoiceAcContact email address
                    if ((grvJobList.Columns[e.ColumnIndex].Name == "InvoiceContactT" | grvJobList.Columns[e.ColumnIndex].Name == "InvoiceACContactsT") & e.RowIndex > -1)
                    {
                        try
                        {
                            DataGridView datagridview = (DataGridView)sender;
                            datagridview.BeginEdit(true);
                        }
                        catch (Exception ex)
                        {
                            cErrorLog.WriteLog("JobStatus", "grvJobList_CellEnter", ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "grvJobList_CellEnter", ex.Message);
            }
        }

        private void grvJobList_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                selectedJobListID = Convert.ToInt32(grvJobList.Rows[e.RowIndex].Cells["JobListID"].Value);
                isDisabled = Convert.ToBoolean(grvJobList.Rows[e.RowIndex].Cells["IsDisable"].Value);
                //ChangeTraficLight(e.RowIndex);
                FillGridPreRequirment();
                FillGridPermitRequiredInspection();
                FillGridNotesCommunication();
                if ((isDisabled))
                    disableJob(true);
                else
                    disableJob(false);
                lblCompanyNo.Text = "Client No:- " + grvJobList.Rows[e.RowIndex].Cells["CompanyNo"].Value.ToString();
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "grvJobList_RowHeaderMouseClick", ex.Message);
            }
        }

        private void grvJobList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                selectedJobListID = Convert.ToInt32(grvJobList.Rows[grvJobList.CurrentRow.Index].Cells["JobListID"].Value);
                if (Convert.IsDBNull(grvJobList.Rows[grvJobList.CurrentRow.Index].Cells["IsDisable"].Value))
                    isDisabled = Convert.ToBoolean(grvJobList.Rows[grvJobList.CurrentRow.Index].Cells["IsDisable"].Value);
                if (e.KeyCode == Keys.Up)
                {
                    if (grvJobList.CurrentRow.Index != 0)
                    {
                        selectedJobListID = Convert.ToInt32(grvJobList.Rows[grvJobList.CurrentRow.Index - 1].Cells["JobListID"].Value);
                        isDisabled = Convert.ToBoolean(grvJobList.Rows[grvJobList.CurrentRow.Index - 1].Cells["IsDisable"].Value);
                        // ChangeTraficLight(grvJobList.CurrentRow.Index - 1);                    
                        // ChangeDirJobNumber(grvJobList.CurrentRow.Index - 1);
                        FillGridPreRequirment();
                        FillGridPermitRequiredInspection();
                        FillGridNotesCommunication();
                        if ((isDisabled))
                            disableJob(true);
                        else
                            disableJob(false);
                        lblCompanyNo.Text = "Client No:- " + grvJobList.Rows[grvJobList.CurrentRow.Index - 1].Cells["CompanyNo"].Value.ToString();
                    }
                }
                if (e.KeyCode == Keys.Down)
                {
                    if (grvJobList.CurrentRow.Index != grvJobList.Rows.Count - 1)
                    {
                        selectedJobListID = Convert.ToInt32(grvJobList.Rows[grvJobList.CurrentRow.Index + 1].Cells["JobListID"].Value);
                        isDisabled = Convert.ToBoolean(grvJobList.Rows[grvJobList.CurrentRow.Index + 1].Cells["IsDisable"].Value);
                        // Dim JobNo As String = grvJobList.Rows[grvJobList.CurrentRow.Index + 1].Cells["JobNumber"].Value.ToString
                        // ChangeTraficLight(grvJobList.CurrentRow.Index + 1);                    
                        //ChangeDirJobNumber(grvJobList.CurrentRow.Index + 1);
                        FillGridPreRequirment();
                        FillGridPermitRequiredInspection();
                        FillGridNotesCommunication();
                        if ((isDisabled))
                            disableJob(true);
                        else
                            disableJob(false);
                        lblCompanyNo.Text = "Client No:- " + grvJobList.Rows[grvJobList.CurrentRow.Index + 1].Cells["CompanyNo"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "grvJobList_KeyDown", ex.Message);
            }
        }

        private void grvNotesCommunication_CellClick(System.Object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (grvNotesCommunication.Columns[e.ColumnIndex].Name == "cmbBillState")
                {
                    if (mdio.lblLogin.Text == "Admin Login")
                    {
                        grvNotesCommunication.Columns[e.ColumnIndex].ReadOnly = true;
                    }
                    else
                    {
                        grvNotesCommunication.Columns[e.ColumnIndex].ReadOnly = false;
                    }
                }
            }
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (grvNotesCommunication.Columns[e.ColumnIndex].Name == "GrdBtnNotesUpdate")
                {
                    try
                    {
                        //Attempt to update the datasource.
                        int cnt = e.RowIndex;
                        if (Convert.ToInt32(grvNotesCommunication.Rows[cnt].Cells["JobTrackingID"].Value.ToString()) == 0)
                        {
                            InsertNotes();
                            return;
                        }
                        btnInsertNotes.Text = "Insert";
                        btndeleteNotes.Enabled = true;
                        if (string.IsNullOrEmpty(grvNotesCommunication.Rows[grvNotesCommunication.CurrentRow.Index].Cells["Track"].Value.ToString()))
                        {
                            KryptonMessageBox.Show("Track field are compulsory", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (string.IsNullOrEmpty(grvNotesCommunication.Rows[grvNotesCommunication.CurrentRow.Index].Cells["TrackSub"].Value.ToString()))
                        {
                            KryptonMessageBox.Show("TrackSub field are compulsory", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        try
                        {
                            //DataAccessLayer DAL = new DataAccessLayer();
                            SqlCommand cmd = new SqlCommand("update  Jobtracking set JobListID= @JobListID,TaskHandler=@TaskHandler,Track=@Track,Status= @Status,Submitted=@Submitted, Obtained=@Obtained,Expires=@Expires,BillState=@BillState , AddDate=@AddDate,NeedDate= @NeedDate,TrackSub=@TrackSub,Comments=@Comments,IsChange=@IsChange,ChangeDate=@ChangeDate,TrackSubID=@TrackSubID, InvOvr=@InvOvr, DeleteItemTimeService=@DeleteItemTimeService where   JobTrackingID=    @JobTrackingID");

                            List<SqlParameter> Param = new List<SqlParameter>();
                            Param.Add(new SqlParameter("@IsChange", 1));
                            Param.Add(new SqlParameter("@ChangeDate", Convert.ToDateTime(DateTime.Now).ToString("MM/dd/yyyy")));
                            //Param.Add(new SqlParameter("@JobListID", DirectCast(grvPreRequirments.Rows[cnt].Cells[14), System.Windows.Forms.DataGridViewComboBoxCell].Value)
                            Param.Add(new SqlParameter("@JobListID", grvNotesCommunication.Rows[cnt].Cells["JobListID"].Value.ToString()));
                            Param.Add(new SqlParameter("@TaskHandler", grvNotesCommunication.Rows[cnt].Cells["cmbTaskHandler"].Value.ToString()));
                            Param.Add(new SqlParameter("@Track", grvNotesCommunication.Rows[cnt].Cells["cmbTrack"].Value.ToString()));
                            Param.Add(new SqlParameter("@Submitted", grvNotesCommunication.Rows[cnt].Cells["Submitted"].Value.ToString()));
                            Param.Add(new SqlParameter("@BillState", grvNotesCommunication.Rows[cnt].Cells["cmbBillState"].Value.ToString()));
                            Param.Add(new SqlParameter("@TrackSub", grvNotesCommunication.Rows[cnt].Cells["TrackSub"].Value.ToString()));
                            Param.Add(new SqlParameter("@Comments", grvNotesCommunication.Rows[cnt].Cells["Comments"].Value.ToString()));
                            Param.Add(new SqlParameter("@Status", grvNotesCommunication.Rows[cnt].Cells["cmbStatus"].Value.ToString()));
                            Param.Add(new SqlParameter("@Obtained", grvNotesCommunication.Rows[cnt].Cells["Obtained"].Value.ToString()));
                            Param.Add(new SqlParameter("@Expires", grvNotesCommunication.Rows[cnt].Cells["Expires"].Value.ToString()));
                            Param.Add(new SqlParameter("@AddDate", grvNotesCommunication.Rows[cnt].Cells["AddDate"].Value.ToString()));
                            Param.Add(new SqlParameter("@NeedDate", grvNotesCommunication.Rows[cnt].Cells["NeedDate"].Value.ToString()));
                            Param.Add(new SqlParameter("@JobTrackingID", grvNotesCommunication.Rows[cnt].Cells["JobTrackingID"].Value.ToString()));
                            Param.Add(new SqlParameter("@TrackSubID", grvNotesCommunication.Rows[cnt].Cells["TrackSubID"].Value.ToString()));
                            Param.Add(new SqlParameter("@InvOvr", grvNotesCommunication.Rows[cnt].Cells["InvOvr"].Value.ToString()));
                            Param.Add(new SqlParameter("@DeleteItemTimeService", grvNotesCommunication.Rows[cnt].Cells["DeleteItemTimeService"].Value.ToString()));
                            int num = repo.db.Database.ExecuteSqlCommand(cmd.CommandText, Param.ToArray());
                            if (num > 0)
                            {
                                grvNotesCommunication.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                                grvNotesCommunication.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(159, 207, 255);
                                KryptonMessageBox.Show("Update Successfully", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (System.Exception eLoad)
                        {
                            //Add your error handling code here.
                            //Display error message, if any.
                            KryptonMessageBox.Show(eLoad.Message, "Manager");
                        }
                        // FillGridPermitRequiredInspection()
                        //If grvNotesCommunication.Rows.Count > 0 Then
                        grvNotesCommunication.CurrentCell = grvNotesCommunication.Rows[cnt].Cells["Comments"];
                        grvNotesCommunication.Rows[cnt].Selected = true;
                        //End If
                        //  System.Windows.Forms.MessageBox.Show("Record Updated!", "Message")

                    }
                    catch (System.Exception eUpdate)
                    {
                        //Add your error handling code here.
                        //Display error message, if any.
                        KryptonMessageBox.Show(eUpdate.Message, "Manager");
                    }
                }
            }
            //If e.ColumnIndex = 4 And e.RowIndex > -1 Then
            //    FillNotesGridCmb(e.ColumnIndex, e.RowIndex)

            //End If
        }

        private void grvNotesCommunication_CellBeginEdit(System.Object sender, System.Windows.Forms.DataGridViewCellCancelEventArgs e)
        {
            if (grvPreRequirments.Columns[e.ColumnIndex].Name == "cmbTaskHandler")
            {
                if (isDiable(((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == true)
                {
                    DataGridViewComboBoxCell cmbTMCell = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell tempVar = (DataGridViewComboBoxCell)((DataGridView)sender)[e.ColumnIndex, e.RowIndex];
                    tempVar.DataSource = repo.GetPreRequirementcolTM();
                    //AND (isDisable <> 1 or IsDisable is  null)
                    tempVar.DisplayMember = "cTrack";
                }
                else
                {
                    DataGridViewComboBoxCell cmbTMCell = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell tempVar2 = (DataGridViewComboBoxCell)((DataGridView)sender)[e.ColumnIndex, e.RowIndex];
                    tempVar2.DataSource = repo.GetPreRequirementcolTM_D();
                    tempVar2.DisplayMember = "cTrack";
                }
            }
            FillNotesGridCmb(e.ColumnIndex, e.RowIndex);
            try
            {

                if (e.ColumnIndex > -1 || e.RowIndex > -1)
                {
                    CheckString = string.Empty;
                    if (Convert.ToInt16(grvNotesCommunication.Rows[grvNotesCommunication.Rows.Count - 1].Cells["JobListID"].Value.ToString()) == 0)
                    {
                        if (grvNotesCommunication.CurrentRow.Index == grvNotesCommunication.Rows.Count - 1)
                        {
                            return;
                        }
                        KryptonMessageBox.Show("First Save then select for update", "Master List Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        e.Cancel = true;
                        return;
                    }
                    CheckString = grvNotesCommunication[e.ColumnIndex, e.RowIndex].Value.ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void grvNotesCommunication_CellEndEdit(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            FillNotesGridCmb(e.ColumnIndex, e.RowIndex);
            //If e.ColumnIndex = 4 And e.RowIndex > -1 Then
            //    FillNotesGridCmb(e.ColumnIndex, e.RowIndex)
            //End If
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (grvNotesCommunication.Columns[e.ColumnIndex].Name == "TrackSub")
                {
                    try
                    {
                        if (e.ColumnIndex > -1 && e.RowIndex > -1)
                            grvNotesCommunication.Rows[e.RowIndex].Cells["TrackSubID"].Value =repo.db.Database.SqlQuery<int> ("SELECT Id FROM  MasterTrackSubItem WHERe TrackSubName='" + grvNotesCommunication.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() + "' AND (IsDelete=0 or IsDelete IS NULL)").FirstOrDefault();
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        var data = (InvoiceTypeRate)repo.GetInvoiceTypeRate(Convert.ToInt32(grvJobList.CurrentRow.Cells["CompanyID"].Value)).SingleOrDefault();

                        string InvoiceType = string.IsNullOrEmpty(grvJobList.CurrentRow.Cells["TypicalInvoiceType"].Value.ToString()) ? (string.IsNullOrEmpty(data.TypicalInvoiceType) ? "Item" : data.TypicalInvoiceType) : grvJobList.CurrentRow.Cells["TypicalInvoiceType"].Value.ToString();

                        string servRate = string.IsNullOrEmpty(grvJobList.CurrentRow.Cells["ServRate"].Value.ToString()) ? (string.IsNullOrEmpty(data.ServRate) ? "1" : data.ServRate) : grvJobList.CurrentRow.Cells["ServRate"].Value.ToString();

                        if (InvoiceType == "Time" && grvNotesCommunication.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == TIMESERVICEFEE)
                        {
                            MessageBox.Show("Invoice Type is Time already", "Manager");
                            grvNotesCommunication.CancelEdit();
                        }
                        else if ((InvoiceType == "Item") && grvNotesCommunication.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == TIMESERVICEFEE)
                        {
                            //TODO
                            ////frmPopupInvoiceTime frmpopup = new frmPopupInvoiceTime();
                            ////frmpopup.jobID = grvJobList.CurrentRow.Cells["JobListID"].Value;
                            ////frmpopup.TimeFactorServRate = servRate;
                            ////frmpopup.ShowDialog();
                            ////if (frmpopup.DialogResult == DialogResult.Yes)
                            ////{
                            ////    grvNotesCommunication.Rows[e.RowIndex].Cells["Comments"].Value = frmpopup.InsertString;
                            ////    grvNotesCommunication.Rows[e.RowIndex].Cells["DeleteItemTimeService"].Value = frmpopup.DeleteItems;
                            ////}
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }


                if (grvNotesCommunication.Columns[e.ColumnIndex].Name == "InvOvr")
                {
                    try
                    {
                        Regex regexint = new Regex("\\d([1-9]|[$]|[\\.\\d+])\\d");
                        Regex regexdec = new Regex("(^(\\$)(0|([1-9][0-9]*))(\\.[0-9]{1,6})?$)|(^(0{0,1}|([1-9][0-9]*))(\\.[0-9]{1,6})?$)");

                        Match mint = regexint.Match(grvNotesCommunication.Rows[e.RowIndex].Cells["InvOvr"].Value.ToString());
                        Match mDec = regexdec.Match(grvNotesCommunication.Rows[e.RowIndex].Cells["InvOvr"].Value.ToString());

                        if ((mint.Success & mDec.Success) != false)
                        {

                        }
                        else
                        {
                            grvNotesCommunication.Rows[e.RowIndex].Cells["InvOvr"].Value = "";
                            MessageBox.Show("Please Enter Number Only");
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            try
            {
                if (e.ColumnIndex > -1 || e.RowIndex > -1)
                {
                    if (Convert.ToInt16(grvNotesCommunication.Rows[grvNotesCommunication.Rows.Count - 1].Cells["JobListID"].Value.ToString()) == 0)
                    {
                        return;
                    }
                    if (grvNotesCommunication.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != CheckString)
                    {
                        grvNotesCommunication.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Pink;
                        grvNotesCommunication.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Pink;
                        CheckString = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void grvNotesCommunication_DataError(object sender, System.Windows.Forms.DataGridViewDataErrorEventArgs e)
        {
            if (grvPreRequirments.Columns[e.ColumnIndex].Name == "cmbTaskHandler")
            {
                if (isDiable(((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == true)
                {
                    DataGridViewComboBoxCell cmbTMCell = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell tempVar = (DataGridViewComboBoxCell)((DataGridView)sender)[e.ColumnIndex, e.RowIndex];
                    tempVar.DataSource = repo.GetPreRequirementcolTM();

                    tempVar.DisplayMember = "cTrack";
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Insert")
            {
                for (int i = 0; i < grvJobList.Rows.Count; i++)
                {
                    if (grvJobList.Rows[i].DefaultCellStyle.BackColor == Color.Pink)
                    {
                        KryptonMessageBox.Show("you can't insert new record first Update and then insert", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                AutoJB = repo.AutoJobnumber();

                btnAdd.Text = "Save";
                btnDelete.Enabled = false;
                DataRow datarow = dtJL.NewRow();
                datarow["JobListID"] = 0;
                datarow["JobNumber"] = AutoJB.ToString();


                //**** Auto Insert Setting  *****
                try
                {

                    XmlDocument myDoc = new XmlDocument();

                    myDoc.Load(Application.StartupPath + "\\VESoftwareSetting.xml");

                    if (myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Apply"].InnerText == "Yes")
                    {

                        datarow["Handler"] = myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Joblist"]["PM"].InnerText;
                        //datarow("PMrv") = myDoc("VESoftwareSetting")("AutoInsert")("Manager")("Joblist")("PMrv").InnerText
                    }
                    else
                    {
                        datarow["Handler"] = "";
                        //datarow("PMrv") = ""
                    }
                }
                catch (Exception ex)
                {
                    datarow["Handler"] = "";
                    //datarow("PMrv") = ""
                }

                // datarow("Client") = ""
                datarow["Description"] = "";
                datarow["Address"] = "";
                datarow["Contacts"] = "";
                datarow["Borough"] = "";
                datarow["DateAdded"] = DateTime.Now;
                datarow["EmailAddress"] = "";
                datarow["OwnerName"] = "";
                datarow["OwnerAddress"] = "";
                datarow["OwnerPhone"] = "";
                datarow["OwnerFax"] = "";
                datarow["ACContacts"] = "";
                datarow["ACEmail"] = "";
                //datarow("PMrv") = ""
                datarow["InvoiceClient"] = 0;
                datarow["InvoiceContact"] = "";
                datarow["InvoiceEmailAddress"] = "";
                datarow["InvoiceACContacts"] = "";
                datarow["InvoiceACEmail"] = "";
                datarow["IsDisable"] = false;
                datarow["IsInvoiceHold"] = false;
                datarow["AdminInvoice"] = false;

                try
                {
                    XmlDocument myDoc = new XmlDocument();

                    myDoc.Load(Application.StartupPath + "\\VESoftwareSetting.xml");
                    if (myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Apply"].InnerText == "Yes")
                    {

                        datarow["PMrv"] = myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Joblist"]["PMrv"].InnerText;
                    }
                    else
                    {
                        datarow["PMrv"] = "";
                    }
                }
                catch (Exception ex)
                {
                    datarow["PMrv"] = "";
                }

                dtJL.Rows.Add(datarow);
                grvJobList.DataSource = dtJL;
                grvJobList.CurrentCell = grvJobList.Rows[grvJobList.Rows.Count - 1].Cells["JobNumber"];
                selectedJobListID = -1;
                // isDisabled = Convert.ToBoolean(grvJobList.Rows[grvJobList.Rows.Count - 1].Cells["IsDisable"].Value)
                grvJobList.Rows[grvJobList.CurrentRow.Index].DefaultCellStyle.SelectionBackColor = Color.Gold;
                grvJobList.Rows[grvJobList.CurrentRow.Index].DefaultCellStyle.BackColor = Color.Gold;
                FillGridPreRequirment();
                FillGridNotesCommunication();
                FillGridPermitRequiredInspection();

                //If (isDisabled) Then
                //    disableJob(True)
                //Else
                //    disableJob(False)
                //End If
                //grdJobTracking.Rows[grdJobTracking.Rows.Count - 1).Selected = True
            }
            else
            {
                InsertJobList();
            }
        }

        private void DirListBox1_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            //////FileListBox1.Path = DirListBox1.Path;
        }

        private void FileListBox1_DoubleClick(System.Object sender, System.EventArgs e)
        {

            ////string fExt = string.Empty;
            ////string filePath = FileListBox1.Path + "\\";
            ////try
            ////{
            ////    if ((FileListBox1.FileName.LastIndexOf(".") + 1) != 0)
            ////    {
            ////        System.Diagnostics.Process.Start(filePath + FileListBox1.FileName);
            ////    }
            ////}
            ////catch (Exception ex)
            ////{
            ////    KryptonMessageBox.Show(ex.Message, "Message");
            ////}
        }

        private void btnInsertPreReq_Click(object sender, EventArgs e)
        {
            if (btnInsertPreReq.Text == "Insert")
            {
                for (Int32 i = 0; i < grvPreRequirments.Rows.Count; i++)
                {
                    if (grvPreRequirments.Rows[i].DefaultCellStyle.BackColor == Color.Pink)
                    {
                        // KryptonMessageBox.Show("you can't insert new record first Update and then insert", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        return;
                    }
                }
                btnInsertPreReq.Text = "Save";
                btnDeletePreReq.Enabled = false;
                DataRow datarow = dtPreReq.NewRow();
                datarow["JobListID"] = 0;
                datarow["JobNumber"] = "";

                datarow["Track"] = "";

                datarow["Status"] = "Pending";

                try
                {
                    XmlDocument myDoc = new XmlDocument();

                    myDoc.Load(Application.StartupPath + "\\VESoftwareSetting.xml");

                    if (myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Apply"].InnerText == "Yes")
                    {
                        datarow["TaskHandler"] = myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Prerequired"]["TM"].InnerText;
                        datarow["Track"] = myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Prerequired"]["Track"].InnerText;
                        datarow["TrackSub"] = myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Prerequired"]["TrackSub"].InnerText;
                    }
                    else
                    {
                        datarow["TaskHandler"] = "";
                        datarow["Track"] = "";
                        datarow["TrackSub"] = "";
                    }
                }
                catch (Exception ex)
                {
                    datarow["TaskHandler"] = "";
                    datarow["Track"] = "";
                    datarow["TrackSub"] = "";
                }

                //If CheckUser = True Then

                //    If UserType = "User" Then
                //        datarow("TaskHandler") = UserName.ToString()
                //        datarow("Track") = "Client Re;"
                //        datarow("TrackSub") = "Client-> Miscell;"
                //    Else
                //        datarow("TaskHandler") = ""
                //        datarow("Track") = ""
                //        datarow("TrackSub") = ""
                //    End If
                //Else
                //    datarow("TaskHandler") = ""
                //    datarow("Track") = ""
                //    datarow("TrackSub") = ""
                //    MessageBox.Show("Data entered for 'MANAGER.B12'  in settings data sheet does not match any option.  '' is substituted.", "Error handling", MessageBoxButtons.OK, MessageBoxIcon.Information)
                //End If
                datarow["Submitted"] = "1/1/1900";
                datarow["Obtained"] = "1/1/1900";
                datarow["Expires"] = "12/30/9999";
                datarow["BillState"] = "";
                datarow["AddDate"] = DateTime.Now;
                datarow["NeedDate"] = "12/30/9999";
                datarow["JobTrackingID"] = 0;

                try
                {
                    XmlDocument myDoc = new XmlDocument();

                    myDoc.Load(Application.StartupPath + "\\VESoftwareSetting.xml");

                    if (myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Apply"].InnerText == "Yes")
                    {
                        datarow["Comments"] = myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Prerequired"]["Comments"].InnerText;
                    }
                    else
                    {
                        datarow["Comments"] = "";
                    }
                }
                catch (Exception ex)
                {
                    datarow["Comments"] = "";
                }

                if (Convert.ToString(datarow["TrackSub"]) != "")
                {

                    ////////DataTable dt = new DataTable();
                    //////////DataAccessLayer Dl = new DataAccessLayer();
                    ////////dt = new Filldatatable("select * from MasterTrackSubItem WHERE  (IsDelete=0 or IsDelete IS NULL) and  TrackSubName = '" + datarow["TrackSub"].ToString() + "' and TrackName = '" + datarow["Track"].ToString() + "'");
                    ////////if (dt.Rows.Count > 0)
                    ////////{
                    ////////    datarow["TrackSubID"] = dt.Rows[0]["id"].ToString();
                    ////////}
                    ///
                    ///
                    datarow["TrackSubID"] = repo.GetTrackSubId(datarow["TrackSub"].ToString(), datarow["Track"].ToString());
                }
                else
                {
                    datarow["TrackSubID"] = 0;
                }
                //datarow("TrackSubID") = 1
                dtPreReq.Rows.Add(datarow);
                grvPreRequirments.DataSource = dtPreReq;
                grvPreRequirments.CurrentCell = grvPreRequirments.Rows[grvPreRequirments.Rows.Count - 1].Cells["comments"];
                grvPreRequirments.Rows[grvPreRequirments.Rows.Count - 1].Selected = true;
                grvPreRequirments.Rows[grvPreRequirments.CurrentRow.Index].DefaultCellStyle.SelectionBackColor = Color.Gold;
                grvPreRequirments.Rows[grvPreRequirments.CurrentRow.Index].DefaultCellStyle.BackColor = Color.Gold;
                //grdJobTracking.Rows[grdJobTracking.Rows.Count - 1).Selected = True

            }
            else
            {
                if (btnAdd.Text == "Save")
                {
                    // KryptonMessageBox.Show("you can't save first save job list", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    return;
                }
                else
                {
                    InsertPreReq();
                }

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnDelete.Enabled = true;
            btnAdd.Text = "Insert";
            fillGridJobList();
            if (grvJobList.Rows.Count > 0)
            {
                grvJobList.CurrentCell = grvJobList.Rows[grvJobList.Rows.Count - 1].Cells["Address"];
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (UserType != "Admin")
            {
                KryptonMessageBox.Show("Only Admin Can Delete Records");
                return;
            }
            else
            {

            }

            int id = 0;
            int rowIndex = 0;
            if (grvJobList.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow SelectedRow in grvJobList.SelectedRows)
                {
                    id = Convert.ToInt32(SelectedRow.Cells["JobListID"].Value.ToString());
                    rowIndex = SelectedRow.Index;
                }
            }
            if (id == 0)
            {
                KryptonMessageBox.Show("Select a row to delete", "Message");
                return;
            }
            if (KryptonMessageBox.Show("Are you sure you want to delete this record? ", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                try
                {
                    //////DataAccessLayer DAL = new DataAccessLayer();
                    //cmd = New SqlCommand("delete from JobList where JobListID=" & id, sqlcon)
                    //int num = new InsertRecord("Update JobList SET IsDelete=1 where JobListID=" + id);
                    int num = repo.db.Database.ExecuteSqlCommand("Update JobList SET IsDelete=1 where JobListID=" + id);

                    if (num > 0)
                    {
                        fillGridJobList();
                        KryptonMessageBox.Show("Record Deleted!", "Manager");
                        repo.LoginActivityInfo(repo.db, "Delete", this.Text);
                    }

                    if (grvJobList.Rows.Count > 1)
                    {
                        grvJobList.Rows[rowIndex - 1].Selected = true;
                        grvJobList.CurrentCell = grvJobList.Rows[rowIndex - 1].Cells["Description"];
                    }
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show(ex.Message, "Manager");
                }
            }

        }

        private void DriveListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnDeletePreReq_Click(object sender, EventArgs e)
        {
            if (UserType != "Admin")
            {
                KryptonMessageBox.Show("Only Admin Can Delete Records");
                return;
            }

            int id = 0;
            int rowIndex = 0;
            /////TODO
            ////foreach (Form frm in mdio.MdiChildren)
            ////{
            ////    if (frm.Text == RptInvoiceView.Text)
            ////    {
            ////        KryptonMessageBox.Show("First close " + RptInvoiceView.Text + " then delete", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            ////        return;
            ////    }
            ////    if (frm.Text == frmInvoiceEditRPT.Text)
            ////    {
            ////        KryptonMessageBox.Show("First close " + frmInvoiceEditRPT.Text + " then delete", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            ////        return;
            ////    }
            ////}
            if (grvPreRequirments.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow SelectedRow in grvPreRequirments.SelectedRows)
                {
                    id = Convert.ToInt32(SelectedRow.Cells["JobTrackingID"].Value.ToString());
                    rowIndex = SelectedRow.Index;
                }
            }
            if (id == 0)
            {
                KryptonMessageBox.Show("Select a row to delete", "Message");
                return;
            }
            if (KryptonMessageBox.Show("Are you sure you want to delete this record? ", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                try
                {
                    //DataAccessLayer DAL = new DataAccessLayer();
                    //cmd = New SqlCommand("delete from JobTracking where JobTrackingID=" & id, sqlcon)
                    int num = repo.db.Database.ExecuteSqlCommand("UPDATE JobTracking SET IsDelete=1 where JobTrackingID=" + id);
                    if (num > 0)
                    {
                        FillGridPreRequirment();
                        KryptonMessageBox.Show("Record Deleted!", "Manager");
                        repo.LoginActivityInfo(repo.db, "Delete", this.Text);
                    }
                    if (grvPreRequirments.Rows.Count > 1)
                    {
                        grvPreRequirments.Rows[rowIndex - 1].Selected = true;
                        grvPreRequirments.CurrentCell = grvPreRequirments.Rows[rowIndex - 1].Cells["Obtained"];
                    }
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show(ex.Message, "Manager");
                }
            }

        }

        private void btnCancelPreReq_Click(object sender, EventArgs e)
        {
            btnDeletePreReq.Enabled = true;
            btnInsertPreReq.Text = "Insert";
            FillGridPreRequirment();
        }

        private void btnInsertPermit_Click(object sender, EventArgs e)
        {
            if (btnInsertPermit.Text == "Insert")
            {
                for (int i = 0; i < grvPermitsRequiredInspection.Rows.Count; i++)
                {
                    if (grvPermitsRequiredInspection.Rows[i].DefaultCellStyle.BackColor == Color.Pink)
                    {
                        // KryptonMessageBox.Show("you can't insert new record first Update and then insert", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        return;
                    }
                }
                btnInsertPermit.Text = "Save";
                btnDeletePermit.Enabled = false;
                DataRow datarow = dtPermit.NewRow();
                datarow["JobListID"] = 0;
                datarow["JobNumber"] = "";

                try
                {
                    XmlDocument myDoc = new XmlDocument();

                    myDoc.Load(Application.StartupPath + "\\VESoftwareSetting.xml");

                    if (myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Apply"].InnerText == "Yes")
                    {
                        datarow["TaskHandler"] = myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Permit"]["TM"].InnerText;
                        datarow["Track"] = myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Permit"]["Track"].InnerText;
                        datarow["TrackSub"] = myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Permit"]["TrackSub"].InnerText;
                    }
                    else
                    {
                        datarow["TaskHandler"] = "";
                        datarow["Track"] = "";
                        datarow["TrackSub"] = "";
                    }
                }
                catch (Exception ex)
                {
                    datarow["TaskHandler"] = "";
                    datarow["Track"] = "";
                    datarow["TrackSub"] = "";
                }

                //if (CheckUser == true)
                //{

                //    if (UserType == "User")
                //    {
                //        datarow("TaskHandler") = UserName.ToString();
                //        datarow("Track") = "VE Requ;";
                //        datarow("TrackSub") = "Miscellaneous->";

                //    }
                //    else
                //    {
                //        datarow("TaskHandler") = "";
                //        datarow("Track") = "";
                //        datarow("TrackSub") = "";

                //    }
                //}
                //else
                //{
                //    datarow("TaskHandler") = "";
                //    datarow("Track") = "";
                //    datarow("TrackSub") = "";
                //    MessageBox.Show("Data entered for 'MANAGER.B12'  in settings data sheet does not match any option.  '' is substituted.", "Error handling", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}

                datarow["Status"] = "Pending";
                datarow["Submitted"] = "1/1/1900";
                datarow["Obtained"] = "1/1/1900";
                datarow["Expires"] = "12/30/9999";
                datarow["FinalAction"] = "No Action";
                datarow["BillState"] = "Not Invoiced";
                datarow["AddDate"] = DateTime.Now;
                datarow["NeedDate"] = "12/30/9999";
                datarow["JobTrackingID"] = 0;

                try
                {
                    XmlDocument myDoc = new XmlDocument();

                    myDoc.Load(Application.StartupPath + "\\VESoftwareSetting.xml");

                    if (myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Apply"].InnerText == "Yes")
                    {
                        datarow["Comments"] = myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Permit"]["Comments"].InnerText;

                    }
                    else
                    {
                        datarow["Comments"] = "";
                    }
                }
                catch (Exception ex)
                {
                    datarow["Comments"] = "";
                }

                if (Convert.ToString(datarow["TrackSub"]) != "")
                {

                    ////DataTable dt = new DataTable();
                    ////DataAccessLayer Dl = new DataAccessLayer();
                    ////dt = Dl.Filldatatable("select * from MasterTrackSubItem WHERE  (IsDelete=0 or IsDelete IS NULL) and  TrackSubName = '" + datarow["TrackSub"].ToString() + "' and TrackName = '" + datarow["Track"].ToString() + "'");
                    ////if (dt.Rows.Count > 0)
                    ////{
                    ////    datarow["TrackSubID"] = dt.Rows[0]["id"].ToString();
                    ////}
                    ///
                    datarow["TrackSubID"] = repo.GetTrackSubId(datarow["TrackSub"].ToString(), datarow["Track"].ToString());
                }
                else
                {
                    datarow["TrackSubID"] = 0;
                }
                //datarow("TrackSubID") = 1
                dtPermit.Rows.Add(datarow);
                grvPermitsRequiredInspection.DataSource = dtPermit;
                grvPermitsRequiredInspection.CurrentCell = grvPermitsRequiredInspection.Rows[grvPermitsRequiredInspection.Rows.Count - 1].Cells["comments"];
                grvPermitsRequiredInspection.Rows[grvPermitsRequiredInspection.Rows.Count - 1].Selected = true;
                grvPermitsRequiredInspection.Rows[grvPermitsRequiredInspection.CurrentRow.Index].DefaultCellStyle.SelectionBackColor = Color.Gold;
                grvPermitsRequiredInspection.Rows[grvPermitsRequiredInspection.CurrentRow.Index].DefaultCellStyle.BackColor = Color.Gold;
                //grdJobTracking.Rows[grdJobTracking.Rows.Count - 1).Selected = True
            }
            else
            {
                if (btnAdd.Text == "Save")
                {
                    // KryptonMessageBox.Show("you can't save first save job list", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    return;
                }
                else
                {
                    InsertPermits();
                }
            }

        }

        private void btnDeletePermit_Click(object sender, EventArgs e)
        {
            if (UserType != "Admin")
            {
                KryptonMessageBox.Show("Only Admin Can Delete Records");
                return;
            }

            int id = 0;
            int rowIndex = 0;
            //foreach (Form frm in mdio.MdiChildren)
            //{
            //    ///need to add form - TODO
            //    //////if (frm.Text == RptInvoiceView.Text)
            //    //////{
            //    //////    KryptonMessageBox.Show("First close " + RptInvoiceView.Text + " then delete", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    //////    return;
            //    //////}
            //    //////if (frm.Text == frmInvoiceEditRPT.Text)
            //    //////{
            //    //////    KryptonMessageBox.Show("First close " + frmInvoiceEditRPT.Text + " then delete", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    //////    return;
            //    //////}
            //}
            if (grvPermitsRequiredInspection.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow SelectedRow in grvPermitsRequiredInspection.SelectedRows)
                {
                    id = Convert.ToInt32(SelectedRow.Cells["JobTrackingID"].Value.ToString());
                    rowIndex = SelectedRow.Index;
                }
            }
            if (id == 0)
            {
                KryptonMessageBox.Show("Select a row to delete", "Message");
                return;
            }
            if (KryptonMessageBox.Show("Are you sure you want to delete this record? ", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                try
                {
                    //DataAccessLayer DAL = new DataAccessLayer();
                    int num = repo.db.Database.ExecuteSqlCommand("Update JobTracking SET IsDelete=1 where JobTrackingID=" + id);
                    if (num > 0)
                    {
                        FillGridPermitRequiredInspection();
                        KryptonMessageBox.Show("Record Deleted!", "Manager");
                        repo.LoginActivityInfo(repo.db, "Delete", this.Text);
                    }
                    if (grvPermitsRequiredInspection.Rows.Count > 1)
                    {
                        grvPermitsRequiredInspection.Rows[rowIndex - 1].Selected = true;
                        grvPermitsRequiredInspection.CurrentCell = grvPermitsRequiredInspection.Rows[rowIndex - 1].Cells["Obtained"];
                    }
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show(ex.Message, "Manager");

                }
            }
        }

        private void btnCancelPermit_Click(object sender, EventArgs e)
        {
            btnDeletePermit.Enabled = true;
            btnInsertPermit.Text = "Insert";
            FillGridPermitRequiredInspection();
        }

        private void btnInsertNotes_Click(object sender, EventArgs e)
        {
            if (btnInsertNotes.Text == "Insert")
            {
                for (int i = 0; i < grvNotesCommunication.Rows.Count; i++)
                {
                    if (grvNotesCommunication.Rows[i].DefaultCellStyle.BackColor == Color.Pink)
                    {
                        // KryptonMessageBox.Show("you can't insert new record first Update and then insert", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        return;
                    }
                }
                btnInsertNotes.Text = "Save";
                btndeleteNotes.Enabled = false;
                DataRow datarow = dtNotes.NewRow();
                datarow["JobListID"] = selectedJobListID;
                datarow["JobNumber"] = "";

                try
                {
                    XmlDocument myDoc = new XmlDocument();

                    myDoc.Load(Application.StartupPath + "\\VESoftwareSetting.xml");

                    if (myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Apply"].InnerText == "Yes")
                    {
                        datarow["TaskHandler"] = myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Notes"]["TM"].InnerText;
                        datarow["Track"] = myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Notes"]["Track"].InnerText;
                        datarow["TrackSub"] = myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Notes"]["TrackSub"].InnerText;
                    }
                    else
                    {
                        datarow["TaskHandler"] = "";
                        datarow["Track"] = "";
                        datarow["TrackSub"] = "";
                    }
                }
                catch (Exception ex)
                {
                    datarow["TaskHandler"] = "";
                    datarow["Track"] = "";
                    datarow["TrackSub"] = "";
                }

                datarow["Status"] = "Pending";
                datarow["Submitted"] = "1/1/1900";
                datarow["Obtained"] = "1/1/1900";
                datarow["Expires"] = "12/30/9999";
                datarow["BillState"] = "Not Invoiced";
                datarow["AddDate"] = DateTime.Now;
                datarow["NeedDate"] = "12/30/9999";
                datarow["JobTrackingID"] = 0;

                try
                {
                    XmlDocument myDoc = new XmlDocument();

                    myDoc.Load(Application.StartupPath + "\\VESoftwareSetting.xml");

                    if (myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Apply"].InnerText == "Yes")
                    {
                        datarow["Comments"] = myDoc["VESoftwareSetting"]["AutoInsert"]["Manager"]["Notes"]["Comments"].InnerText;

                    }
                    else
                    {
                        datarow["Comments"] = "";
                    }
                }
                catch (Exception ex)
                {
                    datarow["Comments"] = "";
                }

                if (Convert.ToString(datarow["TrackSub"]) != "")
                {
                    datarow["TrackSubID"] = repo.GetTrackSubId(datarow["TrackSub"].ToString(), datarow["Track"].ToString());
                }
                else
                {
                    datarow["TrackSubID"] = 0;
                }

                //datarow("Comments") = ""
                //datarow("TrackSubID") = 1
                dtNotes.Rows.Add(datarow);
                grvNotesCommunication.DataSource = dtNotes;
                grvNotesCommunication.CurrentCell = grvNotesCommunication.Rows[grvNotesCommunication.Rows.Count - 1].Cells["comments"];
                grvNotesCommunication.Rows[grvNotesCommunication.Rows.Count - 1].Selected = true;
                grvNotesCommunication.Rows[grvNotesCommunication.CurrentRow.Index].DefaultCellStyle.SelectionBackColor = Color.Gold;
                grvNotesCommunication.Rows[grvNotesCommunication.CurrentRow.Index].DefaultCellStyle.BackColor = Color.Gold;
                //grdJobTracking.Rows[grdJobTracking.Rows.Count - 1).Selected = True

            }
            else
            {
                if (btnAdd.Text == "Save")
                {
                    //  KryptonMessageBox.Show("you can't save first save job list", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    return;
                }
                else
                {
                    InsertNotes();
                }
            }
        }

        private void btndeleteNotes_Click(object sender, EventArgs e)
        {
            if (UserType != "Admin")
            {
                KryptonMessageBox.Show("Only Admin Can Delete Records");
                return;
            }

            int id = 0;
            int rowIndex = 0;
            //Need to add form - TODO
            ////foreach (Form frm in mdio.MdiChildren)
            ////{
            ////    if (frm.Text == RptInvoiceView.Text)
            ////    {
            ////        KryptonMessageBox.Show("First close " + RptInvoiceView.Text + " then delete", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            ////        return;
            ////    }
            ////    if (frm.Text == frmInvoiceEditRPT.Text)
            ////    {
            ////        KryptonMessageBox.Show("First close " + frmInvoiceEditRPT.Text + " then delete", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            ////        return;
            ////    }
            ////}
            if (grvNotesCommunication.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow SelectedRow in grvNotesCommunication.SelectedRows)
                {
                    id = Convert.ToInt32(SelectedRow.Cells["JobTrackingID"].Value.ToString());
                    rowIndex = SelectedRow.Index;
                }
            }
            if (id == 0)
            {
                KryptonMessageBox.Show("Select a row to delete", "Message");
                return;
            }
            if (KryptonMessageBox.Show("Are you sure you want to delete this record? ", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    //DataAccessLayer DAL = new DataAccessLayer();
                    int num = repo.db.Database.ExecuteSqlCommand("UPDATE JobTracking SET IsDelete=1 where JobTrackingID=" + id);

                    if (num > 0)
                    {
                        FillGridNotesCommunication();
                        KryptonMessageBox.Show("Record Deleted!", "Manager");
                        repo.LoginActivityInfo(repo.db, "Delete", this.Text);
                    }
                    if (grvNotesCommunication.Rows.Count > 1)
                    {
                        grvNotesCommunication.Rows[rowIndex - 1].Selected = true;
                        grvNotesCommunication.CurrentCell = grvNotesCommunication.Rows[rowIndex - 1].Cells["AddDate"];
                    }
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show(ex.Message, "Manager");
                }
            }
        }

        private void btnCancelNotes_Click(object sender, EventArgs e)
        {
            btndeleteNotes.Enabled = true;
            btnInsertNotes.Text = "Insert";
            FillGridNotesCommunication();
        }

        private void chkPreRequirment_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            //try
            //{
            //    if (chkPreRequirment.Checked == false)
            //    {
            //        pnlButtonVisible(pnlPreRequire, false);
            //        tblpnlJobtrackingGrid.RowStyles[0].SizeType = SizeType.Absolute;
            //        btnShowTimeData.Visible = true;
            //    }
            //    else
            //    {
            //        pnlButtonVisible(pnlPreRequire, true);
            //        tblpnlJobtrackingGrid.RowStyles[0].SizeType = SizeType.Percent;
            //        btnShowTimeData.Visible = true;
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }
        #endregion

        //public int GetValueMemberID()
        //{
        //    string Query = "SELECT ContactsID,dbo.ClientName(FirstName, MiddleName, LastName) as ClientName FROM  Contacts WHERE CompanyID=" + (System.Windows.Forms.DataGridViewComboBoxCell)grvJobList.Rows[grvJobList.CurrentRow.Index].Cells["Client#"].Value + " ORDER BY FirstName";
        //    //DataAccessLayer DA = new DataAccessLayer();
        //    DataTable DataTableContact = new DataTable();
        //    DataTableContact = DA.Filldatatable(Query);
        //    for (int i = 0; i <= DataTableContact.Rows.Count - 1; i++)
        //    {
        //        if (DataTableContact.Rows[i]["ClientName"].ToString().Trim() == grvJobList["Contacts", grvJobList.CurrentRow.Index].Value.ToString().Trim())
        //            return DataTableContact.Rows[i]["ContactsID"].ToString();
        //    }
        //}

        #region Functions & Methods

        private void FillNotesGridCmb(int colmnIndex, int roIndex)
        {
            cmbTCTackName = new DataGridViewComboBoxCell();
            try
            {
                var data = repo.GetTrackSubItem(grvNotesCommunication.Rows[roIndex].Cells["cmbTrack"].Value.ToString().Trim());
                cmbTCTackName.DataSource = data;
                cmbTCTackName.DisplayMember = "TrackSubName"; //data.Select(x => x.TrackSubName).FirstOrDefault().ToString();
                //grvNotesCommunication.Rows[roIndex].Cells[5].Value = cmbTCTackName.Value;
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "FillNotesGridCmb", ex.Message);
            }
        }

        private bool isDiable(string item)
        {
            try
            {
                int num = repo.db.Database.SqlQuery<int>("select COUNT(*) from masteritem where cGroup='TM' and IsDisable=1 and cTrack='" + item + "'").FirstOrDefault();
                return num > 0;
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "isDiable", ex.Message);
            }
            return false;
        }

        private void FillPreRequireGridTrackSubCmb(int roIndex)
        {
            cmbTCTackName = new DataGridViewComboBoxCell();
            try
            {
                var data = repo.GetTrackSubItem(grvPreRequirments.Rows[roIndex].Cells["Track"].Value.ToString().Trim());
                cmbTCTackName.DataSource = data;
                cmbTCTackName.DisplayMember = data.Select(x => x.TrackSubName).FirstOrDefault().ToString();
                grvPreRequirments.Rows[roIndex].Cells[5].Value = cmbTCTackName;
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "FillPreRequireGridTrackSubCmb", ex.Message);
            }
        }

        private void fillGridJobList()
        {
            try
            {
                string queryString = "";

                if (this.txtJobListJobID.Text != "")
                    queryString = queryString + " and JobList.JobNumber Like'%" + txtJobListJobID.Text + "%'";

                if (this.txtJobListclient.Text != "")
                    queryString = queryString + " and CHARINDEX( ISNULL(NULLIF('" + txtJobListclient.Text + "',''),CompanyName),CompanyName)>0 ";

                if (this.txtJobListAddress.Text != "")
                    queryString = queryString + " and JobList.Address Like'%" + txtJobListAddress.Text + "%'";

                if (txtTown.Text != "")
                    queryString = queryString + " and JobList.Borough like'" + txtTown.Text + "%'";

                if (txtJoblistClienttext.Text != "")
                    queryString = queryString + " and JobList.Clienttext like'" + txtJoblistClienttext.Text + "%'";

                if (this.txtJobListSearchDescription.Text != "")
                    queryString = queryString + " and JobList.Description like'%" + txtJobListSearchDescription.Text + "%'";

                //if (this.cbxJobListPM.SelectedItem.ToString() != "")
                //    queryString = queryString + " and Handler='" + cbxJobListPM.SelectedItem + "'";

                //if (cbxJobListPMrv.SelectedItem.ToString() != "")
                //    queryString = queryString + " and PMrv='" + cbxJobListPMrv.SelectedItem + "'";

                //if (cbxSearchTm.SelectedItem.ToString() != "")
                //    queryString = queryString + " and JobTracking.TaskHandler ='" + cbxSearchTm.SelectedItem + "'";

                if (chkShowOnlyPending.Checked == true)
                    queryString = queryString + "AND JobList.JobListID IN ( SELECT JobListID FROM JobTracking WHERE Status='Pending' AND (IsDelete=0 or IsDelete is null ) )";
                if (chkNotInvoiceJob.Checked == true)
                {
                    queryString = string.Format(queryString, " INNER JOIN JobTracking JT ON JobList.JobListId = JT.JobListId AND  (JT.IsDelete=0 or JT.IsDelete is null )");

                    queryString = queryString + " AND ((JT.BillState ='Not Invoiced' AND JT.Status <> 'Pending' AND JobList.TypicalInvoiceType='Item' AND (JobList.IsDisable IS NULL OR JobList.ISDisable=0) AND (JobList.IsInvoiceHold IS NULL OR JobList.IsInvoiceHold=0)) ";

                    queryString = queryString + " OR ((SELECT COUNT(*) FROM TS_Time WHERE JobListId=JobList.JobListId AND BillState='Not Invoice')> 0 AND JobList.TypicalInvoiceType='Time' AND (JobList.IsDisable IS NULL OR JobList.ISDisable=0) AND (JobList.IsInvoiceHold IS NULL OR JobList.IsInvoiceHold=0))";

                    queryString = queryString + " OR ((SELECT COUNT(*) FROM TS_Expences WHERE JobListId=JobList.JobListId AND BillState='Not Invoice')> 0 AND (JobList.IsDisable IS NULL OR JobList.ISDisable=0) AND (JobList.IsInvoiceHold IS NULL OR JobList.IsInvoiceHold=0)))";
                }
                else
                    queryString = string.Format(queryString, "");
                if (chkShowDisabled.Checked == false)
                    queryString = queryString + " AND (JobList.IsDisable = 0  OR JobList.IsDisable IS NULL)";

                if (chkInvoiceHold.Checked == true)
                    queryString = queryString + " AND (JobList.IsInvoiceHold = 1 )";
                else
                    queryString = queryString + " AND (JobList.IsInvoiceHold = 0 )";

                if (txtCommentsPreRequire.Text.Trim() != string.Empty)
                    queryString = queryString + " AND JobList.JobListID IN (SELECT JobListID FROM JobTracking WHERE Comments like '%" + txtCommentsPreRequire.Text.Trim() + "%' AND (IsDelete=0 or IsDelete is null ) )";
                if (cmbTMWithPending.Text.Trim() != "")
                    queryString = queryString + " AND JobList.JobListID IN (SELECT JobListID FROM JobTracking WHERE Status='Pending' AND TaskHandler =  '" + cmbTMWithPending.Text.Trim() + "' AND (IsDelete=0 or IsDelete is null ))";

                if (selectRecord_Joblist == true)
                    queryString = queryString + "AND JobList.JobListID IN (SELECT TOP 100 JobListID FROM JobList WHERE IsDelete=0 or IsDelete is null order by JobListID DESC )";

                string startDate; string endDate; int index = 0;

                if (chkYear.Checked == true)
                {
                    // index = cmbYear.SelectedIndex * -1
                    startDate = System.DateTime.Now.AddYears(index).Year.ToString() + "-01-01 00:00:00.000";
                    endDate = (System.DateTime.Now.AddYears(index + 1)).Year.ToString() + "-01-01 00:00:00.000";
                    // queryString = queryString & " AND  JobList.DateAdded > '" + startDate + "' AND  JobList.DateAdded < '" + endDate + "' "
                    queryString = queryString + " AND  YEAR(JobList.DateAdded) = " + cmbYear.Text;
                }
                queryString = queryString + "  order by JobList.JobListID  ";
                try
                {
                    // Attempt to load the dataset.
                    //dtJL = new Filldatatable(queryString);
                    var TempDataAfterFilter = repo.GetManagerDataAfterFilter(queryString);
                    dtJL = ToDataTable(TempDataAfterFilter);
                    grvJobList.DataSource = dtJL;
                }
                catch (Exception eLoad)
                {
                    // Add your error handling code here.
                    // Display error message, if any.
                    KryptonMessageBox.Show(eLoad.Message, "Manager");
                }
                // Grid Formatting
                {
                    var withBlock = grvJobList;
                    withBlock.Columns["JobListID"].Visible = false;
                    withBlock.Columns["JobNumber"].HeaderText = "Job#";
                    withBlock.Columns["JobNumber"].Width = 80;
                    withBlock.Columns["DateAdded"].Width = 1;
                    withBlock.Columns["DateAdded"].HeaderText = "Added";
                    withBlock.Columns["DateAdded"].Width = 80;
                    withBlock.Columns["Description"].HeaderText = "Description";
                    withBlock.Columns["Clienttext"].HeaderText = "Client Text";
                    withBlock.Columns["Clienttext"].Width = 180;
                    withBlock.Columns["Description"].Width = 200;
                    withBlock.Columns["Handler"].HeaderText = "PM";
                    withBlock.Columns["Handler"].Width = 40;
                    withBlock.Columns["Address"].Width = 150;
                    withBlock.Columns["CompanyID"].Width = 130;
                    withBlock.Columns["Borough"].Width = 90;
                    withBlock.Columns["Borough"].HeaderText = "Town";
                    withBlock.Columns["Contacts"].Width = 130;
                    // .Columns["Contacts").ReadOnly = True
                    withBlock.Columns["EmailAddress"].Width = 250;
                    withBlock.Columns["Handler"].Visible = false;
                    // .Columns["Borough").Visible = False
                    withBlock.Columns["CompanyID"].Visible = false;
                    withBlock.Columns["Contacts"].Visible = true;
                    withBlock.Columns["ContactsID"].Visible = false;
                    withBlock.Columns["CompanyName"].Visible = false;
                    withBlock.Columns["OwnerName"].HeaderText = "Owner Name";
                    withBlock.Columns["OwnerAddress"].HeaderText = "Owner Address";
                    withBlock.Columns["OwnerPhone"].HeaderText = "Owner Phone";
                    withBlock.Columns["OwnerFax"].HeaderText = "Owner Fax";
                    withBlock.Columns["ACContacts"].HeaderText = "AC Contacts";
                    withBlock.Columns["ACEmail"].HeaderText = "AC Email";
                    withBlock.Columns["CompanyNo"].Visible = false;
                    withBlock.Columns["PMrv"].HeaderText = "PMrv";
                    withBlock.Columns["PMrv"].Width = 40;
                    withBlock.Columns["PMrv"].Visible = false;

                    withBlock.Columns["IsDisable"].DisplayIndex = grvJobList.Columns.Count - 11;
                    withBlock.Columns["IsDisable"].HeaderText = "Disabled";
                    withBlock.Columns["IsDisable"].Width = 60;

                    withBlock.Columns["IsInvoiceHold"].DisplayIndex = grvJobList.Columns.Count - 10;
                    withBlock.Columns["IsInvoiceHold"].HeaderText = "Invoice Hold";
                    withBlock.Columns["IsInvoiceHold"].Width = 100;



                    withBlock.Columns["cmbInvoiceClient"].HeaderText = "InvoiceClient";
                    withBlock.Columns["cmbInvoiceClient"].DisplayIndex = grvJobList.Columns.Count - 9;
                    withBlock.Columns["InvoiceContact"].Width = 90;

                    withBlock.Columns["InvoiceContact"].Visible = false;
                    withBlock.Columns["InvoiceContactT"].HeaderText = "InvoiceContact";
                    withBlock.Columns["InvoiceContactT"].DisplayIndex = grvJobList.Columns.Count - 8;
                    withBlock.Columns["InvoiceEmailAddress"].Width = 90;
                    withBlock.Columns["InvoiceEmailAddress"].HeaderText = "InvoiceEmailAddress";
                    withBlock.Columns["InvoiceEmailAddress"].DisplayIndex = grvJobList.Columns.Count - 7;

                    withBlock.Columns["InvoiceACContacts"].Visible = false;
                    withBlock.Columns["InvoiceACContactsT"].Width = 90;
                    withBlock.Columns["InvoiceACContactsT"].HeaderText = "InvoiceACContacts";
                    withBlock.Columns["InvoiceACContactsT"].DisplayIndex = grvJobList.Columns.Count - 6;

                    withBlock.Columns["InvoiceACEmail"].Width = 90;
                    withBlock.Columns["InvoiceACEmail"].HeaderText = "InvoiceACEmail";
                    withBlock.Columns["InvoiceACEmail"].DisplayIndex = grvJobList.Columns.Count - 5;

                    withBlock.Columns["cmbTypicalInvoiceType"].DisplayIndex = grvJobList.Columns.Count - 4;
                    withBlock.Columns["TypicalInvoiceType"].HeaderText = "Invoice Type";
                    withBlock.Columns["TypicalInvoiceType"].Width = 100;
                    // Item rate column display index setup code will found it partial calss

                    withBlock.Columns["ServRate"].Width = 90;
                    withBlock.Columns["ServRate"].HeaderText = "Serv Rate";
                    withBlock.Columns["ServRate"].DisplayIndex = grvJobList.Columns.Count - 2;

                    withBlock.Columns["AdminInvoice"].Width = 100;
                    withBlock.Columns["AdminInvoice"].HeaderText = "Admin Inv.";
                    withBlock.Columns["AdminInvoice"].DisplayIndex = grvJobList.Columns.Count - 1;
                }

                JobListGridRateVersionColumn(ref grvJobList);

                //Need to do after My.Settings will apply Todo
                if (Properties.Settings.Default.PretimeSheetLoginUserType == "Admin" | Properties.Settings.Default.timeSheetLoginUserType == "Admin")
                {
                    UserType = "Admin";
                    grvJobList.Columns["IsDisable"].Visible = true;
                    grvJobList.Columns["IsInvoiceHold"].Visible = true;
                }
                else
                {
                    grvJobList.Columns["IsDisable"].Visible = false;
                    grvJobList.Columns["IsInvoiceHold"].Visible = false;
                }

                if (grvJobList.Rows.Count > 0)
                {
                    grvJobList.CurrentCell = grvJobList.Rows[grvJobList.Rows.Count - 1].Cells["Address"];
                    grvJobList.Rows[grvJobList.Rows.Count - 1].Selected = true;

                    selectedJobListID = Convert.ToInt32(grvJobList["JobListID", grvJobList.Rows.Count - 1].Value == DBNull.Value ? 0 : grvJobList["JobListID", grvJobList.Rows.Count - 1].Value);
                    isDisabled = Convert.ToBoolean(grvJobList["IsDisable", grvJobList.Rows.Count - 1].Value == DBNull.Value ? 0 : grvJobList["IsDisable", grvJobList.Rows.Count - 1].Value);
                    lblCompanyNo.Text = "Client No:- " + grvJobList.Rows[grvJobList.CurrentRow.Index].Cells["CompanyNo"].Value.ToString();
                }
                else
                {
                    selectedJobListID = 0;
                    isDisabled = false;
                }
                if (grvJobList.Rows.Count > 0)
                    grvJobList.CurrentCell = grvJobList.Rows[grvJobList.Rows.Count - 1].Cells["Address"];
                if (grvJobList.Rows.Count > 0)
                {
                    selectedJobListID = Convert.ToInt32(grvJobList["JobListID", grvJobList.Rows.Count - 1].Value == DBNull.Value ? 0 : grvJobList["JobListID", grvJobList.Rows.Count - 1].Value);
                    isDisabled = Convert.ToBoolean(grvJobList["IsDisable", grvJobList.Rows.Count - 1].Value == DBNull.Value ? 0 : grvJobList["IsDisable", grvJobList.Rows.Count - 1].Value);
                }
                if (selectRecord_Joblist == false)
                {
                    FillGridPreRequirment();
                    FillGridPermitRequiredInspection();
                    FillGridNotesCommunication();
                    if ((isDisabled))
                        disableJob(true);
                    else
                        disableJob(false);
                    //SetBadClient();
                    //if (grvJobList.Rows.Count > 0)
                    //ChangeDirJobNumber(grvJobList.Rows.Count - 1);
                    //ChangeTraficLight(grvJobList.Rows.Count - 1);
                }
                selectRecord_Joblist = false;
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "fillGridJobList", ex.Message);
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void disableJob(bool flag)
        {
            grvPreRequirments.ReadOnly = flag;
            btnInsertPreReq.Enabled = !flag;
            btnDeletePreReq.Enabled = !flag;
            btnCancelPreReq.Enabled = !flag;

            grvNotesCommunication.ReadOnly = flag;
            btnInsertNotes.Enabled = !flag;
            btndeleteNotes.Enabled = !flag;
            btnCancelNotes.Enabled = !flag;

            grvPermitsRequiredInspection.ReadOnly = flag;
            btnInsertPermit.Enabled = !flag;
            btnDeletePermit.Enabled = !flag;
            btnCancelPermit.Enabled = !flag;
        }

        private void JobListGridRateVersionColumn(ref DataGridView grd)
        {
            try
            {
                var withBlock = grd;
                if ((withBlock.Columns["cmbRateVersion"] == null))
                {
                    //DataAccessLayer DAL = new DataAccessLayer();
                    DataGridViewComboBoxColumn cmbVersionTable = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = cmbVersionTable;
                        // su
                        var TempTableVersion = repo.GetTableVersion();
                        //dt1 = new Filldatatable("select * from VersionTable  union SELECT 0 as TableVersionId, '--Use Default--' as TableVersionName order by TableVersionId");
                        DataTable dt1 = ToDataTable(TempTableVersion);
                        withBlock1.DataSource = dt1;
                        withBlock1.DisplayMember = "TableVersionName";
                        withBlock1.ValueMember = "TableVersionId";
                        withBlock1.DataPropertyName = "RateVersionId";
                        withBlock1.HeaderText = "Item Rate";
                        withBlock1.Width = 120;
                        withBlock1.Name = "cmbRateVersion";
                    }
                    withBlock.Columns.Add(cmbVersionTable);
                    withBlock.Columns["cmbRateVersion"].DisplayIndex = grvJobList.Columns.Count - 2;
                }
                else
                    withBlock.Columns["cmbRateVersion"].DisplayIndex = grvJobList.Columns.Count - 2;
                withBlock.Columns["RateVersionId"].Visible = false;
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "JobListGridRateVersionColumn", ex.Message);
            }
        }

        private void FillGridNotesCommunication()
        {
            try
            {
                string queryString = " And JobTracking.JobListID=" + selectedJobListID;

                if (cbxSearchTm.Text.Trim() != "")
                    queryString = queryString + " AND JobTracking.TaskHandler= '" + cbxSearchTm.Text.Trim() + "'";

                if (CmbPreRequireTrack.Text.Trim() != "")
                    queryString = queryString + " AND JobTracking.Track= '" + CmbPreRequireTrack.Text.Trim() + "'";
                if (cmbTrackSubPreRequire.Text.Trim() != "")
                    queryString = queryString + " AND JobTracking.TrackSub= '" + cmbTrackSubPreRequire.Text.Trim() + "'";
                if (cmbStatusPreRequire.Text.Trim() != "")
                    queryString = queryString + " AND JobTracking.Status= '" + cmbStatusPreRequire.Text.Trim() + "'";
                if (cmbBillStatePermit.Text.Trim() != "")
                    queryString = queryString + " AND JobTracking.BillState= '" + cmbBillStatePermit.Text.ToString().Trim() + "'";
                if (txtCommentsPreRequire.Text.ToString().Trim() != "")
                    queryString = queryString + " AND JobTracking.Comments like '%" + txtCommentsPreRequire.Text.Trim() + "%'";
                if (cmbTMWithPending.Text.Trim() != "")
                    queryString = queryString + " AND (JobTracking.Status='Pending' AND JobTracking.TaskHandler=  '" + cmbTMWithPending.Text.Trim() + "' )";
                queryString = queryString + " order by JobTrackingID";


                try
                {
                    // Attempt to load the dataset.
                    var TempNotesComunicationAfterFilter = repo.GetNotesComunicationDataAfterFilter(queryString);
                    dtNotes = ToDataTable(TempNotesComunicationAfterFilter);
                    grvNotesCommunication.DataSource = dtNotes;
                }

                catch (Exception eLoad)
                {
                    // Add your error handling code here.
                    // Display error message, if any.
                    KryptonMessageBox.Show(eLoad.Message, "Manager");
                }
                // Grid Formatting
                {
                    var withBlock = grvNotesCommunication;

                    // Set Column Property
                    withBlock.Columns["JobListID"].DataPropertyName = "JobListID";
                    withBlock.Columns["JobListID"].Visible = false;
                    withBlock.Columns["JobNumber"].HeaderText = "Job#";
                    withBlock.Columns["JobNumber"].Visible = false;
                    withBlock.Columns["Track"].Visible = false;
                    withBlock.Columns["AddDate"].Visible = true;
                    withBlock.Columns["AddDate"].Width = 90;
                    withBlock.Columns["AddDate"].HeaderText = "Added";
                    withBlock.Columns["NeedDate"].Visible = false;
                    withBlock.Columns["Obtained"].Visible = false;
                    withBlock.Columns["Obtained"].Width = 90;
                    withBlock.Columns["Expires"].Visible = false;
                    withBlock.Columns["Expires"].Width = 90;
                    withBlock.Columns["Status"].Visible = false;
                    withBlock.Columns["JobTrackingID"].Visible = false;
                    withBlock.Columns["TaskHandler"].HeaderText = "TM";
                    withBlock.Columns["TaskHandler"].Visible = false;
                    withBlock.Columns["Submitted"].Visible = false;
                    withBlock.Columns["BillState"].Visible = false;
                    withBlock.Columns["Comments"].HeaderText = "Comments";
                    withBlock.Columns["Comments"].Width = 520;
                    withBlock.Columns["InvOvr"].HeaderText = "Inv. Ovr.";
                    withBlock.Columns["InvOvr"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("en-US");
                    // .Columns["TrackSub").Visible = False
                    withBlock.Columns["TrackSub"].Width = 200;
                    withBlock.Columns["TrackSubID"].Visible = false;
                    withBlock.Columns["DeleteItemTimeService"].Visible = false;
                }

                btndeleteNotes.Enabled = true;
                btnInsertNotes.Text = "Insert";
                if (grvNotesCommunication.Rows.Count > 0)
                    grvNotesCommunication.CurrentCell = grvNotesCommunication.Rows[grvNotesCommunication.Rows.Count - 1].Cells["comments"];

                // Dim rows As IEnumerable(Of DataRow) = dtNotes.AsEnumerable()
                // Dim catchData As List(Of DataRow) = rows.Where(Function(d) d.Item("Status") = "Pending").ToList()
                int countRow = 0;
                foreach (DataRow dr in dtNotes.Rows)
                {
                    if (dr["Status"] == "Pending")
                        countRow = countRow + 1;
                }
                if (countRow > 0)
                    lblNotes.ForeColor = Color.Tomato;
                else
                    lblNotes.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "FillGridNotesCommunication", ex.Message);
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillGridPermitRequiredInspection()
        {
            try
            {
                // DataAccessLayer DAL = new DataAccessLayer();

                string queryString = "";

                // If Me.chkShowOnlyPendingTrack.Checked Then queryString = queryString & " and JobTracking.Status ='Pending'"
                if (cbxSearchTm.Text.Trim() != "")
                    queryString = queryString + " AND JobTracking.TaskHandler= '" + cbxSearchTm.Text.Trim() + "'";
                if (CmbPreRequireTrack.Text.ToString() != "")
                    queryString = queryString + " AND JobTracking.Track='" + CmbPreRequireTrack.Text.ToString() + "'";
                if (cmbTrackSubPreRequire.Text.ToString() != "")
                    queryString = queryString + " AND JobTracking.TrackSub='" + cmbTrackSubPreRequire.Text.ToString() + "'";
                if (cmbStatusPreRequire.Text.ToString() != "")
                    queryString = queryString + " AND JobTracking.Status='" + cmbStatusPreRequire.Text.ToString() + "'";
                if (txtCommentsPreRequire.Text.Trim() != "")
                    queryString = queryString + "AND JobTracking.Comments like '%" + txtCommentsPreRequire.Text.Trim() + "%'";
                if (cmbBillStatePermit.Text.ToString() != "")
                    queryString = queryString + " AND JobTracking.BillState='" + cmbBillStatePermit.Text.ToString() + "'";
                if (cmbTMWithPending.Text.Trim() != "")
                    queryString = queryString + " AND (JobTracking.Status='Pending' AND JobTracking.TaskHandler=  '" + cmbTMWithPending.Text.Trim() + "' )";
                queryString = queryString + " order by JobTrackingID";

                try
                {
                    // Attempt to load the dataset.
                    var TempPermitsRequiredAfterFilter = repo.GetPermitsRequirementDataAfterFilter(queryString, selectedJobListID);
                    dtPermit = ToDataTable(TempPermitsRequiredAfterFilter);
                    grvPermitsRequiredInspection.DataSource = dtPermit;
                }
                catch (Exception eLoad)
                {
                    // Add your error handling code here.
                    // Display error message, if any.
                    KryptonMessageBox.Show(eLoad.Message, "Manager");
                }
                // Grid Formatting
                {
                    var withBlock = grvPermitsRequiredInspection;

                    // Set Column Property
                    withBlock.Columns["JobListID"].DataPropertyName = "JobListID";
                    withBlock.Columns["JobListID"].Visible = false;
                    withBlock.Columns["JobNumber"].HeaderText = "Job#";
                    withBlock.Columns["JobNumber"].Visible = false;
                    withBlock.Columns["Track"].Visible = false;
                    withBlock.Columns["AddDate"].Visible = true;
                    withBlock.Columns["AddDate"].Width = 90;
                    withBlock.Columns["AddDate"].HeaderText = "Added";
                    withBlock.Columns["NeedDate"].Visible = false;
                    withBlock.Columns["Obtained"].Visible = true;
                    withBlock.Columns["Obtained"].Width = 90;
                    withBlock.Columns["Expires"].Visible = true;
                    withBlock.Columns["Expires"].Width = 90;
                    // .Columns["FinalAction").HeaderText = "Final Action"
                    // .Columns["FinalAction").Visible = True
                    // .Columns["FinalAction").Width = 80
                    withBlock.Columns["Status"].Visible = false;
                    withBlock.Columns["JobTrackingID"].Visible = false;
                    withBlock.Columns["TaskHandler"].HeaderText = "TM";
                    withBlock.Columns["TaskHandler"].Visible = false;
                    withBlock.Columns["Submitted"].Visible = true;

                    withBlock.Columns["BillState"].Visible = false;
                    withBlock.Columns["Comments"].HeaderText = "Comments";
                    withBlock.Columns["Comments"].Width = 330;
                    withBlock.Columns["InvOvr"].HeaderText = "Inv. Ovr.";
                    withBlock.Columns["InvOvr"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("en-US");
                    // .Columns["TrackSub").Visible = False
                    withBlock.Columns["TrackSub"].Width = 200;
                    withBlock.Columns["TrackSubID"].Visible = false;
                }
                btnDeletePermit.Enabled = true;
                btnInsertPermit.Text = "Insert";
                if (grvPermitsRequiredInspection.Rows.Count > 0)
                    grvPermitsRequiredInspection.CurrentCell = grvPermitsRequiredInspection.Rows[grvPermitsRequiredInspection.Rows.Count - 1].Cells["comments"];
                // Dim rows As IEnumerable(Of DataRow) = dtPermit.AsEnumerable()
                // Dim catchData As List(Of DataRow) = rows.Where(Function(d) d.Item("Status") = "Pending").ToList()

                int countRow = 0;
                foreach (DataRow dr in dtPermit.Rows)
                {
                    if (dr["Status"] == "Pending")
                        countRow = countRow + 1;
                }
                if (countRow > 0)
                    lblPermit.ForeColor = Color.Tomato;
                else
                    lblPermit.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "FillGridPermitRequiredInspection", ex.Message);
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pnlButtonVisible(Panel pnl, bool pnlreset)
        {
            foreach (Control ctrl in pnl.Controls)
            {
                if (ctrl is Button)
                {
                    if ((ctrl.Name != "btnAgingColor"))
                        ctrl.Visible = pnlreset;
                }
                if (ctrl is DataGridView)
                    ctrl.Visible = pnlreset;
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        private void SetColumns()
        {
            try
            {
                var TempColumn = repo.ManagerGridSetColumn();
                DataTable dtJL = new DataTable();
                dtJL = ToDataTable(TempColumn);
                grvJobList.DataSource = dtJL;

                cbxClient.Name = "Client#";
                cbxClient.Width = 200;

                grvJobList.Columns.Insert(1, cbxClient);

                DataTable dt = new DataTable();

                var TempCBlient = repo.GetcbxClient();
                dt = ToDataTable(TempCBlient);
                DataRow datarow = dt.NewRow();
                datarow["CompanyName"] = "";
                datarow["CompanyID"] = 0;
                dt.Rows.InsertAt(datarow, 0);
                cbxClient.DataSource = dt;
                cbxClient.DisplayMember = "CompanyName";
                cbxClient.ValueMember = "CompanyID";
                cbxClient.DataPropertyName = "CompanyID";
                DataTable _cmbIClientDT = dt.Copy();
                _cmbIClientDT.Rows[0]["CompanyName"] = "--Client--";
                _cmbIClientDT.Rows[0]["CompanyID"] = -99;
                cmbInvoiceClient = new DataGridViewComboBoxColumn() { DataSource = _cmbIClientDT, DisplayMember = "CompanyName", ValueMember = "CompanyID", DataPropertyName = "InvoiceClient", Name = "cmbInvoiceClient", HeaderText = "InvoiceClient" };
                grvJobList.Columns.Insert(grvJobList.Columns.Count - 1, cmbInvoiceClient);
                //EFDbContext cmbobj = new EFDbContext();
                {
                    var withBlock = grvJobList;

                    // Set Column Property
                    // With grvJobList

                    withBlock.Columns["JobListID"].Visible = false;
                    withBlock.Columns["JobNumber"].HeaderText = "Job#";
                    withBlock.Columns["JobNumber"].Width = 80;
                    withBlock.Columns["JobNumber"].DisplayIndex = 1;
                    withBlock.Columns["DateAdded"].Width = 80;
                    withBlock.Columns["DateAdded"].HeaderText = "Added";
                    withBlock.Columns["DateAdded"].DisplayIndex = 3;
                    withBlock.Columns["Description"].HeaderText = "Description";
                    withBlock.Columns["Description"].Width = 200;
                    withBlock.Columns["Description"].DisplayIndex = 4;
                    withBlock.Columns["Handler"].HeaderText = "PM";
                    withBlock.Columns["Handler"].Width = 40;
                    withBlock.Columns["Address"].Width = 150;
                    withBlock.Columns["Address"].DisplayIndex = 5;
                    withBlock.Columns["CompanyID"].Width = 130;
                    withBlock.Columns["Borough"].Width = 90;
                    withBlock.Columns["Borough"].HeaderText = "Town";
                    withBlock.Columns["Contacts"].Width = 130;
                    withBlock.Columns["Contacts"].DisplayIndex = 6;
                    withBlock.Columns["EmailAddress"].Width = 250;
                    withBlock.Columns["EmailAddress"].DisplayIndex = 10;
                    withBlock.Columns["Handler"].Visible = false;
                    withBlock.Columns["CompanyID"].Visible = false;
                    withBlock.Columns["Contacts"].Visible = true;
                    withBlock.Columns["ContactsID"].Visible = false;
                    withBlock.Columns["CompanyName"].Visible = false;
                    withBlock.Columns["OwnerName"].HeaderText = "Owner Name";
                    withBlock.Columns["OwnerAddress"].HeaderText = "Owner Address";
                    withBlock.Columns["OwnerPhone"].HeaderText = "Owner Phone";
                    withBlock.Columns["OwnerFax"].HeaderText = "Owner Fax";
                    withBlock.Columns["ACContacts"].HeaderText = "AC Contacts";
                    withBlock.Columns["ACEmail"].HeaderText = "AC Email";
                    withBlock.Columns["CompanyNo"].Visible = false;
                    withBlock.Columns["PMrv"].HeaderText = "PMrv";
                    withBlock.Columns["PMrv"].Width = 40;
                    withBlock.Columns["PMrv"].Visible = false;
                    withBlock.Columns["TypicalInvoiceType"].Width = 100;
                    withBlock.Columns["TypicalInvoiceType"].HeaderText = "Invoice Type";
                    withBlock.Columns["TypicalInvoiceType"].Visible = false;
                    withBlock.Columns["IsDisable"].DisplayIndex = grvJobList.Columns.Count - 3;
                    withBlock.Columns["IsDisable"].HeaderText = "Disabled";
                    withBlock.Columns["TypicalInvoiceType"].DisplayIndex = grvJobList.Columns.Count - 1;

                    withBlock.Columns["IsInvoiceHold"].DisplayIndex = grvJobList.Columns.Count - 1;
                    withBlock.Columns["IsInvoiceHold"].HeaderText = "Invoice Hold";
                    withBlock.Columns["InvoiceClient"].Visible = false;

                    DataGridViewComboBoxColumn colPM = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = colPM;
                        DataTable dtPM = new DataTable();

                        var TempColPM = repo.GetcolPMM();
                        dtPM = ToDataTable(TempColPM);

                        withBlock1.DataSource = dtPM;
                        withBlock1.DisplayMember = "cTrack";
                        withBlock1.DisplayIndex = 5;
                        withBlock1.HeaderText = "PM";
                        withBlock1.DataPropertyName = "Handler";
                        withBlock1.Width = 58;
                        withBlock1.Name = "cmbHandler";
                    }
                    withBlock.Columns.Add(colPM);


                    DataGridViewComboBoxColumn colPMrv = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = colPMrv;
                        //withBlock1.DataSource = cmbobj.FillDAtatableCombo("SELECT cTrack ,Id FROM MasterItem WHERE cGroup='PM' and (IsDelete=0 or IsDelete is null) ORDER BY cTrack ");
                        DataTable dtPM = new DataTable();
                        var TempColPM = repo.GetcolPMM();
                        dtPM = ToDataTable(TempColPM);
                        withBlock1.DataSource = dtPM;
                        withBlock1.DisplayMember = "cTrack";

                        withBlock1.HeaderText = "PMrv";
                        withBlock1.DataPropertyName = "PMrv";
                        withBlock1.Width = 58;
                        withBlock1.Name = "cmbPMrv";
                    }
                    withBlock.Columns.Add(colPMrv);

                    DataGridViewComboBoxColumn colTypicalInvoiceType = new DataGridViewComboBoxColumn();
                    colTypicalInvoiceType.Items.Add("Time");
                    colTypicalInvoiceType.Items.Add("Item");
                    {
                        var withBlock1 = colTypicalInvoiceType;
                        withBlock1.DataPropertyName = "TypicalInvoiceType";
                        withBlock1.HeaderText = "Invoice Type";
                        withBlock1.Width = 100;
                        withBlock1.Name = "cmbTypicalInvoiceType";
                    }
                    withBlock.Columns.Add(colTypicalInvoiceType);
                }

                if (grvJobList.Rows.Count > 0)
                {
                    selectedJobListID = Convert.ToInt32(grvJobList.Rows[0].Cells["JobListID"].Value);
                    isDisabled = Convert.ToBoolean(grvJobList.Rows[0].Cells["IsDisable"].Value);
                    grvJobList.Rows[0].Selected = true;
                }
            }

            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "SetColumns", ex.Message);
            }
        }

        private void SetColumnPreRequirment()
        {
            try
            {
                try
                {
                    // Attempt to load the dataset.
                    var TempPreColumn = repo.PreRequirementSetColumn();
                    dtPreReq = ToDataTable(TempPreColumn);
                    grvPreRequirments.DataSource = dtPreReq;
                }
                catch (Exception eLoad)
                {
                    // Add your error handling code here.
                    // Display error message, if any.
                    KryptonMessageBox.Show(eLoad.Message, "Manager");
                }

                {
                    var withBlock = grvPreRequirments;

                    // ComboTM'
                    DataGridViewComboBoxColumn colTM = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = colTM;
                        DataTable colTmDT = new DataTable();
                        var TempPreColTM = repo.GetPreRequirementcolTM_D();
                        colTmDT = ToDataTable(TempPreColTM);

                        //withBlock1.DataSource = cmbobj.FillDAtatableCombo("SELECT cTrack ,Id FROM MasterItem WHERE cGroup='TM' and (IsDelete=0 or IsDelete is null) AND (isDisable <> 1 or IsDisable is  null) ORDER BY cTrack ");
                        withBlock1.DataSource = colTmDT;
                        withBlock1.DisplayMember = "cTrack";
                        withBlock1.DisplayIndex = 2;
                        withBlock1.HeaderText = "TM";
                        withBlock1.DataPropertyName = "TaskHandler";
                        withBlock1.Width = 65;
                        withBlock1.Name = "cmbTaskHandler";
                    }

                    withBlock.Columns.Add(colTM);


                    DataGridViewComboBoxColumn colTrack = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = colTrack;
                        DataTable colTrackDT = new DataTable();
                        var TempPrecolTrack = repo.GetPreRequirementcolTrack();
                        colTrackDT = ToDataTable(TempPrecolTrack);
                        //withBlock1.DataSource = cmbobj.Filldatatable("select Trackname from MasterTrackSet where (IsDelete=0 or IsDelete is null) and TrackSet='PreRequirements'");
                        withBlock1.DataSource = colTrackDT;
                        withBlock1.DisplayMember = "Trackname";
                        withBlock1.DisplayIndex = 4;
                        withBlock1.HeaderText = "Track";
                        withBlock1.DataPropertyName = "Track";
                        withBlock1.Name = "cmbTrack";
                    }

                    withBlock.Columns.Add(colTrack);

                    DataGridViewComboBoxColumn colStatus = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = colStatus;
                        DataTable colStatusDT = new DataTable();
                        var TempPrecolStatusDT = repo.GetPreRequirementcolStatus();
                        colStatusDT = ToDataTable(TempPrecolStatusDT);

                        //withBlock1.DataSource = cmbobj.FillDAtatableCombo("SELECT cTrack ,Id FROM MasterItem WHERE cGroup='Status' and (IsDelete=0 or IsDelete is null) ORDER BY cTrack ");
                        withBlock1.DataSource = colStatusDT;
                        withBlock1.DisplayMember = "cTrack";
                        withBlock1.DisplayIndex = 9;
                        withBlock1.HeaderText = "Status";
                        withBlock1.DataPropertyName = "Status";
                        withBlock1.Name = "cmbStatus";
                    }
                    withBlock.Columns.Add(colStatus);
                }
                {
                    var withBlock = grvPreRequirments;

                    // Set Column Property
                    withBlock.Columns["JobListID"].DataPropertyName = "JobListID";
                    withBlock.Columns["JobListID"].Visible = false;
                    withBlock.Columns["JobNumber"].HeaderText = "Job#";
                    withBlock.Columns["JobNumber"].Visible = false;
                    withBlock.Columns["Track"].Visible = false;
                    withBlock.Columns["AddDate"].Width = 90;
                    withBlock.Columns["AddDate"].HeaderText = "Added";
                    withBlock.Columns["NeedDate"].Visible = false;
                    withBlock.Columns["Obtained"].Visible = true;
                    withBlock.Columns["Obtained"].Width = 90;
                    withBlock.Columns["Expires"].Visible = false;
                    withBlock.Columns["Expires"].Width = 90;
                    withBlock.Columns["Status"].Visible = false;
                    withBlock.Columns["JobTrackingID"].Visible = false;
                    withBlock.Columns["TaskHandler"].HeaderText = "TM";
                    withBlock.Columns["TaskHandler"].Visible = false;
                    withBlock.Columns["Submitted"].Visible = false;
                    withBlock.Columns["BillState"].Visible = false;
                    withBlock.Columns["Comments"].HeaderText = "Comments";
                    withBlock.Columns["Comments"].Width = 550;
                    withBlock.Columns["TrackSubID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "SetColumnPreRequirment", ex.Message);
            }
        }

        private void SetColumnPermit()
        {
            try
            {

                try
                {
                    // Attempt to load the dataset.
                    var TempColumnPermit = repo.PermitsRequirementSetColumn();
                    dtPermit = ToDataTable(TempColumnPermit);
                    grvPermitsRequiredInspection.DataSource = dtPermit;
                }
                catch (Exception eLoad)
                {
                    KryptonMessageBox.Show(eLoad.Message, "Manager");
                }
                // Grid Formatting
                {
                    var withBlock = grvPermitsRequiredInspection;
                    DataGridViewComboBoxColumn colTM = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = colTM;

                        DataTable colTmDT = new DataTable();
                        var TempPreColTM = repo.GetPreRequirementcolTM_D();
                        colTmDT = ToDataTable(TempPreColTM);

                        // withBlock1.DataSource = cmbobj.FillDAtatableCombo("SELECT cTrack ,Id FROM MasterItem WHERE cGroup='TM' and (IsDelete=0 or IsDelete is null) AND (isDisable <> 1 or IsDisable is  null) ORDER BY cTrack ");
                        withBlock1.DataSource = colTmDT;
                        withBlock1.DisplayMember = "cTrack";
                        withBlock1.DisplayIndex = 2;
                        withBlock1.HeaderText = "TM";
                        withBlock1.DataPropertyName = "TaskHandler";
                        withBlock1.Width = 58;
                        withBlock1.Name = "cmbTaskHandler";
                    }

                    withBlock.Columns.Add(colTM);



                    DataGridViewComboBoxColumn colTrack = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = colTrack;
                        DataTable colTrackDT = new DataTable();
                        var TempPremitcolTrack = repo.GetPermitsRequirementcolTrack();
                        colTrackDT = ToDataTable(TempPremitcolTrack);
                        // withBlock1.DataSource = cmbobj.Filldatatable("select Trackname from MasterTrackSet where (IsDelete=0 or IsDelete is null) and TrackSet='Permits/Required/Inspection'");
                        withBlock1.DataSource = colTrackDT;
                        withBlock1.DisplayMember = "Trackname";
                        withBlock1.DisplayIndex = 4;
                        withBlock1.HeaderText = "Track";
                        withBlock1.DataPropertyName = "Track";
                        withBlock1.Name = "cmbTrack";
                    }
                    withBlock.Columns.Add(colTrack);

                    DataGridViewComboBoxColumn colStatus = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = colStatus;
                        DataTable colStatusDT = new DataTable();
                        var TempPremitcolStatusDT = repo.GetPreRequirementcolStatus();
                        colStatusDT = ToDataTable(TempPremitcolStatusDT);
                        withBlock1.DataSource = colStatusDT;
                        //withBlock1.DataSource = cmbobj.FillDAtatableCombo("SELECT cTrack ,Id FROM MasterItem WHERE cGroup='Status' and (IsDelete=0 or IsDelete is null) ORDER BY cTrack ");
                        withBlock1.DisplayMember = "cTrack";
                        withBlock1.DisplayIndex = 9;
                        withBlock1.HeaderText = "Status";
                        withBlock1.DataPropertyName = "Status";
                        withBlock1.Name = "cmbStatus";
                    }
                    withBlock.Columns.Add(colStatus);


                    DataGridViewComboBoxColumn colFinalAction = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = colFinalAction;
                        withBlock1.Items.Add("No Action");
                        withBlock1.Items.Add("Renewed");
                        withBlock1.Items.Add("Not Req'd");
                        withBlock1.HeaderText = "FinalAction";
                        withBlock1.Width = 80;
                        withBlock1.DisplayIndex = 11;
                        withBlock1.DataPropertyName = "FinalAction";
                        withBlock1.Name = "cmbFinalAction";
                    }
                    withBlock.Columns.Add(colFinalAction);


                    DataGridViewComboBoxColumn colBillStatus = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = colBillStatus;
                        DataTable colBillStatusDT = new DataTable();
                        var TempPremitcolBillStatusDT = repo.GetPermitsRequirementcolBillStatus();
                        colBillStatusDT = ToDataTable(TempPremitcolBillStatusDT);
                        withBlock1.DataSource = colBillStatusDT;
                        //withBlock1.DataSource = cmbobj.FillDAtatableCombo("SELECT cTrack ,Id FROM MasterItem WHERE cGroup='Bill State' and (IsDelete=0 or IsDelete is null) ORDER BY cTrack ");
                        withBlock1.DisplayMember = "cTrack";
                        withBlock1.DisplayIndex = 13;
                        withBlock1.HeaderText = "Bill State";
                        withBlock1.DataPropertyName = "BillState";
                        withBlock1.Name = "cmbBillState";
                    }

                    withBlock.Columns.Add(colBillStatus);
                }
                {
                    var withBlock = grvPermitsRequiredInspection;

                    // Set Column Property
                    withBlock.Columns["JobListID"].DataPropertyName = "JobListID";
                    withBlock.Columns["JobListID"].Visible = false;
                    withBlock.Columns["JobNumber"].HeaderText = "Job#";
                    withBlock.Columns["JobNumber"].Visible = false;
                    withBlock.Columns["Track"].Visible = false;
                    withBlock.Columns["AddDate"].Visible = true;
                    withBlock.Columns["AddDate"].Width = 90;
                    withBlock.Columns["AddDate"].HeaderText = "Added";
                    withBlock.Columns["NeedDate"].Visible = false;
                    withBlock.Columns["Obtained"].Visible = true;
                    withBlock.Columns["Obtained"].Width = 90;
                    withBlock.Columns["Expires"].Visible = true;
                    withBlock.Columns["Expires"].Width = 90;
                    withBlock.Columns["FinalAction"].Visible = false;
                    withBlock.Columns["FinalAction"].Width = 80;
                    withBlock.Columns["Status"].Visible = false;
                    withBlock.Columns["JobTrackingID"].Visible = false;
                    withBlock.Columns["TaskHandler"].HeaderText = "TM";
                    withBlock.Columns["TaskHandler"].Visible = false;
                    withBlock.Columns["Submitted"].Visible = true;
                    withBlock.Columns["BillState"].Visible = false;
                    withBlock.Columns["Comments"].HeaderText = "Comments";
                    withBlock.Columns["Comments"].Width = 360;
                    withBlock.Columns["TrackSub"].Width = 200;
                    withBlock.Columns["TrackSubID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "SetColumnPermit", ex.Message);
            }
        }

        private void SetColumnNotes()
        {
            //DataAccessLayer cmbobj = new DataAccessLayer();
            try
            {

                try
                {
                    // Attempt to load the dataset.
                    var TempColumnNotes = repo.NotesSetColumn();
                    dtNotes = ToDataTable(TempColumnNotes);

                    grvNotesCommunication.DataSource = dtNotes;
                }
                catch (Exception eLoad)
                {
                    // Add your error handling code here.0
                    // Display error message, if any.
                    KryptonMessageBox.Show(eLoad.Message, "Manager");
                }
                // Grid Formatting
                {
                    var withBlock = grvNotesCommunication;
                    DataGridViewComboBoxColumn colTM = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = colTM;
                        DataTable colTMDT = new DataTable();
                        var TempNotescolTM = repo.GetPreRequirementcolTM_D();
                        colTMDT = ToDataTable(TempNotescolTM);
                        withBlock1.DataSource = colTMDT;
                        //withBlock1.DataSource = cmbobj.FillDAtatableCombo("SELECT cTrack ,Id FROM MasterItem WHERE cGroup='TM' and (IsDelete=0 or IsDelete is null) AND (isDisable <> 1 or IsDisable is  null) ORDER BY cTrack ");
                        withBlock1.DisplayMember = "cTrack";
                        withBlock1.DisplayIndex = 2;
                        withBlock1.HeaderText = "TM";
                        withBlock1.DataPropertyName = "TaskHandler";
                        withBlock1.Width = 58;
                        withBlock1.Name = "cmbTaskHandler";
                    }

                    withBlock.Columns.Add(colTM);

                    DataGridViewComboBoxColumn colTrack = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = colTrack;
                        DataTable colTrackDT = new DataTable();
                        var TempNotescolTrack = repo.GetNotescolTrack();
                        colTrackDT = ToDataTable(TempNotescolTrack);
                        withBlock1.DataSource = colTrackDT;
                        //withBlock1.DataSource = cmbobj.Filldatatable("select Trackname from MasterTrackSet where (IsDelete=0 or IsDelete is null) and TrackSet='Notes/Communication'");
                        withBlock1.DisplayMember = "Trackname";
                        withBlock1.DisplayIndex = 4;
                        withBlock1.HeaderText = "Track";
                        withBlock1.DataPropertyName = "Track";
                        withBlock1.Name = "cmbTrack";
                    }

                    withBlock.Columns.Add(colTrack);

                    DataGridViewComboBoxColumn colStatus = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = colStatus;
                        DataTable colStatusDT = new DataTable();
                        var TempNotescolStatusDT = repo.GetPreRequirementcolStatus();
                        colStatusDT = ToDataTable(TempNotescolStatusDT);
                        withBlock1.DataSource = colStatusDT;

                        //withBlock1.DataSource = cmbobj.FillDAtatableCombo("SELECT cTrack ,Id FROM MasterItem WHERE cGroup='Status' and (IsDelete=0 or IsDelete is null) ORDER BY cTrack ");
                        withBlock1.DisplayMember = "cTrack";
                        withBlock1.DisplayIndex = 9;
                        withBlock1.HeaderText = "Status";
                        withBlock1.DataPropertyName = "Status";
                        withBlock1.Name = "cmbStatus";
                    }
                    withBlock.Columns.Add(colStatus);

                    DataGridViewComboBoxColumn colBillStatus = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = colBillStatus;
                        DataTable colBillStatusDT = new DataTable();
                        var NotescolBillStatusDT = repo.GetPermitsRequirementcolBillStatus();
                        colBillStatusDT = ToDataTable(NotescolBillStatusDT);
                        withBlock1.DataSource = colBillStatusDT;
                        //withBlock1.DataSource = cmbobj.FillDAtatableCombo("SELECT cTrack ,Id FROM MasterItem WHERE cGroup='Bill State' and (IsDelete=0 or IsDelete is null) ORDER BY cTrack ");
                        withBlock1.DisplayMember = "cTrack";
                        withBlock1.DisplayIndex = 13;
                        withBlock1.HeaderText = "Bill State";
                        withBlock1.DataPropertyName = "BillState";
                        withBlock1.Name = "cmbBillState";
                    }
                    withBlock.Columns.Add(colBillStatus);

                    withBlock.Columns["JobListID"].DataPropertyName = "JobListID";
                    withBlock.Columns["JobListID"].Visible = false;
                    withBlock.Columns["JobNumber"].HeaderText = "Job#";
                    withBlock.Columns["JobNumber"].Visible = false;
                    withBlock.Columns["Track"].Visible = false;
                    withBlock.Columns["AddDate"].Visible = true;
                    withBlock.Columns["AddDate"].Width = 90;
                    withBlock.Columns["AddDate"].HeaderText = "Added";
                    withBlock.Columns["NeedDate"].Visible = false;
                    withBlock.Columns["Obtained"].Visible = false;
                    withBlock.Columns["Obtained"].Width = 90;
                    withBlock.Columns["Expires"].Visible = false;
                    withBlock.Columns["Expires"].Width = 90;
                    withBlock.Columns["Status"].Visible = false;
                    withBlock.Columns["JobTrackingID"].Visible = false;
                    withBlock.Columns["TaskHandler"].HeaderText = "TM";
                    withBlock.Columns["TaskHandler"].Visible = false;
                    withBlock.Columns["Submitted"].Visible = false;
                    withBlock.Columns["BillState"].Visible = false;
                    withBlock.Columns["Comments"].HeaderText = "Comments";
                    withBlock.Columns["Comments"].Width = 550;
                    withBlock.Columns["TrackSub"].Width = 200;
                }
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "SetColumnNotes", ex.Message);
            }
        }

        private void FillGridPreRequirment()
        {
            try
            {
                string queryString = "";

                // If Me.chkShowOnlyPendingTrack.Checked Then queryString = queryString & " and JobTracking.Status ='Pending'"
                //TODO
                ////if (cbxSearchTm.Text.Trim() == "")
                ////    queryString = queryString + " AND JobTracking.TaskHandler= '" + cbxSearchTm.Text.Trim() + "'";
                if (CmbPreRequireTrack.Text.ToString() != "")
                    queryString = queryString + " AND JobTracking.Track='" + CmbPreRequireTrack.Text.ToString() + "'";
                if (cmbTrackSubPreRequire.Text.ToString() != "")
                    queryString = queryString + " AND JobTracking.TrackSub='" + cmbTrackSubPreRequire.Text.ToString() + "'";
                if (cmbStatusPreRequire.Text.ToString() != "")
                    queryString = queryString + " AND JobTracking.Status='" + cmbStatusPreRequire.Text.ToString() + "'";
                if (txtCommentsPreRequire.Text.Trim() != "")
                    queryString = queryString + "AND JobTracking.Comments like '%" + txtCommentsPreRequire.Text.Trim() + "%'";
                if (cmbBillStatePermit.Text.Trim() != "")
                    queryString = queryString + " AND JobTracking.BillState= '" + cmbBillStatePermit.Text.Trim() + "'";
                if (cmbTMWithPending.Text.Trim() != "")
                    queryString = queryString + " AND (JobTracking.Status='Pending' AND JobTracking.TaskHandler=  '" + cmbTMWithPending.Text.Trim() + "' )";
                queryString = queryString + " order by JobTrackingID";

                try
                {
                    // Attempt to load the dataset.
                    var TempPreRequirementDataAfter = repo.GetPreRequirementDataAfterFilter(queryString, selectedJobListID);
                    dtPreReq = ToDataTable(TempPreRequirementDataAfter);
                    grvPreRequirments.DataSource = dtPreReq;
                }
                catch (Exception eLoad)
                {
                    // Add your error handling code here.
                    // Display error message, if any.
                    KryptonMessageBox.Show(eLoad.Message, "Manager");
                }

                // Grid Formatting
                {
                    var withBlock = grvPreRequirments;

                    // Set Column Property
                    withBlock.Columns["JobListID"].DataPropertyName = "JobListID";
                    withBlock.Columns["JobListID"].Visible = false;
                    withBlock.Columns["JobNumber"].HeaderText = "Job#";
                    withBlock.Columns["JobNumber"].Visible = false;
                    withBlock.Columns["Track"].Visible = false;
                    withBlock.Columns["AddDate"].Width = 90;
                    withBlock.Columns["AddDate"].HeaderText = "Added";
                    withBlock.Columns["NeedDate"].Visible = false;
                    withBlock.Columns["Obtained"].Visible = true;
                    withBlock.Columns["Obtained"].Width = 90;
                    withBlock.Columns["Expires"].Visible = false;
                    withBlock.Columns["Expires"].Width = 90;
                    withBlock.Columns["Status"].Visible = false;
                    withBlock.Columns["JobTrackingID"].Visible = false;
                    withBlock.Columns["TaskHandler"].HeaderText = "TM";
                    withBlock.Columns["TaskHandler"].Visible = false;
                    withBlock.Columns["Submitted"].Visible = false;
                    withBlock.Columns["BillState"].Visible = false;
                    withBlock.Columns["Comments"].HeaderText = "Comments";
                    withBlock.Columns["Comments"].Width = 522;
                    withBlock.Columns["InvOvr"].HeaderText = "Inv. Ovr.";
                    withBlock.Columns["InvOvr"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("en-US");
                    withBlock.Columns["TrackSub"].Width = 200;
                    withBlock.Columns["TrackSubID"].Visible = false;
                }

                btnDeletePreReq.Enabled = true;
                btnInsertPreReq.Text = "Insert";
                if (grvPreRequirments.Rows.Count > 0)
                    grvPreRequirments.CurrentCell = grvPreRequirments.Rows[grvPreRequirments.Rows.Count - 1].Cells["comments"];

                // Dim rows As IEnumerable(Of DataRow) = dtPreReq.AsEnumerable()
                // Dim catchData As List(Of DataRow) = rows.Where(Function(d) d.Item("Status") = "Pending").ToList()
                int countRow = 0;
                foreach (DataRow dr in dtPreReq.Rows)
                {
                    if (dr["Status"] == "Pending")
                        countRow = countRow + 1;
                }
                if (countRow > 0)
                    lblPreRequirment.ForeColor = Color.Tomato;
                else
                    lblPreRequirment.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "FillGridPreRequirment", ex.Message);
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected void InsertJobList()
        {
            // grvJobList.Rows[0].Cells["JobNumber").Selected = True 'move this line to below until input validate
            grvJobList.EndEdit();
            //DAL = new EFDbContext();
            try
            {
                if (grvJobList.Rows[grvJobList.Rows.Count - 1].Cells["JobNumber"].FormattedValue == "")
                {
                    KryptonMessageBox.Show("Please enter Job Number ", "Manager");
                    grvJobList.CurrentCell = grvJobList.Rows[grvJobList.Rows.Count - 1].Cells["JobNumber"];
                    return;
                }
                else
                    foreach (DataGridViewRow row in grvJobList.Rows)
                    {
                        if (grvJobList.Rows.Count - 1 != row.Index)
                        {
                            if (grvJobList.Rows[row.Index].Cells["JobNumber"].EditedFormattedValue == grvJobList.Rows[grvJobList.Rows.Count - 1].Cells["JobNumber"].EditedFormattedValue)
                            {
                                if (KryptonMessageBox.Show("Entered Job Number already exist for this Client:-" + grvJobList.Rows[row.Index].Cells["Client#"].EditedFormattedValue.ToString() + " ! you want to continue.", "Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
                                {
                                    grvJobList.CurrentCell = grvJobList.Rows[grvJobList.Rows.Count - 1].Cells["JobNumber"];
                                    return;
                                }
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
            }
            try
            {
                btnDelete.Enabled = true;
                int cnt = grvJobList.Rows.Count - 1;
                // cmd = New SqlCommand(" Insert into JobList(JobNumber,Client,DateAdded,Description,Handler,Address,Borough,Contacts,ContactsEmails) values (@JobNumber,@Client,@DateAdded,@Description,@Handler,@Address,@Borough,@Contacts,@ContactsEmails)", sqlcon)
                if ((!ValidateRateServTypeValue(cnt)))
                    return;
                grvJobList.Rows[0].Cells["JobNumber"].Selected = true;
                SqlCommand cmd = new SqlCommand();
                if ((AutoJB.ToString() != grvJobList.Rows[cnt].Cells["JobNumber"].Value.ToString()))
                    cmd.CommandText = "Insert into JobList (JobNumber, CompanyID, ContactsID, DateAdded, Description, Handler, Address, Borough,InvoiceClient ,InvoiceContact, InvoiceEmailAddress, InvoiceACContacts, InvoiceACEmail,IsNewRecord,OwnerName,OwnerAddress,OwnerPhone,OwnerFax,ACContacts,ACEmail,Clienttext,ContactsEmails, PMrv,RateVersionId,ServRate,AdminInvoice, IsInvoiceHold) values (@JobNumber, @CompanyID, @ContactsID, @DateAdded, @Description, @Handler, @Address, @Borough,@InvoiceClient ,@InvoiceContact,@InvoiceEmailAddress,@InvoiceACContacts, @InvoiceACEmail, @IsNewRecord, @OwnerName, @OwnerAddress,@OwnerPhone,@OwnerFax,@ACContacts,@ACEmail,@Clienttext,@ContactsEmails, @PMrv,@RateVersionId,@ServRate,@AdminInvoice, @IsInvoiceHold)";
                else

                    cmd.CommandText = "Insert into JobList (JobNumber, CompanyID, ContactsID, DateAdded, Description, Handler, Address, Borough,InvoiceClient ,InvoiceContact, InvoiceEmailAddress, InvoiceACContacts, InvoiceACEmail,IsNewRecord,OwnerName,OwnerAddress,OwnerPhone,OwnerFax,ACContacts,ACEmail,Clienttext,ContactsEmails, PMrv, RateVersionId,ServRate,AdminInvoice, IsInvoiceHold) values (@JobNumber,@CompanyID,@ContactsID,@DateAdded,@Description,@Handler,@Address,@Borough,@InvoiceClient ,@InvoiceContact,@InvoiceEmailAddress,@InvoiceACContacts, @InvoiceACEmail,@IsNewRecord,@OwnerName,@OwnerAddress,@OwnerPhone,@OwnerFax,@ACContacts,@ACEmail,@Clienttext,@ContactsEmails, @PMrv, @RateVersionId,@ServRate,@AdminInvoice, @IsInvoiceHold)";


                List<SqlParameter> Param = new List<SqlParameter>();
                Param.Add(new SqlParameter("@IsNewRecord", 1));
                Param.Add(new SqlParameter("@JobListID", grvJobList.Rows[cnt].Cells["JobListID"].Value.ToString()));

                if ((AutoJB.ToString() != grvJobList.Rows[cnt].Cells["JobNumber"].Value.ToString()))
                    Param.Add(new SqlParameter("@JobNumber", grvJobList.Rows[cnt].Cells["JobNumber"].Value.ToString()));
                else

                    Param.Add(new SqlParameter("@JobNumber", AutoJB.ToString()));
                Param.Add(new SqlParameter("@DateAdded", grvJobList.Rows[cnt].Cells["DateAdded"].Value.ToString()));
                Param.Add(new SqlParameter("@Description", grvJobList.Rows[cnt].Cells["Description"].Value.ToString()));
                Param.Add(new SqlParameter("@Address", grvJobList.Rows[cnt].Cells["Address"].Value.ToString()));
                Param.Add(new SqlParameter("@Handler", grvJobList.Rows[cnt].Cells["cmbHandler"].Value.ToString()));
                Param.Add(new SqlParameter("@Borough", grvJobList.Rows[cnt].Cells["Borough"].Value.ToString()));
                Param.Add(new SqlParameter("@OwnerName", grvJobList.Rows[cnt].Cells["OwnerName"].Value.ToString()));
                Param.Add(new SqlParameter("@OwnerAddress", grvJobList.Rows[cnt].Cells["OwnerAddress"].Value.ToString()));
                Param.Add(new SqlParameter("@OwnerPhone", grvJobList.Rows[cnt].Cells["OwnerPhone"].Value.ToString()));
                Param.Add(new SqlParameter("@OwnerFax", grvJobList.Rows[cnt].Cells["OwnerFax"].Value.ToString()));
                Param.Add(new SqlParameter("@ACContacts", grvJobList.Rows[cnt].Cells["ACContacts"].Value.ToString()));
                Param.Add(new SqlParameter("@ACEmail", grvJobList.Rows[cnt].Cells["ACEmail"].Value.ToString()));
                Param.Add(new SqlParameter("@Clienttext", grvJobList.Rows[cnt].Cells["Clienttext"].Value.ToString()));
                Param.Add(new SqlParameter("@ContactsEmails", grvJobList.Rows[cnt].Cells["EmailAddress"].Value.ToString()));
                Param.Add(new SqlParameter("@PMrv", grvJobList.Rows[cnt].Cells["cmbPMrv"].Value.ToString()));
                Param.Add(new SqlParameter("@IsInvoiceHold", grvJobList.Rows[cnt].Cells["IsInvoiceHold"].Value));
                Param.Add(new SqlParameter("@InvoiceClient", grvJobList.Rows[cnt].Cells["InvoiceClient"].Value.ToString()));
                Param.Add(new SqlParameter("@InvoiceContact", grvJobList.Rows[cnt].Cells["InvoiceContact"].Value.ToString()));
                Param.Add(new SqlParameter("@InvoiceEmailAddress", grvJobList.Rows[cnt].Cells["InvoiceEmailAddress"].Value.ToString()));
                Param.Add(new SqlParameter("@InvoiceACContacts", grvJobList.Rows[cnt].Cells["InvoiceACContacts"].Value.ToString()));
                Param.Add(new SqlParameter("@InvoiceACEmail", grvJobList.Rows[cnt].Cells["InvoiceACEmail"].Value.ToString()));
                Param.Add(new SqlParameter("@RateVersionId", grvJobList.Rows[cnt].Cells["RateVersionId"].Value));
                Param.Add(new SqlParameter("@ServRate", grvJobList.Rows[cnt].Cells["ServRate"].Value));
                Param.Add(new SqlParameter("@AdminInvoice", grvJobList.Rows[cnt].Cells["AdminInvoice"].Value));
                if (grvJobList.Rows[cnt].Cells["Client#"].Value.ToString() == "")
                    Param.Add(new SqlParameter("@CompanyID", 0));
                else
                    Param.Add(new SqlParameter("@CompanyID", grvJobList.Rows[cnt].Cells["Client#"].Value));

                int ContactsID;
                string values = Convert.ToString(grvJobList.Rows[cnt].Cells["ContactsID"].Value);
                if (String.IsNullOrEmpty(values))
                {
                    Param.Add(new SqlParameter("@ContactsID", "0"));
                    ContactsID = 0;
                }
                else
                {
                    ContactsID = Convert.ToInt32(grvJobList.Rows[cnt].Cells["ContactsID"].Value.ToString());
                    Param.Add(new SqlParameter("@ContactsID", grvJobList.Rows[cnt].Cells["ContactsID"].Value.ToString()));
                }
                //(new db.Database.ExecuteSqlCommand(cmd.CommandText, Param) > 0)
                int num = repo.db.Database.ExecuteSqlCommand(cmd.CommandText, Param.ToArray());

                if (num > 0)
                {
                    // System.Windows.Forms.MessageBox.Show("Record Saved!", "Message")
                    fillGridJobList();
                    grvJobList.Rows[grvJobList.Rows.Count - 1].Selected = true;
                    grvJobList.CurrentCell = grvJobList.Rows[grvJobList.Rows.Count - 1].Cells["JobNumber"];
                    btnAdd.Text = "Insert";
                    //new LoginActivityInfo("Insert", this.Text);
                }
            }
            catch (Exception ex)
            {
                cErrorLog.WriteLog("JobStatus", "InsertJobList", ex.Message);
                KryptonMessageBox.Show(ex.Message, "Manager");
            }
        }

        private bool ValidateRateServTypeValue(int rowindex)
        {
            bool conditionValid = true;
            if ((string.IsNullOrEmpty(grvJobList.Rows[rowindex].Cells["RateVersionId"].Value.ToString()) | grvJobList.Rows[rowindex].Cells["RateVersionId"].Value.ToString() == "0"))
            {
                conditionValid = false;
                MessageBox.Show("Please Select Item Rate", "Manager", MessageBoxButtons.OK);
            }
            else if ((string.IsNullOrEmpty(grvJobList.Rows[rowindex].Cells["ServRate"].Value.ToString())))
            {
                conditionValid = false;
                MessageBox.Show("Please Select Service Rate", "Manager", MessageBoxButtons.OK);
            }
            else if ((string.IsNullOrEmpty(grvJobList.Rows[rowindex].Cells["TypicalInvoiceType"].Value.ToString())))
            {
                conditionValid = false;
                MessageBox.Show("Please Select Invoice Type", "Manager", MessageBoxButtons.OK);
            }
            return conditionValid;
        }

        protected void InsertPreReq()
        {
            grvPreRequirments.Rows[0].Cells["comments"].Selected = true;
            grvPreRequirments.EndEdit();
            if (string.IsNullOrEmpty(grvPreRequirments.Rows[grvPreRequirments.Rows.Count - 1].Cells["Track"].Value.ToString()))
            {
                KryptonMessageBox.Show("Track field are compulsory", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(grvPreRequirments.Rows[grvPreRequirments.Rows.Count - 1].Cells["TrackSub"].Value.ToString()))
            {
                KryptonMessageBox.Show("TrackSub field are compulsory", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                btnDeletePreReq.Enabled = true;
                int cnt = grvPreRequirments.Rows.Count - 1;
                //DataAccessLayer DAL = new DataAccessLayer();

                SqlCommand cmd = new SqlCommand("Insert into Jobtracking(JobListID,Track,AddDate,NeedDate,Obtained,Expires,Status,Submitted,BillState,TaskHandler,TrackSub,Comments,IsNewRecord,TrackSubID,InvOvr) values (@JobListID,@Track,@AddDate,@NeedDate,@Obtained,@Expires,@Status,@Submitted,@BillState,@TaskHandler,@TrackSub,@Comments,@IsNewRecord,@TrackSubID,@InvOvr)");
                List<SqlParameter> Param = new List<SqlParameter>();
                Param.Add(new SqlParameter("@IsNewRecord", 1));
                Param.Add(new SqlParameter("@JobListID", selectedJobListID));
                Param.Add(new SqlParameter("@TaskHandler", grvPreRequirments.Rows[cnt].Cells["cmbTaskHandler"].Value.ToString()));
                Param.Add(new SqlParameter("@Track", grvPreRequirments.Rows[cnt].Cells["cmbTrack"].Value.ToString()));
                Param.Add(new SqlParameter("@Submitted", grvPreRequirments.Rows[cnt].Cells["Submitted"].Value.ToString()));
                Param.Add(new SqlParameter("@BillState", grvPreRequirments.Rows[cnt].Cells["BillState"].Value.ToString()));
                Param.Add(new SqlParameter("@TrackSub", grvPreRequirments.Rows[cnt].Cells["TrackSub"].Value.ToString()));
                Param.Add(new SqlParameter("@Comments", grvPreRequirments.Rows[cnt].Cells["Comments"].Value.ToString()));
                Param.Add(new SqlParameter("@Status", grvPreRequirments.Rows[cnt].Cells["cmbStatus"].Value.ToString()));
                Param.Add(new SqlParameter("@Obtained", grvPreRequirments.Rows[cnt].Cells["Obtained"].Value.ToString()));
                Param.Add(new SqlParameter("@Expires", grvPreRequirments.Rows[cnt].Cells["Expires"].Value.ToString()));
                Param.Add(new SqlParameter("@AddDate", grvPreRequirments.Rows[cnt].Cells["AddDate"].Value.ToString()));
                Param.Add(new SqlParameter("@NeedDate", grvPreRequirments.Rows[cnt].Cells["NeedDate"].Value.ToString()));
                Param.Add(new SqlParameter("@TrackSubID", grvPreRequirments.Rows[cnt].Cells["TrackSubID"].Value.ToString()));
                Param.Add(new SqlParameter("@InvOvr", grvPreRequirments.Rows[cnt].Cells["InvOvr"].Value.ToString()));

                //if (new Database.ParameterSqlExcecuteQuery(cmd, Param) > 0)
                repo.Insert();
                int num = repo.db.Database.ExecuteSqlCommand(cmd.CommandText, Param.ToArray());
                if (num > 0)
                {
                    //System.Windows.Forms.MessageBox.Show("Record Saved!", "Message")
                    //////new Database.LoginActivityInfo("Insert", this.Text);
                    FillGridPreRequirment();
                    if (grvPreRequirments.Rows.Count > 0)
                    {
                        grvPreRequirments.Rows[grvPreRequirments.Rows.Count - 1].Selected = true;
                        grvPreRequirments.CurrentCell = grvPreRequirments.Rows[grvPreRequirments.Rows.Count - 1].Cells["comments"];
                    }

                    btnInsertPreReq.Text = "Insert";
                }

            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Manager");
            }
        }

        protected void InsertPermits()
        {
            //grvPermitsRequiredInspection.Rows[0].Cells["comments").Selected = True
            grvPermitsRequiredInspection.EndEdit();
            if (string.IsNullOrEmpty(grvPermitsRequiredInspection.Rows[grvPermitsRequiredInspection.Rows.Count - 1].Cells["Track"].Value.ToString()))
            {
                KryptonMessageBox.Show("Track field are compulsory", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(grvPermitsRequiredInspection.Rows[grvPermitsRequiredInspection.Rows.Count - 1].Cells["TrackSub"].Value.ToString()))
            {
                KryptonMessageBox.Show("TrackSub field are compulsory", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                btnDeletePermit.Enabled = true;
                int cnt = grvPermitsRequiredInspection.Rows.Count - 1;
                //DataAccessLayer DAL = new DataAccessLayer();
                SqlCommand cmd = new SqlCommand("Insert into Jobtracking(JobListID,Track,AddDate,NeedDate,Obtained,Expires,Status,Submitted,BillState,TaskHandler,TrackSub,Comments,IsNewRecord,TrackSubID, FinalAction,InvOvr ) values (@JobListID,@Track,@AddDate,@NeedDate,@Obtained,@Expires,@Status,@Submitted,@BillState,@TaskHandler,@TrackSub,@Comments,@IsNewRecord,@TrackSubID, @FinalAction,@InvOvr)");
                List<SqlParameter> Param = new List<SqlParameter>();
                Param.Add(new SqlParameter("@IsNewRecord", 1));
                Param.Add(new SqlParameter("@JobListID", selectedJobListID));
                Param.Add(new SqlParameter("@TaskHandler", grvPermitsRequiredInspection.Rows[cnt].Cells["cmbTaskHandler"].Value.ToString()));
                Param.Add(new SqlParameter("@Track", grvPermitsRequiredInspection.Rows[cnt].Cells["cmbTrack"].Value.ToString()));
                Param.Add(new SqlParameter("@Submitted", grvPermitsRequiredInspection.Rows[cnt].Cells["Submitted"].Value.ToString()));
                Param.Add(new SqlParameter("@BillState", grvPermitsRequiredInspection.Rows[cnt].Cells["cmbBillState"].Value.ToString()));
                Param.Add(new SqlParameter("@TrackSub", grvPermitsRequiredInspection.Rows[cnt].Cells["TrackSub"].Value.ToString()));
                Param.Add(new SqlParameter("@Comments", grvPermitsRequiredInspection.Rows[cnt].Cells["Comments"].Value.ToString()));
                Param.Add(new SqlParameter("@Status", grvPermitsRequiredInspection.Rows[cnt].Cells["cmbStatus"].Value.ToString()));
                Param.Add(new SqlParameter("@Obtained", grvPermitsRequiredInspection.Rows[cnt].Cells["Obtained"].Value.ToString()));
                Param.Add(new SqlParameter("@Expires", grvPermitsRequiredInspection.Rows[cnt].Cells["Expires"].Value.ToString()));
                Param.Add(new SqlParameter("@FinalAction", grvPermitsRequiredInspection.Rows[cnt].Cells["FinalAction"].Value.ToString()));
                Param.Add(new SqlParameter("@AddDate", grvPermitsRequiredInspection.Rows[cnt].Cells["AddDate"].Value.ToString()));
                Param.Add(new SqlParameter("@NeedDate", grvPermitsRequiredInspection.Rows[cnt].Cells["NeedDate"].Value.ToString()));
                Param.Add(new SqlParameter("@InvOvr", grvPermitsRequiredInspection.Rows[cnt].Cells["InvOvr"].Value.ToString()));
                Param.Add(new SqlParameter("@TrackSubID", grvPermitsRequiredInspection.Rows[cnt].Cells["TrackSubID"].Value.ToString()));

                int num = repo.db.Database.ExecuteSqlCommand(cmd.CommandText, Param.ToArray());
                repo.LoginActivityInfo(repo.db, "Insert", this.Text);
                if (num > 0)
                {

                    FillGridPermitRequiredInspection();
                    if (grvPermitsRequiredInspection.Rows.Count > 0)
                    {
                        grvPermitsRequiredInspection.Rows[grvPermitsRequiredInspection.Rows.Count - 1].Selected = true;
                        grvPermitsRequiredInspection.CurrentCell = grvPermitsRequiredInspection.Rows[grvPermitsRequiredInspection.Rows.Count - 1].Cells["comments"];
                    }

                    btnInsertPermit.Text = "Insert";
                }

            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Manager");
            }
        }

        protected void InsertNotes()
        {
            grvNotesCommunication.Rows[0].Cells["comments"].Selected = true;
            grvNotesCommunication.EndEdit();
            if (string.IsNullOrEmpty(grvNotesCommunication.Rows[grvNotesCommunication.Rows.Count - 1].Cells["Track"].Value.ToString()))
            {
                KryptonMessageBox.Show("Track field are compulsory", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(grvNotesCommunication.Rows[grvNotesCommunication.Rows.Count - 1].Cells["TrackSub"].Value.ToString()))
            {
                KryptonMessageBox.Show("TrackSub field are compulsory", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                btndeleteNotes.Enabled = true;
                int cnt = grvNotesCommunication.Rows.Count - 1;
                //DataAccessLayer DAL = new DataAccessLayer();

                SqlCommand cmd = new SqlCommand("Insert into Jobtracking(JobListID,Track,AddDate,NeedDate,Obtained,Expires, Status, Submitted,BillState,TaskHandler,TrackSub,Comments,IsNewRecord,TrackSubID,InvOvr,DeleteItemTimeService) values (@JobListID,@Track,@AddDate,@NeedDate,@Obtained,@Expires,@Status,@Submitted,@BillState,@TaskHandler,@TrackSub,@Comments,@IsNewRecord,@TrackSubID,@InvOvr,@DeleteItemTimeService)");
                List<SqlParameter> Param = new List<SqlParameter>();
                Param.Add(new SqlParameter("@IsNewRecord", 1));
                Param.Add(new SqlParameter("@JobListID", selectedJobListID));
                Param.Add(new SqlParameter("@TaskHandler", grvNotesCommunication.Rows[cnt].Cells["cmbTaskHandler"].Value.ToString()));
                Param.Add(new SqlParameter("@Track", grvNotesCommunication.Rows[cnt].Cells["cmbTrack"].Value.ToString()));
                Param.Add(new SqlParameter("@Submitted", grvNotesCommunication.Rows[cnt].Cells["Submitted"].Value.ToString()));
                Param.Add(new SqlParameter("@BillState", grvNotesCommunication.Rows[cnt].Cells["cmbBillState"].Value.ToString()));
                Param.Add(new SqlParameter("@TrackSub", grvNotesCommunication.Rows[cnt].Cells["TrackSub"].Value.ToString()));
                Param.Add(new SqlParameter("@Comments", grvNotesCommunication.Rows[cnt].Cells["Comments"].Value.ToString()));
                Param.Add(new SqlParameter("@Status", grvNotesCommunication.Rows[cnt].Cells["cmbStatus"].Value.ToString()));
                Param.Add(new SqlParameter("@Obtained", grvNotesCommunication.Rows[cnt].Cells["Obtained"].Value.ToString()));
                Param.Add(new SqlParameter("@Expires", grvNotesCommunication.Rows[cnt].Cells["Expires"].Value.ToString()));
                Param.Add(new SqlParameter("@AddDate", grvNotesCommunication.Rows[cnt].Cells["AddDate"].Value.ToString()));
                Param.Add(new SqlParameter("@NeedDate", grvNotesCommunication.Rows[cnt].Cells["NeedDate"].Value.ToString()));
                Param.Add(new SqlParameter("@InvOvr", grvNotesCommunication.Rows[cnt].Cells["InvOvr"].Value.ToString()));
                Param.Add(new SqlParameter("@TrackSubID", grvNotesCommunication.Rows[cnt].Cells["TrackSubID"].EditedFormattedValue.ToString()));
                Param.Add(new SqlParameter("@DeleteItemTimeService", grvNotesCommunication.Rows[cnt].Cells["DeleteItemTimeService"].EditedFormattedValue.ToString()));
                int num = repo.db.Database.ExecuteSqlCommand(cmd.CommandText, Param.ToArray());
                if (num > 0)
                {
                    FillGridNotesCommunication();
                    repo.LoginActivityInfo(repo.db, "Insert", this.Text);
                    if (grvNotesCommunication.Rows.Count > 0)
                    {
                        grvNotesCommunication.Rows[grvNotesCommunication.Rows.Count - 1].Selected = true;
                        grvNotesCommunication.CurrentCell = grvNotesCommunication.Rows[grvNotesCommunication.Rows.Count - 1].Cells["comments"];
                    }

                    btnInsertNotes.Text = "Insert";
                }
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Manager");
            }
        }
        #endregion
    }
}
