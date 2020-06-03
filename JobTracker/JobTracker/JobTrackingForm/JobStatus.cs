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
        #endregion

        public JobStatus()
        {
            InitializeComponent();
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

    }
}
