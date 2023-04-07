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
        private readonly ApplicationDbContext _context;
        private readonly BTProjectService _projectService;

        public ProjectServiceTests()
        {
            //Dependencies
            _userManager = A.Fake<UserManager<BTUser>>();
            _rolesService = A.Fake<IRolesService>();
            _context = DbContextGenerator.GenerateDbContext();

            //SUT
            _projectService = new BTProjectService(_context, _userManager, _rolesService);
        }

        /// <summary>
        /// Happy path for Add New Project Service
        /// </summary>
        [Fact]
        public async void AddNewProjectService_ReturnsSuccess()
        {
            //Arrange
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

            //Act
            Func<Task> result = async () => await _projectService.AddNewProjectAsync(project);

            //Assert
            await result.Should().NotThrowAsync();
            
        }

        /// <summary>
        /// Force an exception to the Add new Project Service
        /// </summary>
        [Fact]
        public async void AddNewProjectService_ReturnsException()
        {
            //Arrange to add an empty project
            var project = new Project();

            //Act
            Func<Task> result = async () => await _projectService.AddNewProjectAsync(project);

            //Assert
            await result.Should().ThrowAsync<Exception>();

        }

        /// <summary>
        /// Get Project by Id Test
        /// </summary>
        [Fact]
        public async void GetProjectByIdAsync_ReturnsProject()
        {
            //Arrange
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

            await _projectService.AddNewProjectAsync(project);

            //Act
            var result = _projectService.GetProjectByIdAsync(1, 1);

            //Assert
            result.Should().BeOfType<Task<Project>>();
        }

        [Fact]
        public async void GetAllProjectsByCompanyIdAsync_ReturnsListOfProjects()
        {
            //Arrange
            for (int i = 0; i < 10; i++)
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
                await _projectService.AddNewProjectAsync(project);
            }

            //Act
            var result = await _projectService.GetAllProjectsByCompanyIdAsync(1);

            //Assert
            result.Should().BeOfType<List<Project>>();
            
        }

        [Fact]
        public async void GetAllProjectsByCompanyIdAsync_ReturnsNoProjects()
        {
            
            //Act
            var result = await _projectService.GetAllProjectsByCompanyIdAsync(1);

            //Assert
            result.Count.Should().Be(0);
        }

    }
}
