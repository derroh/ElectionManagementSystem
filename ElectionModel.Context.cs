﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElectionManagementSystem
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ElectionManagementSystemEntities : DbContext
    {
        public ElectionManagementSystemEntities()
            : base("name=ElectionManagementSystemEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Ballot> Ballots { get; set; }
        public virtual DbSet<ElectionCandidate> ElectionCandidates { get; set; }
        public virtual DbSet<ElectionPosition> ElectionPositions { get; set; }
        public virtual DbSet<Election> Elections { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
