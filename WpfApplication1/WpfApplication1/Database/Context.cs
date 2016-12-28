﻿namespace WpfApplication1.Database
{
    using System.Data.Entity;

    public partial class Context : DbContext
    {
        public Context() : base("NetTough_Databasev3")
        { }
        public virtual DbSet<Detail> Details { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Detail>()
                .Property(e => e.KeyData)
                .IsFixedLength();
            modelBuilder.Entity<Session>()
                .Property(e => e.IP_in)
                .IsUnicode(false);
            modelBuilder.Entity<Session>()
                .Property(e => e.IP_out)
                .IsUnicode(false);
            modelBuilder.Entity<Session>()
                .HasMany(e => e.Details)
                .WithRequired(e => e.Session)
                .WillCascadeOnDelete(false);
        }
    }
}
