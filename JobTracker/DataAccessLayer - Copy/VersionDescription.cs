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
    
    public partial class VersionDescription
    {
        public int VersionDescId { get; set; }
        public Nullable<int> TableVersionId { get; set; }
        public Nullable<int> MasterTrackSubItemId { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<int> Id { get; set; }
        public string TrackSet { get; set; }
        public string TrackName { get; set; }
        public string Description { get; set; }
        public string CalColor { get; set; }
        public string Account { get; set; }
        public string TrackSubName { get; set; }
    }
}
