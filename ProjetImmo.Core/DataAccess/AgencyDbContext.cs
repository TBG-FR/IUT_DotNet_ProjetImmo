using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjetImmo.Core.DataAccess
{
    class AgencyDbContext : DbContext
    {

        #region AgencyDbContext - Singleton

        private static AgencyDbContext context = null;

        public async static Task<AgencyDbContext> getContext()
        {

            if (context == null) // If the context doesn't exist, create it
            {
                context = new AgencyDbContext(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "db_agency.sqlite"));

                await context.Database.MigrateAsync(); // Do a migration at context creation
            }

            return context;

        }

        #endregion

        #region AgencyDbContext - DataBasePath

        public string DatabasePath { get; }

        private AgencyDbContext(String databasePath)
        {
            DatabasePath = databasePath;
        }

        #endregion

        #region AgencyDbContext - Configuration & Options

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Filename={DatabasePath}" /*, x => x.SuppressForeignKeyEnforcement()*/);
        }

        #endregion

        #region AgencyDbContext - Tables

        public DbSet<Models.Estate> Estate { get; set; }
        public DbSet<Models.Estate> Person { get; set; }

        #endregion
        
        #region AgencyDbContext - Foreign Key (Links & Relations)

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ESTATE : #REFERENT
            modelBuilder.Entity<Models.Estate>().HasOne<Models.Person>(e => e.Referent).WithMany(p => p.ManagedEstates);

            // ESTATE : #OWNER
            modelBuilder.Entity<Models.Estate>().HasOne<Models.Person>(e => e.Owner).WithMany(p => p.OwnedEstates);

            // ESTATE : #ADDRESS
            modelBuilder.Entity<Models.Estate>().HasOne<Models.Address>(e => e.Address).WithOne(a => a.AddressedEstate);

            // PERSON : #ADDRESS
            modelBuilder.Entity<Models.Person>().HasOne<Models.Address>(p => p.Address).WithOne(a => a.AddressedPerson);
        }

        #endregion

    }
}
