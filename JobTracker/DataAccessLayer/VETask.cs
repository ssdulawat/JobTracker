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
    
    public partial class VETask
    {
        public long VETaskID { get; set; }
        public string JobNumber { get; set; }
        public string PM { get; set; }
        public string TM { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<System.DateTime> CompletedDate { get; set; }
        public Nullable<long> JobID { get; set; }
    }
}
