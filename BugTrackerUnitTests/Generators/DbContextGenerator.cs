using BugTrackerByBenci.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTrackerUnitTests.Generators
{
    public static class DbContextGenerator
    {
        /// <summary>
        /// Function to generate an in memory database
        /// </summary>
        /// <returns>ApplicationDbContext</returns>
        public static ApplicationDbContext GenerateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

            return new ApplicationDbContext(options);
        }
    }
}
