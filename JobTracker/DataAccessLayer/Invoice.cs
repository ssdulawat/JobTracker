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
    
    public partial class Invoice
    {
        public int InvoiceID { get; set; }
        public Nullable<int> JobListID { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceFileName { get; set; }
        public byte[] InvoiceFile { get; set; }
        public string InvoiceFileType { get; set; }
        public string Comments { get; set; }
        public Nullable<bool> UploadFile { get; set; }
        public Nullable<bool> IsNewRecord { get; set; }
        public Nullable<bool> IsChange { get; set; }
        public Nullable<System.DateTime> ChangeDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
