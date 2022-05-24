using BugTrackerByBenci.Models;
using BugTrackerByBenci.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BugTrackerByBenci.Data.Data_Utility
{
    public class DataUtility
    {
        //private static int company1Id;
        //private static int company2Id;
        //private static int company3Id;
        //private static int company4Id;
        //private static int company5Id;

        //private static int portfolioId;
        //private static int blogId;
        //private static int bugtrackerId;
        //private static int movieId;
        //private static int addressbookId;


        public static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }    

        public static string BuildConnectionString(string databaseUrl)
        {
            // Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');
            // Provides a simple way to create and manage the contents of connection strings used by the NpgSqlConnection class. 
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };
            return builder.ToString();
        }

        //public static async Task ManageDataAsync(IServiceProvider svcProvider)
        //{
        //    var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
        //    var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    var userManagerSvc = svcProvider.GetRequiredService<UserManager<BTUser>>();
        //    await dbContextSvc.Database.MigrateAsync();

        //    //await SeedRolesAsync(roleManagerSvc);
        //    //await SeedDefaultCompaniesAsync(dbContextSvc);
        //    //await SeedDefaultUsersAsync(userManagerSvc);
        //    //await SeedDemoUsersAsync(userManagerSvc);
        //    //await SeedDefaultTicketTypesAsync(dbContextSvc);
        //    //await SeedDefaultTicketStatusesAsync(dbContextSvc);
        //    //await SeedDefaultProjectsAsync(dbContextSvc);
        //    //await SeedDefaultProjectPrioritiesAsync(dbContextSvc);
        //    //await SeedDefaultTicketsAsync(dbContextSvc, userManagerSvc);
        //    //await SeedDefaultNotificationTypesAsync(dbContextSvc);

        //}

        //public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        //{
        //    //Seed Roles
        //    await roleManager.CreateAsync(new IdentityRole(nameof(BTRoles.Admin)));
        //    await roleManager.CreateAsync(new IdentityRole(nameof(BTRoles.ProjectManager)));
        //    await roleManager.CreateAsync(new IdentityRole(nameof(BTRoles.Developer)));
        //    await roleManager.CreateAsync(new IdentityRole(nameof(BTRoles.Submitter)));
        //    await roleManager.CreateAsync(new IdentityRole(nameof(BTRoles.DemoUser)));
        //}

        //public static async Task SeedDefaultCompaniesAsync(ApplicationDbContext context)
        //{

        //}

        //public static async Task SeedDefaultUsersAsync(UserManager<BTUser> userManager)
        //{

        //}

        //public static async Task SeedDemoUsersAsync(UserManager<BTUser> userManager)
        //{

        //}

        //public static async Task SeedDefaultProjectsAsync(ApplicationDbContext context)
        //{

        //}

        //public static async Task SeedDefaultProjectPrioritiesAsync(ApplicationDbContext context)
        //{

        //}

        //public static async Task SeedDefaultTicketStatusesAsync(ApplicationDbContext context)
        //{

        //}

        //public static async Task SeedDefaultTicketTypesAsync(ApplicationDbContext context)
        //{

        //}

        //public static async Task SeedDefaultTicketsAsync(ApplicationDbContext context, UserManager<BTUser> userManager)
        //{

        //}

        //public static async Task SeedDefaultNotificationTypesAsync(ApplicationDbContext context)
        //{

        //}

    }
}
