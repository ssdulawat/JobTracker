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
    
    public partial class InvoiceJobList
    {
        public int JobListID { get; set; }
        public string JobNumber { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ContactsName { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string CompanyName { get; set; }
    }
}
