//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ITS.DAL.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_MachineGroup
    {
        public int ID { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> InsertDate { get; set; }
        public Nullable<int> InsertUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdateUser { get; set; }
    }
}
