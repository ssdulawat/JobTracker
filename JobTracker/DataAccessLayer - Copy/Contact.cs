//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Contact
    {
        public int ContactsID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ContactTitle { get; set; }
        public string MobilePhone { get; set; }
        public string EmailAddress { get; set; }
        public string Notes { get; set; }
        public string SpecialRiggerNUM { get; set; }
        public string MasterRiggerNUM { get; set; }
        public string SpecialSignNUM { get; set; }
        public string MasterSignNUM { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string FaxNumber { get; set; }
        public string AlternativePhone { get; set; }
        public string FieldPhone { get; set; }
        public string Pager { get; set; }
        public Nullable<bool> Accounting { get; set; }
        public Nullable<bool> IsChange { get; set; }
        public Nullable<bool> IsNewRecord { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> ChangeDate { get; set; }
    }
}
