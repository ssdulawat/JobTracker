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
    
    public partial class MasterTrackSubItem
    {
        public int Id { get; set; }
        public Nullable<int> TrackId { get; set; }
        public string TrackName { get; set; }
        public string TrackSubName { get; set; }
        public Nullable<decimal> nRate { get; set; }
        public string Description { get; set; }
        public string Account { get; set; }
        public Nullable<bool> IsNewRecord { get; set; }
        public Nullable<bool> IsChange { get; set; }
        public Nullable<System.DateTime> ChangeDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string CalColor { get; set; }
    }
}