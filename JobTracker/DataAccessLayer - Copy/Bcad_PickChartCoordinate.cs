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
    
    public partial class Bcad_PickChartCoordinate
    {
        public int PickChartCoordi_ID { get; set; }
        public Nullable<int> ProjectNameID { get; set; }
        public Nullable<int> CraneCoordi_ID { get; set; }
        public Nullable<double> Coordi_X { get; set; }
        public Nullable<double> Coordi_Y { get; set; }
        public Nullable<double> Coordi_Z { get; set; }
        public string Pick { get; set; }
        public Nullable<double> PickWaight { get; set; }
        public Nullable<double> Capacity { get; set; }
        public Nullable<double> Radius { get; set; }
    }
}
