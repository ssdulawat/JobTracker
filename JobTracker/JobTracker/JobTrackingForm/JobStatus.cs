using JobTracker.JobTrackingMDIForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobTracker.JobTrackingForm
{
    public partial class JobStatus : Form
    {

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
        //public Notification ToasterNoty;
        public string CheckString;
        public ComboBox cb = new ComboBox();
        public string GridSymbole;
        public DataGridViewComboBoxCell cmbTCTackName;
        public static JobStatus _Instance;
        public string CopyPath;
        public string FolderName;
        public bool isDisable;
        public bool ManagerLoad;
        public Int64 JobID;
        public bool selectRecord_Joblist = false;
        // private DataAccessLayer DAL;
        public Int32 Colorid;
        public string UserName;
        public string UserType;
        public bool CheckUser;
        public string ColorColumn;
        const string TIMESERVICEFEE = "TimeServiceFee;";
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

        private Int16 processcount { get; set; }

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

                       // SetColumns();
                       // fillGridJobList();
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
                    }
                    if (processcount == 2)
                    {
                     //   SetColumnPermit();
                      //  FillGridPermitRequiredInspection();
                    }
                    if (processcount == 3)
                    {
                     //   SetColumnNotes();
                     //   FillGridNotesCommunication();
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
                    //processcount = processcount + 1;
                }

                ManagerLoad = false;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
