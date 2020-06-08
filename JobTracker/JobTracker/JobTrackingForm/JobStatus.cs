using DataAccessLayer;
using DataAccessLayer.Model;
using JobTracker.JobTrackingMDIForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            // ProgressBar1.Visible = false;
            //Label12.Visible = false;
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
                        var MainGrid = new List<ManagerData>();
                        MainGrid = dAL.GetManagerData();

                        grvJobList.DataSource = MainGrid;
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
                        //   SetColumnPreRequirment();
                        //  FillGridPreRequirment();

                        var PreRequirement = new List<PreRequirement>();
                        PreRequirement = dAL.GetPreRequirement();
                        grvPreRequirments.DataSource = PreRequirement;

                    }
                    if (processcount == 2)
                    {
                        //   SetColumnPermit();
                        //  FillGridPermitRequiredInspection();
                        var PermitRequiredInspection = new List<PermitsRequirement>();
                        PermitRequiredInspection = dAL.GetPermitsRequirement();
                        grvPreRequirments.DataSource = PermitRequiredInspection;
                    }
                    if (processcount == 3)
                    {
                        //   SetColumnNotes();
                        //   FillGridNotesCommunication();
                        var NotesCommunication = new List<NotesComunication>();
                        NotesCommunication = dAL.GetNotesComunication();
                        grvPreRequirments.DataSource = NotesCommunication;
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
            //try
            //{
            //    DataAccessLayer DAL = new DataAccessLayer();
            //    string queryString = "SELECT  DISTINCT    JobList.JobListID, JobList.JobNumber,JobList.Clienttext, Company.CompanyID, JobList.DateAdded, JobList.Description, JobList.Handler, JobList.Borough, JobList.Address, Contacts.FirstName + ' ' + Contacts.MiddleName + ' ' + Contacts.LastName AS Contacts, Contacts.EmailAddress, Contacts.ContactsID,   Company.CompanyName,JobList.ACContacts,JobList.ACEmail,JobList.OwnerName,JobList.OwnerAddress,JobList.OwnerPhone,JobList.OwnerFax,Company.CompanyNo, JobList.PMrv,     IsNull(JobList.IsDisable, 0) as IsDisable, IsNull(JobList.IsInvoiceHold, 0) as IsInvoiceHold," + " jd.InvoiceType AS TypicalInvoiceType, JobList.InvoiceClient, JobList.InvoiceContact,(Select dbo.ClientName(FirstName,MiddleName,LastName) FROM Contacts WHERE ContactsId LIKE jobList.InvoiceContact ) as InvoiceContactT ,JobList.InvoiceEmailAddress, JobList.InvoiceACContacts,(Select dbo.ClientName(FirstName,MiddleName,LastName) FROM Contacts WHERE ContactsId LIKE jobList.InvoiceACContacts ) as InvoiceACContactsT,JobList.InvoiceACEmail," + "CONVERT(INT,jd.TableVersionId) AS RateVersionId," + "jd.ServRate AS ServRate, IsNull(JobList.AdminInvoice, 0) as AdminInvoice FROM  JobList LEFT OUTER JOIN            Contacts ON JobList.ContactsID = Contacts.ContactsID LEFT OUTER JOIN      Company ON JobList.CompanyID = Company.CompanyID LEFT OUTER JOIN        JobTracking ON JobList.JobListID = JobTracking.JobListID INNER JOIN vwJobListDefaultValue jd ON JobList.JobListId=jd.JobListID {0}    WHERE (JobList.IsDelete=0 or JobList.IsDelete is null) ";
            //    // Dim queryString As String = "SELECT  DISTINCT    JobList.JobListID, JobList.JobNumber,JobList.Clienttext, Company.CompanyID, JobList.DateAdded, JobList.Description, JobList.Handler, JobList.Borough , JobList.Address,      Contacts.FirstName + ' ' + Contacts.MiddleName + ' ' + Contacts.LastName AS Contacts,( case when JobList.ContactsEmails='' or JobList.ContactsEmails is null then  Contacts.EmailAddress else JobList.ContactsEmails end) as EmailAddress, Contacts.ContactsID,   Company.CompanyName,JobList.ACContacts,JobList.ACEmail,JobList.OwnerName,JobList.OwnerAddress,JobList.OwnerPhone,JobList.OwnerFax,Company.CompanyNo FROM  JobList LEFT OUTER JOIN            Contacts ON JobList.ContactsID = Contacts.ContactsID LEFT OUTER JOIN      Company ON JobList.CompanyID = Company.CompanyID LEFT OUTER JOIN        JobTracking ON JobList.JobListID = JobTracking.JobListID     WHERE (JobList.IsDelete=0 or JobList.IsDelete is null)"

            //    if (this.txtJobListJobID.Text != "")
            //        queryString = queryString + " and JobList.JobNumber Like'%" + txtJobListJobID.Text + "%'";
            //    // If Me.txtJobListclient.Text <> "" Then queryString = queryString & " and CompanyName Like'%" & txtJobListclient.Text & "%'"
            //    if (this.txtJobListclient.Text != "")
            //        queryString = queryString + " and CHARINDEX( ISNULL(NULLIF('" + txtJobListclient.Text + "',''),CompanyName),CompanyName)>0 ";

            //    if (this.txtJobListAddress.Text != "")
            //        queryString = queryString + " and JobList.Address Like'%" + txtJobListAddress.Text + "%'";

            //    if (txtTown.Text != "")
            //        queryString = queryString + " and JobList.Borough like'" + txtTown.Text + "%'";

            //    if (txtJoblistClienttext.Text != "")
            //        queryString = queryString + " and JobList.Clienttext like'" + txtJoblistClienttext.Text + "%'";

            //    if (this.txtJobListSearchDescription.Text != "")
            //        queryString = queryString + " and JobList.Description like'%" + txtJobListSearchDescription.Text + "%'";

            //    if (this.cbxJobListPM.SelectedItem != "")
            //        queryString = queryString + " and Handler='" + cbxJobListPM.SelectedItem + "'";

            //    if (this.cbxJobListPMrv.SelectedItem != "")
            //        queryString = queryString + " and PMrv='" + cbxJobListPMrv.SelectedItem + "'";

            //    if (this.cbxSearchTm.SelectedItem != "")
            //        queryString = queryString + " and JobTracking.TaskHandler ='" + cbxSearchTm.SelectedItem + "'";

            //    if (chkShowOnlyPending.Checked == true)
            //        queryString = queryString + "AND JobList.JobListID IN ( SELECT JobListID FROM JobTracking WHERE Status='Pending' AND (IsDelete=0 or IsDelete is null ) )"; // (Old Query updated on Date-July 16,2012) and JobTracking.Status='Pending'"
            //    if (chkNotInvoiceJob.Checked == true)
            //    {
            //        queryString = string.Format(queryString, " INNER JOIN JobTracking JT ON JobList.JobListId = JT.JobListId AND  (JT.IsDelete=0 or JT.IsDelete is null )");
            //        // Unchecked Invoice Hold & Not Invoiced (TrackSub) & Not Pending (TrackSub) & Unchecked Disabled & Invoice Type = Item 
            //        // queryString = queryString & " AND JobList.JobListID IN ( SELECT JobListID FROM JobTracking WHERE BillState ='Not Invoiced' AND Status <> 'Pending' AND (IsDelete=0 or IsDelete is null ) ) AND JobList.TypicalInvoiceType='Item' AND (JobList.IsDisable IS NULL OR JobList.ISDisable=0) AND (JobList.IsInvoiceHold IS NULL OR JobList.IsInvoiceHold=0) "
            //        queryString = queryString + " AND ((JT.BillState ='Not Invoiced' AND JT.Status <> 'Pending' AND JobList.TypicalInvoiceType='Item' AND (JobList.IsDisable IS NULL OR JobList.ISDisable=0) AND (JobList.IsInvoiceHold IS NULL OR JobList.IsInvoiceHold=0)) ";
            //        // Unchecked Invoice Hold & Not Invoiced (Time) & Unchecked Disabled & Invoice Type = Time
            //        // queryString = queryString & " OR (JobList.JobListID IN ( SELECT JobListID FROM JobTracking WHERE Track='Time;' AND BillState ='Not Invoiced' AND (IsDelete=0 or IsDelete is null ) ) AND JobList.TypicalInvoiceType='Time' AND (JobList.IsDisable IS NULL OR JobList.ISDisable=0) AND (JobList.IsInvoiceHold IS NULL OR JobList.IsInvoiceHold=0) )"
            //        queryString = queryString + " OR ((SELECT COUNT(*) FROM TS_Time WHERE JobListId=JobList.JobListId AND BillState='Not Invoice')> 0 AND JobList.TypicalInvoiceType='Time' AND (JobList.IsDisable IS NULL OR JobList.ISDisable=0) AND (JobList.IsInvoiceHold IS NULL OR JobList.IsInvoiceHold=0))";
            //        // Unchecked Invoice Hold & Not Invoiced (Expense) & Unchecked Disabled
            //        // queryString = queryString & " OR (JobList.JobListID IN ( SELECT JobListID FROM JobTracking WHERE Track='Reinburs;' AND BillState ='Not Invoiced' AND (IsDelete=0 or IsDelete is null ) )  AND (JobList.IsDisable IS NULL OR JobList.ISDisable=0) AND (JobList.IsInvoiceHold IS NULL OR JobList.IsInvoiceHold=0) )"
            //        queryString = queryString + " OR ((SELECT COUNT(*) FROM TS_Expences WHERE JobListId=JobList.JobListId AND BillState='Not Invoice')> 0 AND (JobList.IsDisable IS NULL OR JobList.ISDisable=0) AND (JobList.IsInvoiceHold IS NULL OR JobList.IsInvoiceHold=0)))";
            //    }
            //    else
            //        queryString = string.Format(queryString, "");
            //    if (chkShowDisabled.Checked == false)
            //        queryString = queryString + " AND (JobList.IsDisable = 0  OR JobList.IsDisable IS NULL)";

            //    if (chkInvoiceHold.Checked == true)
            //        queryString = queryString + " AND (JobList.IsInvoiceHold = 1 )";
            //    else
            //        queryString = queryString + " AND (JobList.IsInvoiceHold = 0 )";

            //    if (txtCommentsPreRequire.Text.Trim != string.Empty)
            //        queryString = queryString + " AND JobList.JobListID IN (SELECT JobListID FROM JobTracking WHERE Comments like '%" + txtCommentsPreRequire.Text.Trim + "%' AND (IsDelete=0 or IsDelete is null ) )";
            //    if (cmbTMWithPending.Text.Trim != "")
            //        queryString = queryString + " AND JobList.JobListID IN (SELECT JobListID FROM JobTracking WHERE Status='Pending' AND TaskHandler =  '" + cmbTMWithPending.Text.Trim + "' AND (IsDelete=0 or IsDelete is null ))";
            //    // If cmbBillStatePermit.Text.Trim <> String.Empty Then
            //    // queryString = queryString & " AND JobList.JobListID IN ( SELECT JobListID FROM JobTracking WHERE BillState ='" + cmbBillStatePermit.Text.Trim + "' ) "
            //    // End If
            //    if (selectRecord_Joblist == true)
            //        queryString = queryString + "AND JobList.JobListID IN (SELECT TOP 100 JobListID FROM JobList WHERE IsDelete=0 or IsDelete is null order by JobListID DESC )";
            //    string startDate;
            //    string endDate;
            //    Int16 index;
            //    if (chkYear.Checked == true)
            //    {
            //        // index = cmbYear.SelectedIndex * -1
            //        startDate = (System.DateTime.Now.AddYears(index)).Year.ToString() + "-01-01 00:00:00.000";
            //        endDate = (System.DateTime.Now.AddYears(index + 1)).Year.ToString() + "-01-01 00:00:00.000";
            //        // queryString = queryString & " AND  JobList.DateAdded > '" + startDate + "' AND  JobList.DateAdded < '" + endDate + "' "
            //        queryString = queryString + " AND  YEAR(JobList.DateAdded) = " + cmbYear.Text;
            //    }
            //    queryString = queryString + "  order by JobList.JobListID  ";
            //    try
            //    {
            //        // Attempt to load the dataset.
            //        dtJL = DAL.Filldatatable(queryString);
            //        grvJobList.DataSource = dtJL;
            //    }
            //    catch (Exception eLoad)
            //    {
            //        // Add your error handling code here.
            //        // Display error message, if any.
            //        KryptonMessageBox.Show(eLoad.Message, "Manager");
            //    }
            //    // Grid Formatting
            //    {
            //        var withBlock = grvJobList;
            //        withBlock.Columns("JobListID").Visible = false;
            //        withBlock.Columns("JobNumber").HeaderText = "Job#";
            //        withBlock.Columns("JobNumber").Width = 80;
            //        withBlock.Columns("DateAdded").Width = 1;
            //        withBlock.Columns("DateAdded").HeaderText = "Added";
            //        withBlock.Columns("DateAdded").Width = 80;
            //        withBlock.Columns("Description").HeaderText = "Description";
            //        withBlock.Columns("Clienttext").HeaderText = "Client Text";
            //        withBlock.Columns("Clienttext").Width = 180;
            //        withBlock.Columns("Description").Width = 200;
            //        withBlock.Columns("Handler").HeaderText = "PM";
            //        withBlock.Columns("Handler").Width = 40;
            //        withBlock.Columns("Address").Width = 150;
            //        withBlock.Columns("CompanyID").Width = 130;
            //        withBlock.Columns("Borough").Width = 90;
            //        withBlock.Columns("Borough").HeaderText = "Town";
            //        withBlock.Columns("Contacts").Width = 130;
            //        // .Columns("Contacts").ReadOnly = True
            //        withBlock.Columns("EmailAddress").Width = 250;
            //        withBlock.Columns("Handler").Visible = false;
            //        // .Columns("Borough").Visible = False
            //        withBlock.Columns("CompanyID").Visible = false;
            //        withBlock.Columns("Contacts").Visible = true;
            //        withBlock.Columns("ContactsID").Visible = false;
            //        withBlock.Columns("CompanyName").Visible = false;
            //        withBlock.Columns("OwnerName").HeaderText = "Owner Name";
            //        withBlock.Columns("OwnerAddress").HeaderText = "Owner Address";
            //        withBlock.Columns("OwnerPhone").HeaderText = "Owner Phone";
            //        withBlock.Columns("OwnerFax").HeaderText = "Owner Fax";
            //        withBlock.Columns("ACContacts").HeaderText = "AC Contacts";
            //        withBlock.Columns("ACEmail").HeaderText = "AC Email";
            //        withBlock.Columns("CompanyNo").Visible = false;
            //        // .Columns("PMrv").DisplayIndex = (grvJobList.Columns.Count - 2)
            //        withBlock.Columns("PMrv").HeaderText = "PMrv";
            //        withBlock.Columns("PMrv").Width = 40;
            //        // .Columns("DBadClient").Visible = False
            //        withBlock.Columns("PMrv").Visible = false;

            //        withBlock.Columns("IsDisable").DisplayIndex = grvJobList.Columns.Count - 11;
            //        withBlock.Columns("IsDisable").HeaderText = "Disabled";
            //        withBlock.Columns("IsDisable").Width = 60;

            //        withBlock.Columns("IsInvoiceHold").DisplayIndex = grvJobList.Columns.Count - 10;
            //        withBlock.Columns("IsInvoiceHold").HeaderText = "Invoice Hold";
            //        withBlock.Columns("IsInvoiceHold").Width = 100;



            //        withBlock.Columns("cmbInvoiceClient").HeaderText = "InvoiceClient";
            //        withBlock.Columns("cmbInvoiceClient").DisplayIndex = grvJobList.Columns.Count - 9;
            //        withBlock.Columns("InvoiceContact").Width = 90;

            //        withBlock.Columns("InvoiceContact").Visible = false;
            //        withBlock.Columns("InvoiceContactT").HeaderText = "InvoiceContact";
            //        withBlock.Columns("InvoiceContactT").DisplayIndex = grvJobList.Columns.Count - 8;
            //        withBlock.Columns("InvoiceEmailAddress").Width = 90;
            //        withBlock.Columns("InvoiceEmailAddress").HeaderText = "InvoiceEmailAddress";
            //        withBlock.Columns("InvoiceEmailAddress").DisplayIndex = grvJobList.Columns.Count - 7;

            //        withBlock.Columns("InvoiceACContacts").Visible = false;
            //        withBlock.Columns("InvoiceACContactsT").Width = 90;
            //        withBlock.Columns("InvoiceACContactsT").HeaderText = "InvoiceACContacts";
            //        withBlock.Columns("InvoiceACContactsT").DisplayIndex = grvJobList.Columns.Count - 6;

            //        withBlock.Columns("InvoiceACEmail").Width = 90;
            //        withBlock.Columns("InvoiceACEmail").HeaderText = "InvoiceACEmail";
            //        withBlock.Columns("InvoiceACEmail").DisplayIndex = grvJobList.Columns.Count - 5;

            //        withBlock.Columns("cmbTypicalInvoiceType").DisplayIndex = grvJobList.Columns.Count - 4;
            //        withBlock.Columns("TypicalInvoiceType").HeaderText = "Invoice Type";
            //        withBlock.Columns("TypicalInvoiceType").Width = 100;
            //        // Item rate column display index setup code will found it partial calss

            //        withBlock.Columns("ServRate").Width = 90;
            //        withBlock.Columns("ServRate").HeaderText = "Serv Rate";
            //        withBlock.Columns("ServRate").DisplayIndex = grvJobList.Columns.Count - 2;

            //        withBlock.Columns("AdminInvoice").Width = 100;
            //        withBlock.Columns("AdminInvoice").HeaderText = "Admin Inv.";
            //        withBlock.Columns("AdminInvoice").DisplayIndex = grvJobList.Columns.Count - 1;
            //    }
            //    JobListGridRateVersionColumn(grvJobList);

            //    if (My.Settings.PretimeSheetLoginUserType == "Admin" | My.Settings.timeSheetLoginUserType == "Admin")
            //    {
            //        UserType = "Admin";
            //        grvJobList.Columns("IsDisable").Visible = true;
            //        grvJobList.Columns("IsInvoiceHold").Visible = true;
            //    }
            //    else
            //    {
            //        grvJobList.Columns("IsDisable").Visible = false;
            //        grvJobList.Columns("IsInvoiceHold").Visible = false;
            //    }

            //    if (grvJobList.Rows.Count > 0)
            //    {
            //        grvJobList.CurrentCell = grvJobList.Rows(grvJobList.Rows.Count - 1).Cells("Address");
            //        grvJobList.Rows(grvJobList.Rows.Count - 1).Selected = true;

            //        selectedJobListID = Convert.ToInt32(grvJobList.Item("JobListID", grvJobList.Rows.Count - 1).Value);
            //        isDisabled = Convert.ToBoolean(grvJobList.Item("IsDisable", grvJobList.Rows.Count - 1).Value);
            //        lblCompanyNo.Text = "Client No:- " + grvJobList.Rows(grvJobList.CurrentRow.Index).Cells("CompanyNo").Value.ToString();
            //    }
            //    else
            //    {
            //        selectedJobListID = 0;
            //        isDisabled = false;
            //    }
            //    if (grvJobList.Rows.Count > 0)
            //        grvJobList.CurrentCell = grvJobList.Rows(grvJobList.Rows.Count - 1).Cells("Address");
            //    if (grvJobList.Rows.Count > 0)
            //    {
            //        selectedJobListID = Convert.ToInt32(grvJobList.Item("JobListID", grvJobList.Rows.Count - 1).Value);
            //        isDisabled = Convert.ToBoolean(grvJobList.Item("IsDisable", grvJobList.Rows.Count - 1).Value);
            //    }
            //    if (selectRecord_Joblist == false)
            //    {
            //        FillGridPreRequirment();
            //        FillGridPermitRequiredInspection();
            //        FillGridNotesCommunication();
            //        if ((isDisabled))
            //            disableJob(true);
            //        else
            //            disableJob(false);
            //        SetBadClient();
            //        if (grvJobList.Rows.Count > 0)
            //            ChangeDirJobNumber(grvJobList.Rows.Count - 1);
            //        ChangeTraficLight(grvJobList.Rows.Count - 1);
            //    }
            //    selectRecord_Joblist = false;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
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

                var TempColumn = dAL.GetManagerGridSetColumn();
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

    }
}
