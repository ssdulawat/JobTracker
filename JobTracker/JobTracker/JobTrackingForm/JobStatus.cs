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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace JobTracker.JobTrackingForm
{
    public partial class JobStatus : Form
    {
        Manager dAL = new Manager();
        #region
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

        #region "Properties"
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

        public JobStatus()
        {
            InitializeComponent();
        }
        public void New()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            //Add any initialization after the InitializeComponent() call
        }


        private void JobStatus_Load(System.Object sender, System.EventArgs e)
        {
            ProgressBar1.Visible = false;
            label11.Visible = false;
            // mdio = MdiParent;
            ManagerLoad = true;
            btnImportTimeSheetData.Visible = false;
            BtnHistoryClick.Visible = true;

            //DefaultValueSetup();
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
                        //var MainGrid = new List<ManagerData>();
                        //MainGrid = dAL.GetManagerData();

                        //grvJobList.DataSource = MainGrid;
                        selectRecord_Joblist = false;
                    }
                    catch (Exception ex)
                    {
                    }
                }
                if (selectRecord_Joblist == false)
                {
                    if (processcount == 1)
                    {
                        SetColumnPreRequirment();
                        FillGridPreRequirment();

                        //var PreRequirement = new List<PreRequirement>();
                        //PreRequirement = dAL.GetPreRequirement();
                        //grvPreRequirments.DataSource = PreRequirement;

                    }
                    if (processcount == 2)
                    {
                        SetColumnPermit();
                        FillGridPermitRequiredInspection();
                        //var PermitRequiredInspection = new List<PermitsRequirement>();
                        //PermitRequiredInspection = dAL.GetPermitsRequirement();
                        //grvPreRequirments.DataSource = PermitRequiredInspection;
                    }
                    if (processcount == 3)
                    {
                        SetColumnNotes();
                        FillGridNotesCommunication();
                        //var NotesCommunication = new List<NotesComunication>();
                        //NotesCommunication = dAL.GetNotesComunication();
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
            }
        }

        private void fillGridJobList()
        {
            try
            {
                string queryString = "SELECT  DISTINCT    JobList.JobListID, JobList.JobNumber,JobList.Clienttext, Company.CompanyID, JobList.DateAdded, JobList.Description, JobList.Handler, JobList.Borough, JobList.Address, Contacts.FirstName + ' ' + Contacts.MiddleName + ' ' + Contacts.LastName AS Contacts, Contacts.EmailAddress, Contacts.ContactsID,Company.CompanyName,JobList.ACContacts,JobList.ACEmail,JobList.OwnerName,JobList.OwnerAddress,JobList.OwnerPhone,JobList.OwnerFax,Company.CompanyNo, JobList.PMrv,     IsNull(JobList.IsDisable, 0) as IsDisable, IsNull(JobList.IsInvoiceHold, 0) as IsInvoiceHold," + " jd.InvoiceType AS TypicalInvoiceType, JobList.InvoiceClient, JobList.InvoiceContact,(Select dbo.ClientName(FirstName,MiddleName,LastName) FROM Contacts WHERE ContactsId LIKE jobList.InvoiceContact ) as InvoiceContactT ,JobList.InvoiceEmailAddress, JobList.InvoiceACContacts,(Select dbo.ClientName(FirstName,MiddleName,LastName) FROM Contacts WHERE ContactsId LIKE jobList.InvoiceACContacts ) as InvoiceACContactsT,JobList.InvoiceACEmail," + "CONVERT(INT,jd.TableVersionId) AS RateVersionId," + "jd.ServRate AS ServRate, IsNull(JobList.AdminInvoice, 0) as AdminInvoice FROM  JobList LEFT OUTER JOIN Contacts ON JobList.ContactsID = Contacts.ContactsID LEFT OUTER JOIN Company ON JobList.CompanyID = Company.CompanyID LEFT OUTER JOIN JobTracking ON JobList.JobListID = JobTracking.JobListID INNER JOIN vwJobListDefaultValue jd ON JobList.JobListId=jd.JobListID {0}  WHERE (JobList.IsDelete=0 or JobList.IsDelete is null) ";

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
                string startDate;
                string endDate;
                int index = 0;
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
                    //dtJL = DAL.Filldatatable(queryString);
                    var TempDataAfterFilter = dAL.GetManagerDataAfterFilter(queryString);
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
                    // .Columns("Contacts").ReadOnly = True
                    withBlock.Columns["EmailAddress"].Width = 250;
                    withBlock.Columns["Handler"].Visible = false;
                    // .Columns("Borough").Visible = False
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
                //if (My.Settings.PretimeSheetLoginUserType == "Admin" | My.Settings.timeSheetLoginUserType == "Admin")
                //{
                //    UserType = "Admin";
                //    grvJobList.Columns["IsDisable"].Visible = true;
                //    grvJobList.Columns["IsInvoiceHold"].Visible = true;
                //}
                //else
                //{
                //    grvJobList.Columns["IsDisable"].Visible = false;
                //    grvJobList.Columns["IsInvoiceHold"].Visible = false;
                //}

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
            {
                var withBlock = grd;
                if ((withBlock.Columns["cmbRateVersion"] == null))
                {
                    //DataAccessLayer DAL = new DataAccessLayer();
                    DataGridViewComboBoxColumn cmbVersionTable = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = cmbVersionTable;
                        // su
                        var TempTableVersion = dAL.GetTableVersion();
                        //dt1 = DAL.Filldatatable("select * from VersionTable  union SELECT 0 as TableVersionId, '--Use Default--' as TableVersionName order by TableVersionId");
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
        }

        private void FillGridNotesCommunication()
        {
            try
            {
                string queryString = "SELECT JobTracking.JobListID,JobList.JobNumber,JobTracking.TaskHandler,JobTracking.Track,JobTracking.TrackSub, JobTracking.Comments,JobTracking.Status,JobTracking.Submitted, JobTracking.Obtained,JobTracking.Expires,JobTracking.BillState , JobTracking.AddDate, JobTracking.NeedDate,     JobTracking.JobTrackingID,JobTracking.TrackSubID," + "JobTracking.InvOvr," + "JobTracking.DeleteItemTimeService  FROM  JobTracking INNER JOIN    JobList ON JobTracking.JobListID = JobList.JobListID where JobTracking.Track in (select Trackname from MasterTrackSet where TrackSet='Notes/Communication')  and  JobTracking.JobListID=" + selectedJobListID + "and (JobTracking.IsDelete=0 or JobTracking.IsDelete is null) ";

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
                    var TempNotesComunicationAfterFilter = dAL.GetNotesComunicationDataAfterFilter(queryString);
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
                    // .Columns("TrackSub").Visible = False
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
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void FillGridPermitRequiredInspection()
        {
            try
            {
                // DataAccessLayer DAL = new DataAccessLayer();

                string queryString = "SELECT     JobTracking.JobListID, JobList.JobNumber, JobTracking.TaskHandler, JobTracking.Track, JobTracking.TrackSub, JobTracking.Comments, JobTracking.Status, JobTracking.Submitted, JobTracking.Obtained, JobTracking.Expires, JobTracking.FinalAction, JobTracking.BillState, JobTracking.AddDate, JobTracking.NeedDate, JobTracking.JobTrackingID, JobTracking.TrackSubID,JobTracking.InvOvr FROM JobTracking INNER JOIN JobList ON JobTracking.JobListID = JobList.JobListID WHERE (JobTracking.Track IN(SELECT TrackName FROM MasterTrackSet WHERE (TrackSet = 'Permits/Required/Inspection'))) AND (JobTracking.JobListID = " + selectedJobListID + ") AND (JobTracking.IsDelete = 0 OR JobTracking.IsDelete IS NULL)";

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
                    var TempPermitsRequiredAfterFilter = dAL.GetPermitsRequirementDataAfterFilter(queryString);
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
                    // .Columns("FinalAction").HeaderText = "Final Action"
                    // .Columns("FinalAction").Visible = True
                    // .Columns("FinalAction").Width = 80
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
                    // .Columns("TrackSub").Visible = False
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

                var TempColumn = dAL.ManagerGridSetColumn();
                DataTable dtJL = new DataTable();
                dtJL = ToDataTable(TempColumn);
                grvJobList.DataSource = dtJL;

                cbxClient.Name = "Client#";
                cbxClient.Width = 200;

                grvJobList.Columns.Insert(1, cbxClient);

                DataTable dt = new DataTable();

                var TempCBlient = dAL.GetcbxClient();
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
                //TestVariousInfoEntities cmbobj = new TestVariousInfoEntities();
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

                        var TempColPM = dAL.GetcolPMM();
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
                        var TempColPM = dAL.GetcolPMM();
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
            }
        }
        private void SetColumnPreRequirment()
        {
            try
            {

                try
                {
                    // Attempt to load the dataset.
                    var TempPreColumn = dAL.PreRequirementSetColumn();
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
                        var TempPreColTM = dAL.GetPreRequirementcolTM();
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
                        var TempPrecolTrack = dAL.GetPreRequirementcolTrack();
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
                        var TempPrecolStatusDT = dAL.GetPreRequirementcolStatus();
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
            }
        }

        private void SetColumnPermit()
        {
            try
            {

                try
                {
                    // Attempt to load the dataset.
                    var TempColumnPermit = dAL.PermitsRequirementSetColumn();
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
                        var TempPreColTM = dAL.GetPreRequirementcolTM();
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
                        var TempPremitcolTrack = dAL.GetPermitsRequirementcolTrack();
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
                        var TempPremitcolStatusDT = dAL.GetPreRequirementcolStatus();
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
                        var TempPremitcolBillStatusDT = dAL.GetPermitsRequirementcolBillStatus();
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
                    var TempColumnNotes = dAL.NotesSetColumn();
                    dtNotes = ToDataTable(TempColumnNotes);

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
                    DataGridViewComboBoxColumn colTM = new DataGridViewComboBoxColumn();
                    {
                        var withBlock1 = colTM;
                        DataTable colTMDT = new DataTable();
                        var TempNotescolTM = dAL.GetPreRequirementcolTM();
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
                        var TempNotescolTrack = dAL.GetNotescolTrack();
                        colTrackDT = ToDataTable(TempNotescolTrack);
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
                        var TempNotescolStatusDT = dAL.GetPreRequirementcolStatus();
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
                        var NotescolBillStatusDT = dAL.GetPermitsRequirementcolBillStatus();
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
            }
        }

        private void FillGridPreRequirment()
        {
            try
            {
                string queryString = "SELECT JobTracking.JobListID,JobList.JobNumber,JobTracking.TaskHandler,JobTracking.Track,JobTracking.TrackSub, JobTracking.Comments,JobTracking.Status,JobTracking.Submitted, JobTracking.Obtained,JobTracking.Expires,JobTracking.BillState , JobTracking.AddDate, JobTracking.NeedDate,     JobTracking.JobTrackingID,JobTracking.TrackSubID,JobTracking.InvOvr  FROM  JobTracking INNER JOIN    JobList ON JobTracking.JobListID = JobList.JobListID where JobTracking.Track in (SELECT Trackname FROM MasterTrackSet WHERE TrackSet='PreRequirements') and  JobTracking.JobListID=" + selectedJobListID + " and (JobTracking.IsDelete=0 or JobTracking.IsDelete is null)";

                // If Me.chkShowOnlyPendingTrack.Checked Then queryString = queryString & " and JobTracking.Status ='Pending'"
                if (cbxSearchTm.Text.Trim() == "")
                    queryString = queryString + " AND JobTracking.TaskHandler= '" + cbxSearchTm.Text.Trim() + "'";
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
                    var TempPreRequirementDataAfter = dAL.GetPreRequirementDataAfterFilter(queryString);
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
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            TestVariousInfoEntities DAL = new TestVariousInfoEntities();
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

                            if (DAL.Database.ExecuteSqlCommand(cmd.ToString(), Param) > 0)
                            {
                                grvJobList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                                grvJobList.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(159, 207, 255);
                                KryptonMessageBox.Show("Update Successfully", "Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }

                        catch (Exception eLoad)
                        {
                            KryptonMessageBox.Show(eLoad.Message, "Manager");
                        }
                        grvJobList.CurrentCell = grvJobList.Rows[cnt].Cells["Address"];
                        grvJobList.Rows[cnt].Selected = true;
                    }
                    catch (Exception eUpdate)
                    {
                        KryptonMessageBox.Show(eUpdate.Message, "Manager");
                    }
                }
            }
            grvJobList_CellEnter(sender, e);
        }

        protected void InsertJobList()
        {
            // grvJobList.Rows(0).Cells("JobNumber").Selected = True 'move this line to below until input validate
            grvJobList.EndEdit();
            TestVariousInfoEntities DAL = new TestVariousInfoEntities();
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
                    Param.Add(new SqlParameter("@CompanyID", (System.Windows.Forms.DataGridViewComboBoxCell)grvJobList.Rows[cnt].Cells["Client#"].Value));
                int ContactsID;
                if (grvJobList.Rows[cnt].Cells["ContactsID"].Value.ToString() == "")
                {
                    Param.Add(new SqlParameter("@ContactsID", 0));
                    ContactsID = 0;
                }
                else
                {
                    ContactsID = Convert.ToInt32(grvJobList.Rows[cnt].Cells["ContactsID"].Value.ToString());
                    Param.Add(new SqlParameter("@ContactsID", grvJobList.Rows[cnt].Cells["ContactsID"].Value.ToString()));
                }
                //(DAL.Database.ExecuteSqlCommand(cmd.ToString(), Param) > 0)
                int num = DAL.Database.ExecuteSqlCommand(cmd.ToString(), Param);

                if (num > 0)
                {
                    // System.Windows.Forms.MessageBox.Show("Record Saved!", "Message")
                    fillGridJobList();
                    grvJobList.Rows[grvJobList.Rows.Count - 1].Selected = true;
                    grvJobList.CurrentCell = grvJobList.Rows[grvJobList.Rows.Count - 1].Cells["JobNumber"];
                    btnAdd.Text = "Insert";
                    //DAL.LoginActivityInfo("Insert", this.Text);
                }
            }
            catch (Exception ex)
            {
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

        private void grvJobList_CellEnter(object sender, DataGridViewCellEventArgs e)
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
                    try
                    {
                        //int i = GetValueMemberID();
                        //if (i == 0)
                        //{
                        //    break;
                        //}

                        //grvJobList.Rows[e.RowIndex].Cells["ContactsID"].Value = i;
                    }
                    catch (Exception ex)
                    {

                    }
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
                    }
                }
            }
        }

        private void grvJobList_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
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
        private void grvJobList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void grvJobList_KeyDown(object sender, KeyEventArgs e)
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
                    // Dim JobNo As String = grvJobList.Rows(grvJobList.CurrentRow.Index + 1).Cells("JobNumber").Value.ToString
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
    }
}
