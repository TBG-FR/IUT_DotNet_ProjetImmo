using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetImmo.Core.DataAccess
{
    public class AgencyDbContextFactory : IDesignTimeDbContextFactory<AgencyDbContext>
    {

        public AgencyDbContext CreateDbContext(string[] args)
        {

            var optionsBuilder = new DbContextOptionsBuilder<AgencyDbContext>();
            optionsBuilder.UseSqlite("Data Source = db_agency.sqlite");

            return new AgencyDbContext(optionsBuilder.Options);

        }

    }
}
