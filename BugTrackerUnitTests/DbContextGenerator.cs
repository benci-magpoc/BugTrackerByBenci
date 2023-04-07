using BugTrackerByBenci.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTrackerUnitTests
{
    public static class DbContextGenerator
    {
        public static ApplicationDbContext GenerateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

            return new ApplicationDbContext(options);
        }
    }
}
