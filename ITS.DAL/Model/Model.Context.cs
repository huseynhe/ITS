﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ITSEntities : DbContext
    {
        public ITSEntities()
            : base("name=ITSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_AccessRight> tbl_AccessRight { get; set; }
        public virtual DbSet<tbl_BusinessCenter> tbl_BusinessCenter { get; set; }
        public virtual DbSet<tbl_CareTracking> tbl_CareTracking { get; set; }
        public virtual DbSet<tbl_CareTrackingDetail> tbl_CareTrackingDetail { get; set; }
        public virtual DbSet<tbl_Country> tbl_Country { get; set; }
        public virtual DbSet<tbl_Employee> tbl_Employee { get; set; }
        public virtual DbSet<tbl_Machine> tbl_Machine { get; set; }
        public virtual DbSet<tbl_MachineGroup> tbl_MachineGroup { get; set; }
        public virtual DbSet<tbl_Person> tbl_Person { get; set; }
        public virtual DbSet<tbl_Region> tbl_Region { get; set; }
        public virtual DbSet<tbl_Type> tbl_Type { get; set; }
        public virtual DbSet<tbl_User> tbl_User { get; set; }
    }
}
