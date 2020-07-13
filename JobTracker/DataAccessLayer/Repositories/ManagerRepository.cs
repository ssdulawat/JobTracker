using DataAccessLayer.Model;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ManagerRepository : BaseRepository, IDisposable
    {
        public EFDbContext db = null;
        //public ManagerRepository():this(db)
        //{
        //    db = new EFDbContext();
        //    //ManagerRepository(db);
        //}
        public ManagerRepository()
        {
            this.db = GetDbContext();
        }

        public List<ManagerData> GetManagerData()
        {
            string queryString = "SELECT  DISTINCT JobList.JobListID, JobList.JobNumber,JobList.Clienttext, Company.CompanyID, JobList.DateAdded AS Added, JobList.Description, JobList.Handler AS PM, JobList.Borough AS Town, JobList.Address, Contacts.FirstName + ' ' + Contacts.MiddleName + ' ' + Contacts.LastName AS Contacts, Contacts.EmailAddress, Contacts.ContactsID,Company.CompanyName,JobList.ACContacts,JobList.ACEmail,JobList.OwnerName,JobList.OwnerAddress,JobList.OwnerPhone,JobList.OwnerFax,Company.CompanyNo, JobList.PMrv, IsNull(JobList.IsDisable, 0) as IsDisable, IsNull(JobList.IsInvoiceHold, 0) as IsInvoiceHold, jd.InvoiceType AS TypicalInvoiceType, JobList.InvoiceClient, JobList.InvoiceContact,(Select dbo.ClientName(FirstName,MiddleName,LastName) FROM Contacts WHERE ContactsId LIKE jobList.InvoiceContact ) as InvoiceContactT ,JobList.InvoiceEmailAddress, JobList.InvoiceACContacts,(Select dbo.ClientName(FirstName,MiddleName,LastName) FROM Contacts WHERE ContactsId LIKE jobList.InvoiceACContacts ) as InvoiceACContactsT,JobList.InvoiceACEmail,CONVERT(INT,jd.TableVersionId) AS RateVersionId,jd.ServRate AS ServRate, IsNull(JobList.AdminInvoice, 0) as AdminInvoice FROM  JobList LEFT OUTER JOIN            Contacts ON JobList.ContactsID = Contacts.ContactsID LEFT OUTER JOIN Company ON JobList.CompanyID = Company.CompanyID LEFT OUTER JOIN        JobTracking ON JobList.JobListID = JobTracking.JobListID INNER JOIN vwJobListDefaultValue jd ON JobList.JobListId=jd.JobListID     WHERE (JobList.IsDelete=0 or JobList.IsDelete is null)  AND (JobList.IsDisable = 0  OR JobList.IsDisable IS NULL) AND (JobList.IsInvoiceHold = 0 )  order by JobList.JobListID";
            var result = db.Database.SqlQuery<ManagerData>(queryString).ToList();

            return result;
        }
        public List<PreRequirement> GetPreRequirement()
        {
            string queryString = "SELECT JobList.JobNumber,JobTracking.TaskHandler AS TM,JobTracking.Track,JobTracking.TrackSub, JobTracking.Comments,JobTracking.Status,JobTracking.Submitted, JobTracking.Obtained,JobTracking.Expires,JobTracking.BillState , JobTracking.AddDate, JobTracking.NeedDate,     JobTracking.JobTrackingID,JobTracking.TrackSubID,JobTracking.InvOvr FROM  JobTracking INNER JOIN JobList ON JobTracking.JobListID =JobList.JobListID where JobTracking.Track in (SELECT Trackname FROM MasterTrackSet WHERE TrackSet = 'PreRequirements') and JobTracking.JobListID = 1838  and(JobTracking.IsDelete = 0 or JobTracking.IsDelete is null)  order by JobTrackingID";



            var result = db.Database.SqlQuery<PreRequirement>(queryString).ToList();

            return result;
        }
        public List<PermitsRequirement> GetPermitsRequirement()
        {
            string queryString = "SELECT   JobTracking.TaskHandler AS TM, JobTracking.Track, JobTracking.TrackSub, JobTracking.Comments, JobTracking.Status, JobTracking.Submitted, JobTracking.Obtained, JobTracking.Expires, JobTracking.FinalAction, JobTracking.BillState, JobTracking.AddDate AS Added,JobTracking.InvOvr FROM JobTracking INNER JOIN JobList ON JobTracking.JobListID = JobList.JobListID WHERE (JobTracking.Track IN(SELECT TrackName FROM MasterTrackSet WHERE (TrackSet = 'Permits/Required/Inspection'))) AND (JobTracking.JobListID = 2773 ) AND (JobTracking.IsDelete = 0 OR JobTracking.IsDelete IS NULL) order by JobTrackingID";

            var result = db.Database.SqlQuery<PermitsRequirement>(queryString).ToList();

            return result;
        }
        public List<NotesComunication> GetNotesComunication()
        {
            string queryString = "SELECT JobTracking.TaskHandler AS TM,JobTracking.Track,JobTracking.TrackSub, JobTracking.Comments,JobTracking.Status,JobTracking.BillState , JobTracking.AddDate AS Added,JobTracking.InvOvr  FROM  JobTracking INNER JOIN    JobList ON JobTracking.JobListID = JobList.JobListID where JobTracking.Track in (select Trackname from MasterTrackSet where TrackSet='Notes/Communication')  and  JobTracking.JobListID= 2773 and (JobTracking.IsDelete=0 or JobTracking.IsDelete is null)  order by JobTrackingID";

            var result = db.Database.SqlQuery<NotesComunication>(queryString).ToList();

            return result;
        }
        public List<ManagerSetColumn> ManagerGridSetColumn()
        {
            string queryString = "SELECT top 5 JobList.JobListID, JobList.JobNumber,JobList.Clienttext, Company.CompanyID, JobList.DateAdded, JobList.Description, JobList.Handler, JobList.Borough, JobList.Address, Contacts.FirstName + ' ' + Contacts.MiddleName + ' ' + Contacts.LastName AS Contacts, Contacts.EmailAddress, Contacts.ContactsID,   Company.CompanyName,JobList.ACContacts,JobList.ACEmail,JobList.OwnerName,JobList.OwnerAddress,JobList.OwnerPhone, JobList.OwnerFax,Company.CompanyNo, JobList.PMrv,  IsNull(JobList.IsDisable, 0) as IsDisable, IsNull(JobList.IsInvoiceHold, 0) as IsInvoiceHold,JobList.TypicalInvoiceType,JobList.InvoiceClient,JobList.ServRate, IsNull(JobList.AdminInvoice, 0) as AdminInvoice FROM  JobList LEFT OUTER JOIN  Contacts ON JobList.ContactsID = Contacts.ContactsID LEFT OUTER JOIN Company ON JobList.CompanyID = Company.CompanyID LEFT OUTER JOIN  JobTracking ON JobList.JobListID = JobTracking.JobListID WHERE (JobList.IsDelete=0 or JobList.IsDelete is null)";

            var result = db.Database.SqlQuery<ManagerSetColumn>(queryString).ToList();

            return result;
        }
        public List<cbxClientM> GetcbxClient()
        {
            string queryString = "SELECT CompanyName,CompanyID,TableVersionId  FROM dbo.Company where CompanyName<> '' AND (IsDelete=0 or IsDelete is null)  union select'' as CompanyName,0 as CompanyID,0 AS TableVersionId ORDER BY CompanyName";

            var result = db.Database.SqlQuery<cbxClientM>(queryString).ToList();

            return result;
        }
        public List<colPMM> GetcolPMM()
        {
            string queryString = "SELECT cTrack ,Id FROM MasterItem WHERE cGroup='PM' and (IsDelete=0 or IsDelete is null) ORDER BY cTrack";
            var result = db.Database.SqlQuery<colPMM>(queryString).ToList();

            return result;
        }
        public List<PreRequirementSetColumn> PreRequirementSetColumn()
        {
            string queryString = "SELECT JobTracking.JobListID,JobList.JobNumber,JobTracking.TaskHandler,JobTracking.Track,JobTracking.TrackSub, JobTracking.Comments,JobTracking.Status,JobTracking.Submitted,JobTracking.Obtained,JobTracking.Expires,JobTracking.BillState , JobTracking.AddDate, JobTracking.NeedDate,JobTracking.JobTrackingID,JobTracking.TrackSubID,JobTracking.InvOvr FROM  JobTracking INNER JOIN JobList ON JobTracking.JobListID = JobList.JobListID where JobTracking.Track = 'Client Re;'   and JobTracking.JobListID = 0 and(JobTracking.IsDelete = 0 or JobTracking.IsDelete is null)";
            var result = db.Database.SqlQuery<PreRequirementSetColumn>(queryString).ToList();

            return result;
        }
        public List<colPreRequirTMM> GetPreRequirementcolTM_D()
        {
            string queryString = "SELECT cTrack ,Id FROM MasterItem WHERE cGroup='TM' and (IsDelete=0 or IsDelete is null) AND (isDisable <> 1 or IsDisable is  null) ORDER BY cTrack";
            var result = db.Database.SqlQuery<colPreRequirTMM>(queryString).ToList();

            return result;
        }

        public List<colPreRequirTMM> GetPreRequirementcolTM()
        {
            string queryString = "SELECT cTrack ,Id FROM MasterItem WHERE cGroup='TM' and (IsDelete=0 or IsDelete is null) ORDER BY cTrack";
            var result = db.Database.SqlQuery<colPreRequirTMM>(queryString).ToList();

            return result;
        }
        public List<colPreRequircolTrack> GetPreRequirementcolTrack()
        {
            string queryString = "select Trackname from MasterTrackSet where (IsDelete=0 or IsDelete is null) and TrackSet='PreRequirements'";
            var result = db.Database.SqlQuery<colPreRequircolTrack>(queryString).ToList();

            return result;
        }
        public List<colPreRequircolStatus> GetPreRequirementcolStatus()
        {
            string queryString = "SELECT cTrack, Id FROM MasterItem WHERE cGroup = 'Status' and(IsDelete= 0 or IsDelete is null) ORDER BY cTrack";
            var result = db.Database.SqlQuery<colPreRequircolStatus>(queryString).ToList();
            return result;
        }
        public List<PermitsRequirementSetColumn> PermitsRequirementSetColumn()
        {
            string queryString = "SELECT JobTracking.JobListID, JobList.JobNumber, JobTracking.TaskHandler, JobTracking.Track, JobTracking.TrackSub, JobTracking.Comments, JobTracking.Status, JobTracking.Submitted, JobTracking.Obtained, JobTracking.Expires, jobTracking.FinalAction, JobTracking.BillState, JobTracking.AddDate, JobTracking.NeedDate, JobTracking.JobTrackingID,JobTracking.TrackSubID,JobTracking.InvOvr FROM JobTracking INNER JOIN JobList ON JobTracking.JobListID = JobList.JobListID WHERE (JobTracking.Track IN ('Inspect;', 'Permit;', 'VE Requ;')) AND (JobTracking.JobListID = 0) AND (JobTracking.IsDelete = 0 OR JobTracking.IsDelete IS NULL)";

            var result = db.Database.SqlQuery<PermitsRequirementSetColumn>(queryString).ToList();
            return result;
        }
        public List<colPreRequircolTrack> GetPermitsRequirementcolTrack()
        {
            string queryString = "select Trackname from MasterTrackSet where (IsDelete=0 or IsDelete is null) and TrackSet='Permits/Required/Inspection'";
            var result = db.Database.SqlQuery<colPreRequircolTrack>(queryString).ToList();

            return result;
        }
        public List<colBillStatus> GetPermitsRequirementcolBillStatus()
        {
            string queryString = "SELECT cTrack ,Id FROM MasterItem WHERE cGroup='Bill State' and (IsDelete=0 or IsDelete is null) ORDER BY cTrack";
            var result = db.Database.SqlQuery<colBillStatus>(queryString).ToList();
            return result;
        }
        public List<NotesComunicationSetColumn> NotesSetColumn()
        {
            string queryString = "SELECT JobTracking.JobListID,JobList.JobNumber,JobTracking.TaskHandler,JobTracking.Track,JobTracking.TrackSub, JobTracking.Comments,JobTracking.Status,JobTracking.Submitted,JobTracking.Obtained,JobTracking.Expires,JobTracking.BillState , JobTracking.AddDate, JobTracking.NeedDate,JobTracking.JobTrackingID,JobTracking.InvOvr  FROM  JobTracking INNER JOIN    JobList ON JobTracking.JobListID = JobList.JobListID where JobTracking.Track in ('Commun;','Miscel;','Time;','Reinburs;') and JobTracking.JobListID=0 and (JobTracking.IsDelete=0 or JobTracking.IsDelete is null)";

            var result = db.Database.SqlQuery<NotesComunicationSetColumn>(queryString).ToList();
            return result;
        }
        public List<colPreRequircolTrack> GetNotescolTrack()
        {
            string queryString = "select Trackname from MasterTrackSet where(IsDelete= 0 or IsDelete is null) and TrackSet = 'Notes/Communication'";
            var result = db.Database.SqlQuery<colPreRequircolTrack>(queryString).ToList();

            return result;
        }
        public List<TableVersion> GetTableVersion()
        {
            string queryString = "select * from VersionTable  union SELECT 0 as TableVersionId, '--Use Default--' as TableVersionName order by TableVersionId";
            var result = db.Database.SqlQuery<TableVersion>(queryString).ToList();

            return result;
        }
        public List<ManagerData> GetManagerDataAfterFilter(string queryString)
        {
            string Sql = "SELECT  DISTINCT JobList.JobListID, JobList.JobNumber,JobList.Clienttext, Company.CompanyID, JobList.DateAdded, JobList.Description, JobList.Handler, JobList.Borough, JobList.Address, Contacts.FirstName + ' ' + Contacts.MiddleName + ' ' + Contacts.LastName AS Contacts, Contacts.EmailAddress, Contacts.ContactsID,Company.CompanyName,JobList.ACContacts,JobList.ACEmail,JobList.OwnerName,JobList.OwnerAddress, JobList.OwnerPhone,JobList.OwnerFax,Company.CompanyNo, JobList.PMrv, IsNull(JobList.IsDisable, 0) as IsDisable, IsNull(JobList.IsInvoiceHold, 0) as IsInvoiceHold," + " jd.InvoiceType AS TypicalInvoiceType, JobList.InvoiceClient, JobList.InvoiceContact,(Select dbo.ClientName(FirstName,MiddleName,LastName) FROM Contacts WHERE ContactsId LIKE jobList.InvoiceContact ) as InvoiceContactT ,JobList.InvoiceEmailAddress, JobList.InvoiceACContacts,(Select dbo.ClientName(FirstName,MiddleName,LastName) FROM Contacts WHERE ContactsId LIKE jobList.InvoiceACContacts ) as InvoiceACContactsT,JobList.InvoiceACEmail," + "CONVERT(INT,jd.TableVersionId) AS RateVersionId," + "jd.ServRate AS ServRate, IsNull(JobList.AdminInvoice, 0) as AdminInvoice FROM  JobList LEFT OUTER JOIN Contacts ON JobList.ContactsID = Contacts.ContactsID LEFT OUTER JOIN Company ON JobList.CompanyID = Company.CompanyID LEFT OUTER JOIN JobTracking ON JobList.JobListID = JobTracking.JobListID INNER JOIN vwJobListDefaultValue jd ON JobList.JobListId=jd.JobListID WHERE (JobList.IsDelete=0 or JobList.IsDelete is null) ";

            if (!String.IsNullOrEmpty(queryString))
                Sql = Sql + queryString;

            var result = db.Database.SqlQuery<ManagerData>(Sql).ToList();

            return result;
        }

        public List<PreRequirement> GetPreRequirementDataAfterFilter(string queryString, int selectedJobListID)
        {
            var sql = "SELECT JobTracking.JobListID,JobList.JobNumber,JobTracking.TaskHandler,JobTracking.Track,JobTracking.TrackSub, JobTracking.Comments,JobTracking.Status,JobTracking.Submitted, JobTracking.Obtained,JobTracking.Expires,JobTracking.BillState , JobTracking.AddDate, JobTracking.NeedDate,     JobTracking.JobTrackingID,JobTracking.TrackSubID,JobTracking.InvOvr  FROM  JobTracking INNER JOIN JobList ON JobTracking.JobListID = JobList.JobListID where JobTracking.Track in (SELECT Trackname FROM MasterTrackSet WHERE TrackSet='PreRequirements') and JobTracking.JobListID=" + selectedJobListID + " and (JobTracking.IsDelete=0 or JobTracking.IsDelete is null)";

            sql = sql + queryString;

            var result = db.Database.SqlQuery<PreRequirement>(sql).ToList();

            return result;
        }

        public List<colTrackSubItem> GetTrackSubItem(string TrackName)
        {
            var sql = "Select ID, TrackSubName from MasterTrackSubItem WHERE  (IsDelete=0 or IsDelete IS NULL) AND TrackName='" + TrackName + "'Order by TrackSubName";

            var result = db.Database.SqlQuery<colTrackSubItem>(sql).ToList();

            return result;
        }

        public List<PermitsRequirement> GetPermitsRequirementDataAfterFilter(string queryString, int selectedJobListID)
        {
            string sql = "SELECT JobTracking.JobListID, JobList.JobNumber, JobTracking.TaskHandler, JobTracking.Track, JobTracking.TrackSub, JobTracking.Comments, JobTracking.Status,JobTracking.Submitted, JobTracking.Obtained, JobTracking.Expires, JobTracking.FinalAction, JobTracking.BillState, JobTracking.AddDate, JobTracking.NeedDate, JobTracking.JobTrackingID, JobTracking.TrackSubID,JobTracking.InvOvr FROM JobTracking INNER JOIN JobList ON JobTracking.JobListID = JobList.JobListID WHERE (JobTracking.Track IN(SELECT TrackName FROM MasterTrackSet WHERE (TrackSet = 'Permits/Required/Inspection'))) AND (JobTracking.JobListID = " + selectedJobListID + ") AND (JobTracking.IsDelete = 0 OR JobTracking.IsDelete IS NULL)";
            sql = sql + queryString;
            var result = db.Database.SqlQuery<PermitsRequirement>(sql).ToList();

            return result;
        }

        public List<NotesComunication> GetNotesComunicationDataAfterFilter(string queryString)
        {
            string sql = "SELECT JobTracking.JobListID,JobList.JobNumber,JobTracking.TaskHandler,JobTracking.Track,JobTracking.TrackSub, JobTracking.Comments, JobTracking.Status,JobTracking.Submitted, JobTracking.Obtained,JobTracking.Expires,JobTracking.BillState , JobTracking.AddDate, JobTracking.NeedDate,     JobTracking.JobTrackingID,JobTracking.TrackSubID," + "JobTracking.InvOvr," + "JobTracking.DeleteItemTimeService  FROM  JobTracking INNER JOIN JobList ON JobTracking.JobListID = JobList.JobListID where JobTracking.Track in (select Trackname from MasterTrackSet Where TrackSet='Notes/Communication') and (JobTracking.IsDelete=0 or JobTracking.IsDelete is null) ";
            sql = sql + queryString;

            var result = db.Database.SqlQuery<NotesComunication>(sql).ToList();

            return result;
        }

        public List<InvoiceTypeRate> GetInvoiceTypeRate(int companyId)
        {
            string queryString = "SELECT TypicalInvoiceType,ServRate FROM Company WHERE CompanyId=" + companyId;
            var result = db.Database.SqlQuery<InvoiceTypeRate>(queryString).ToList();

            return result;
        }

        public string AutoJobnumber()
        {
            string AutoJobnumberValue = "";
            try
            {
                //DataAccessLayer dl2 = new DataAccessLayer();

                //'Dim quiry As String = "select (Max(right(JobNumber,len(JobNumber)-charindex('-', JobNumber)))+1) from joblist where year(dateAdded) = YEAR( GETDATE() ) and cast((YEAR(getdate())%100)as NCHAR ) = Cast((LEFT(jobnumber, len(jobnumber)-charindex('-',jobNumber)-1))as NCHAR)"

                string quiry = "Select (Max(right(JobNumber,len(JobNumber)-charindex('-', JobNumber)))+1) from joblist where year(dateAdded) = YEAR( GETDATE() ) and cast((YEAR(getdate())%100)as NCHAR ) = Cast((LEFT(jobnumber,2))as NCHAR)";

                //Int16 Jb = 0;
                var Jb = db.Database.SqlQuery<Int32>(quiry).SingleOrDefault();

                //Int16 y = 0;

                if (Jb >= 0)
                {

                    quiry = "select ( YEAR( GETDATE() ) % 100 )";

                    var y = db.Database.SqlQuery<Int32>(quiry).SingleOrDefault();

                    AutoJobnumberValue = y.ToString("00") + "-" + (Jb).ToString("00");
                }
            }
            catch (Exception ex)
            {

            }
            return AutoJobnumberValue;
        }

        public Int32 GetTrackSubId(string TrackSubName, string TrackName)
        {
            var id = db.Database.SqlQuery<Int32>("select id from MasterTrackSubItem WHERE (IsDelete=0 or IsDelete IS NULL) and  TrackSubName = '" + TrackSubName + "' and TrackName = '" + TrackName + "'").SingleOrDefault();

            return id;
        }

        public Int32 GetValueMemberID(int CompanyID, string Contacts)
        {
            //string Query = "SELECT ContactsID,dbo.ClientName(FirstName, MiddleName, LastName) as ClientName FROM  Contacts WHERE CompanyID=" + CompanyID + " && ClientName='" + Contacts + "' ORDER BY FirstName";
            string Query = "Select a.ContactsID from (SELECT ContactsID,dbo.ClientName(FirstName, MiddleName, LastName) as ClientName FROM  Contacts WHERE CompanyID=" + CompanyID + ") as a where a.ClientName='" + Contacts + "'";
            var data = db.Database.SqlQuery<ContanctsClient>(Query).FirstOrDefault();
            int id = 0;
            if (!(data is null))
                id = Convert.ToInt32(data.ContactsID);
            return id;
        }

        public int Insert()
        {
            //JobList
            //string sql= "Insert into JobList (JobNumber, CompanyID, ContactsID, DateAdded, Description, Handler, Address, Borough,InvoiceClient ,InvoiceContact, InvoiceEmailAddress, InvoiceACContacts, InvoiceACEmail,IsNewRecord,OwnerName,OwnerAddress,OwnerPhone,OwnerFax,ACContacts,ACEmail,Clienttext,ContactsEmails, PMrv, RateVersionId,ServRate,AdminInvoice, IsInvoiceHold) values (@JobNumber,@CompanyID,@ContactsID,@DateAdded,@Description,@Handler,@Address,@Borough,@InvoiceClient ,@InvoiceContact,@InvoiceEmailAddress,@InvoiceACContacts, @InvoiceACEmail,@IsNewRecord,@OwnerName,@OwnerAddress,@OwnerPhone,@OwnerFax,@ACContacts,@ACEmail,@Clienttext,@ContactsEmails, @PMrv, @RateVersionId,@ServRate,@AdminInvoice, @IsInvoiceHold)";
            //using (var context = GetDbContext())
            //{
            //    context.Insurances.Add(InsuranceData);
            //    context.SaveChanges();
            //    return true;
            //}

            //int val=db.Database.ExecuteSqlCommand("insert into dbo.bcad_Site(siteName) VAlues('test0011')");
            return 0;
        }

        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose(); ;
                db = null;
            }
        }
    }
}
