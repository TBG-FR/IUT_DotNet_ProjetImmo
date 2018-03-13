using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjetImmo.Core.DataAccess
{

    /*
     * Migration (in Nu-Get console) : 'Add-Migration *MIGRATION_NAME* -Project ProjetImmo.Core -Verbose'
     */

    public class AgencyDbContext : DbContext
    {

        #region AgencyDbContext - Singleton

        private static AgencyDbContext context = null;

        public static AgencyDbContext Current
        {
            get
            {
                if (context == null)
                    throw new NullReferenceException("Database should be initiliazed");
                return context;
            }

        }

        public async static Task<AgencyDbContext> Initialize()
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

        internal AgencyDbContext(DbContextOptions options) : base(options) { }

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
        public DbSet<Models.Person> Person { get; set; }
        public DbSet<Models.Address> Address { get; set; }
        public DbSet<Models.Picture> Picture { get; set; }

        public DbSet<Models.Keyword> Keyword { get; set; }
        public DbSet<Models.Keyword> EstateKeyword { get; set; }

        public DbSet<Models.Transaction> Transaction { get; set; }
        public DbSet<Models.RentalTransaction> RentalTransaction { get; set; }
        public DbSet<Models.SaleTransaction> SaleTransaction { get; set; }

        #endregion

        #region AgencyDbContext - Foreign Keys (Links & Relations)

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ESTATE : #REFERENT [One Estate has One Referent (Person), One Referent has Many Estates]
            modelBuilder.Entity<Models.Estate>().HasOne<Models.Person>(e => e.Referent).WithMany(p => p.ManagedEstates);

            // ESTATE : #OWNER [One Estate has One Owner (Person), One Owner has Many Estates]
            modelBuilder.Entity<Models.Estate>().HasOne<Models.Person>(e => e.Owner).WithMany(p => p.OwnedEstates);

            // ESTATE : #ADDRESS [One Estate has One Address, One Address has MANY Estates] (Old : One Address has One Estate)
            modelBuilder.Entity<Models.Estate>().HasOne<Models.Address>(e => e.Address).WithMany(a => a.AddressedEstates);

            // PERSON : #ADDRESS [One Person has One Address, One Address has MANY Persons] (Old : One Address has One Person)
            modelBuilder.Entity<Models.Person>().HasOne<Models.Address>(p => p.Address).WithMany(a => a.AddressedPersons);

            // ESTATE -> PICTURES [One Estate has Many Pictures, One Picture has One Estate]
            modelBuilder.Entity<Models.Estate>().HasMany<Models.Picture>(e => e.Pictures).WithOne(p => p.RelatedEstate);

            // ESTATE <-> KEYWORDS [One Estate has Many Keywords, One Keyword has Many Estates]
            //  => EstateKeyword table (ESTATE <>- ESTATEKEYWORD -<> KEYWORDS)
            modelBuilder.Entity<Models.EstateKeyword>().HasKey(ek => new { ek.EstateID, ek.KeywordID});
            modelBuilder.Entity<Models.Estate>().HasMany<Models.EstateKeyword>(e => e.Keywords).WithOne(ek => ek.Estate).HasForeignKey(ek => ek.EstateID);
            modelBuilder.Entity<Models.Keyword>().HasMany<Models.EstateKeyword>(k => k.DescribedEstates).WithOne(ek => ek.Keyword).HasForeignKey(ek => ek.KeywordID);

            // ESTATE -> TRANSACTIONS [One Estate has Many Transactions, One Transcation has One Estate] => PERSON CAN BE NULL
            modelBuilder.Entity<Models.Estate>().HasMany<Models.Transaction>(e => e.Transactions).WithOne(t => t.RelatedEstate);//.IsOptional() -- .HasOptional()

        }

        #endregion

    }
}
