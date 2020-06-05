﻿using DataAccessLayer.Model;
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
            string queryString = "SELECT  DISTINCT JobList.JobListID, JobList.JobNumber,JobList.Clienttext, Company.CompanyID, JobList.DateAdded AS Added, JobList.Description, JobList.Handler AS PM, JobList.Borough AS Town, JobList.Address, Contacts.FirstName + ' ' + Contacts.MiddleName + ' ' + Contacts.LastName AS Contacts, Contacts.EmailAddress, Contacts.ContactsID,   Company.CompanyName,JobList.ACContacts,JobList.ACEmail,JobList.OwnerName,JobList.OwnerAddress,JobList.OwnerPhone,JobList.OwnerFax,Company.CompanyNo, JobList.PMrv,     IsNull(JobList.IsDisable, 0) as IsDisable, IsNull(JobList.IsInvoiceHold, 0) as IsInvoiceHold, jd.InvoiceType AS TypicalInvoiceType, JobList.InvoiceClient, JobList.InvoiceContact,(Select dbo.ClientName(FirstName,MiddleName,LastName) FROM Contacts WHERE ContactsId LIKE jobList.InvoiceContact ) as InvoiceContactT ,JobList.InvoiceEmailAddress, JobList.InvoiceACContacts,(Select dbo.ClientName(FirstName,MiddleName,LastName) FROM Contacts WHERE ContactsId LIKE jobList.InvoiceACContacts ) as InvoiceACContactsT,JobList.InvoiceACEmail,CONVERT(INT,jd.TableVersionId) AS RateVersionId,jd.ServRate AS ServRate, IsNull(JobList.AdminInvoice, 0) as AdminInvoice FROM  JobList LEFT OUTER JOIN            Contacts ON JobList.ContactsID = Contacts.ContactsID LEFT OUTER JOIN      Company ON JobList.CompanyID = Company.CompanyID LEFT OUTER JOIN        JobTracking ON JobList.JobListID = JobTracking.JobListID INNER JOIN vwJobListDefaultValue jd ON JobList.JobListId=jd.JobListID     WHERE (JobList.IsDelete=0 or JobList.IsDelete is null)  AND (JobList.IsDisable = 0  OR JobList.IsDisable IS NULL) AND (JobList.IsInvoiceHold = 0 )  order by JobList.JobListID";
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
    }
}
