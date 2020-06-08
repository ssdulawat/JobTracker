using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Manager
    {
        TestVariousInfoEntities db = new TestVariousInfoEntities();

        public List<ManagerData> GetManagerData()
        {
            string queryString = "SELECT  DISTINCT JobList.JobListID, JobList.JobNumber,JobList.Clienttext, Company.CompanyID, JobList.DateAdded AS Added, JobList.Description, JobList.Handler AS PM, JobList.Borough AS Town, JobList.Address, Contacts.FirstName + ' ' + Contacts.MiddleName + ' ' + Contacts.LastName AS Contacts, Contacts.EmailAddress, Contacts.ContactsID,Company.CompanyName,JobList.ACContacts,JobList.ACEmail,JobList.OwnerName,JobList.OwnerAddress,JobList.OwnerPhone,JobList.OwnerFax,Company.CompanyNo, JobList.PMrv,     IsNull(JobList.IsDisable, 0) as IsDisable, IsNull(JobList.IsInvoiceHold, 0) as IsInvoiceHold, jd.InvoiceType AS TypicalInvoiceType, JobList.InvoiceClient, JobList.InvoiceContact,(Select dbo.ClientName(FirstName,MiddleName,LastName) FROM Contacts WHERE ContactsId LIKE jobList.InvoiceContact ) as InvoiceContactT ,JobList.InvoiceEmailAddress, JobList.InvoiceACContacts,(Select dbo.ClientName(FirstName,MiddleName,LastName) FROM Contacts WHERE ContactsId LIKE jobList.InvoiceACContacts ) as InvoiceACContactsT,JobList.InvoiceACEmail,CONVERT(INT,jd.TableVersionId) AS RateVersionId,jd.ServRate AS ServRate, IsNull(JobList.AdminInvoice, 0) as AdminInvoice FROM  JobList LEFT OUTER JOIN            Contacts ON JobList.ContactsID = Contacts.ContactsID LEFT OUTER JOIN      Company ON JobList.CompanyID = Company.CompanyID LEFT OUTER JOIN        JobTracking ON JobList.JobListID = JobTracking.JobListID INNER JOIN vwJobListDefaultValue jd ON JobList.JobListId=jd.JobListID     WHERE (JobList.IsDelete=0 or JobList.IsDelete is null)  AND (JobList.IsDisable = 0  OR JobList.IsDisable IS NULL) AND (JobList.IsInvoiceHold = 0 )  order by JobList.JobListID";
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
            string queryString = "SELECT top 5 JobList.JobListID, JobList.JobNumber,JobList.Clienttext, Company.CompanyID, JobList.DateAdded, JobList.Description, JobList.Handler, JobList.Borough, JobList.Address, Contacts.FirstName + ' ' + Contacts.MiddleName + ' ' + Contacts.LastName AS Contacts, Contacts.EmailAddress, Contacts.ContactsID,   Company.CompanyName,JobList.ACContacts,JobList.ACEmail,JobList.OwnerName,JobList.OwnerAddress,JobList.OwnerPhone,JobList.OwnerFax,Company.CompanyNo, JobList.PMrv,  IsNull(JobList.IsDisable, 0) as IsDisable,IsNull(JobList.IsInvoiceHold, 0) as IsInvoiceHold,JobList.TypicalInvoiceType,JobList.InvoiceClient,JobList.ServRate,IsNull(JobList.AdminInvoice, 0) as AdminInvoice FROM  JobList LEFT OUTER JOIN  Contacts ON JobList.ContactsID = Contacts.ContactsID LEFT OUTER JOIN      Company ON JobList.CompanyID = Company.CompanyID LEFT OUTER JOIN  JobTracking ON JobList.JobListID = JobTracking.JobListID WHERE (JobList.IsDelete=0 or JobList.IsDelete is null)";

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
        public List<colPreRequirTMM> GetPreRequirementcolTM()
        {
            string queryString = "SELECT cTrack ,Id FROM MasterItem WHERE cGroup='TM' and (IsDelete=0 or IsDelete is null) AND (isDisable <> 1 or IsDisable is  null) ORDER BY cTrack";
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

    }
}
