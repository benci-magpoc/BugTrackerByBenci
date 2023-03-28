using BugTrackerByBenci.Data;
using BugTrackerByBenci.Models;
using BugTrackerByBenci.Services;
using BugTrackerByBenci.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace BugTrackerUnitTests.Services
{
    public class ProjectServiceTests
    {
        private readonly UserManager<BTUser> _userManager;
        private readonly IRolesService _rolesService;

        public ProjectServiceTests()
        {
            _userManager = A.Fake<UserManager<BTUser>>();
            _rolesService = A.Fake<IRolesService>();
        }

        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Fake Database")
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            
            if(await databaseContext.Projects.CountAsync() < 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Add(
                        new Project()
                        {
                            CompanyId = 1,
                            Name = "Build a Personal Porfolio",
                            Description = "Single page html, css & javascript page.  Serves as a landing page for candidates and contains a bio and links to all applications and challenges.",
                            Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                            StartDate = DateTime.SpecifyKind(new DateTime(2021, 8, 20), DateTimeKind.Utc),
                            EndDate = DateTime.SpecifyKind(new DateTime(2021, 8, 20).AddMonths(1), DateTimeKind.Utc),
                            ProjectPriorityId = 1
                        });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }


        [Fact]
        public async void Add_New_Project_Service_ReturnsSuccess()
        {
            var project = new Project()
            {
                CompanyId = 1,
                Name = "Build a Personal Porfolio",
                Description = "Single page html, css & javascript page.  Serves as a landing page for candidates and contains a bio and links to all applications and challenges.",
                Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                StartDate = DateTime.SpecifyKind(new DateTime(2021, 8, 20), DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(new DateTime(2021, 8, 20).AddMonths(1), DateTimeKind.Utc),
                ProjectPriorityId = 0
            };
            var dbContext = await GetDbContext();
            
            var projectService = new BTProjectService(dbContext, _userManager, _rolesService);
            
            //A.CallTo(() => projectService.AddNewProjectAsync(project)).MustHaveHappened();
            
            //Act
            var result = projectService.AddNewProjectAsync(project);

            //Assert
            result.Wait();
            result.Should().NotBeNull();
            
        }


    }
}
