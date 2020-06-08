using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class ManagerData
    {
        public int? JobListID { get; set; }
        public string JobNumber { get; set; }
        public string Clienttext { get; set; }
        public int? CompanyID { get; set; }
        public DateTime? DateAdded { get; set; }
        public string Description { get; set; }
        public string PM { get; set; }
        public string Handler { get; set; }
        public string Borough { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Contacts { get; set; }
        public string EmailAddress { get; set; }
        public int? ContactsID { get; set; }
        public string CompanyName { get; set; }
        public string ACContacts { get; set; }
        public string ACEmail { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerFax { get; set; }
        public string CompanyNo { get; set; }
        public string PMrv { get; set; }
        public bool IsDisable { get; set; }
        public bool IsInvoiceHold { get; set; }
        public string TypicalInvoiceType { get; set; }
        public string InvoiceType { get; set; }
        public int? InvoiceClient { get; set; }
        public string InvoiceContact { get; set; }
        public string InvoiceContactT { get; set; }
        public string InvoiceEmailAddress { get; set; }
        public string InvoiceACContacts { get; set; }
        public string InvoiceACContactsT { get; set; }
        public string InvoiceACEmail { get; set; }
        public int? RateVersionId { get; set; }
        public decimal ServRate { get; set; }
        public bool AdminInvoice { get; set; }


    }

    public class PreRequirement
    {
        public string JobNumber { get; set; }
        public string TaskHandler { get; set; }
        public string Track { get; set; }
        public string TrackSub { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public DateTime? Submitted { get; set; }
        public DateTime? Obtained { get; set; }

        public DateTime? Expires { get; set; }
        public string BillState { get; set; }
        public DateTime? AddDate { get; set; }
        public DateTime? NeedDate { get; set; }
        public int? JobTrackingID { get; set; }
        public int? TrackSubID { get; set; }
        public string InvOvr { get; set; }
    }
    public class PermitsRequirement
    {
        public string TaskHandler { get; set; }
        public string Track { get; set; }
        public string TrackSub { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public DateTime? Submitted { get; set; }
        public DateTime? Obtained { get; set; }
        public DateTime? Expires { get; set; }
        public string FinalAction { get; set; }
        public string BillState { get; set; }
        public DateTime? AddDate { get; set; }
        public string InvOvr { get; set; }

    }
    public class NotesComunication
    {
        public string TaskHandler { get; set; }
        public string Track { get; set; }
        public string TrackSub { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public string BillState { get; set; }
        public DateTime? AddDate { get; set; }
        public string InvOvr { get; set; }

    }

    public class ManagerSetColumn
    {

        public int? JobListID { get; set; }
        public string JobNumber { get; set; }
        public string Clienttext { get; set; }
        public int? CompanyID { get; set; }
        public DateTime? DateAdded { get; set; }
        public string Description { get; set; }
        public string PM { get; set; }
        public string Handler { get; set; }
        public string Borough { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Contacts { get; set; }
        public string EmailAddress { get; set; }
        public int? ContactsID { get; set; }
        public string CompanyName { get; set; }
        public string ACContacts { get; set; }
        public string ACEmail { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerFax { get; set; }
        public string CompanyNo { get; set; }
        public string PMrv { get; set; }
        public bool IsDisable { get; set; }
        public bool IsInvoiceHold { get; set; }
        public string TypicalInvoiceType { get; set; }
        public string InvoiceType { get; set; }
        public int? InvoiceClient { get; set; }
        public string InvoiceContact { get; set; }
        public string InvoiceContactT { get; set; }
        public string InvoiceEmailAddress { get; set; }
        public string InvoiceACContacts { get; set; }
        public string InvoiceACContactsT { get; set; }
        public string InvoiceACEmail { get; set; }
        public int? RateVersionId { get; set; }
        public decimal? ServRate { get; set; }
        public bool? AdminInvoice { get; set; }
    }

    public class cbxClientM
    {
        public string CompanyName { get; set; }
        public int? CompanyID { get; set; }
        public int? TableVersionId { get; set; }
    }
    public class colPMM
    {
        public string cTrack { get; set; }
        public int? Id { get; set; }
    }

    public class PreRequirementSetColumn
    {
        public int? JobListID { get; set; }
        public string JobNumber { get; set; }
        public string TaskHandler { get; set; }
        public string Track { get; set; }
        public string TrackSub { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public DateTime? Submitted { get; set; }
        public DateTime? Obtained { get; set; }

        public DateTime? Expires { get; set; }
        public string BillState { get; set; }
        public DateTime? AddDate { get; set; }
        public DateTime? NeedDate { get; set; }
        public int? JobTrackingID { get; set; }
        public int? TrackSubID { get; set; }
        public string InvOvr { get; set; }
    }
    public class colPreRequirTMM
    {
        public string cTrack { get; set; }
        public int? Id { get; set; }
    }
    public class colPreRequircolStatus
    {
        public string cTrack { get; set; }
        public int? Id { get; set; }
    }
    public class colPreRequircolTrack
    {
        public string Trackname { get; set; }
    }
    public class PermitsRequirementSetColumn
    {
        public int? JobListID { get; set; }
        public string TaskHandler { get; set; }
        public string JobNumber { get; set; }
        public string Track { get; set; }
        public string TrackSub { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public DateTime? Submitted { get; set; }
        public DateTime? Obtained { get; set; }
        public DateTime? Expires { get; set; }
        public string FinalAction { get; set; }
        public string BillState { get; set; }
        public DateTime? AddDate { get; set; }
        public DateTime? NeedDate { get; set; }
        public int? JobTrackingID { get; set; }
        public int? TrackSubID { get; set; }
        public string InvOvr { get; set; }

    }
    public class colBillStatus
    {
        public string cTrack { get; set; }
        public int? Id { get; set; }
    }

    public class NotesComunicationSetColumn
    {
        public int? JobListID { get; set; }
        public string JobNumber { get; set; }
        public string TaskHandler { get; set; }
        public string Track { get; set; }
        public string TrackSub { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public DateTime? Submitted { get; set; }
        public DateTime? Obtained { get; set; }
        public DateTime? Expires { get; set; }
        public string BillState { get; set; }
        public DateTime? AddDate { get; set; }
        public DateTime? NeedDate { get; set; }
        public int? JobTrackingID { get; set; }
        public string InvOvr { get; set; }

    }
}

