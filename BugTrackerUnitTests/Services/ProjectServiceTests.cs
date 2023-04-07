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
using BugTrackerUnitTests.Generators;

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
            var project = ProjectGenerator.GenerateProject(1);

            //Act
            Func<Task> result = async () => await _projectService.AddNewProjectAsync(project[0]);

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
            var project = ProjectGenerator.GenerateProject(1);
            
            await _projectService.AddNewProjectAsync(project[0]);

            //Act
            var result = _projectService.GetProjectByIdAsync(1, 1);

            //Assert
            result.Should().BeOfType<Task<Project>>();
        }

        /// <summary>
        /// Adds 10 projects to inmemory DB and asserts 10
        /// </summary>
        [Fact]
        public async void GetAllProjectsByCompanyIdAsync_ReturnsListOfProjects()
        {
            //Arrange
            var projects = ProjectGenerator.GenerateProject(10);

            for (int i = 0; i < 10; i++)
            {
                await _projectService.AddNewProjectAsync(projects[i]);
            }

            //Act
            var result = await _projectService.GetAllProjectsByCompanyIdAsync(1);

            //Assert
            result.Should().BeOfType<List<Project>>();
            _context.Projects.Should().HaveCount(10);
        }

        /// <summary>
        /// Gets projects tests but no projects added
        /// </summary>
        [Fact]
        public async void GetAllProjectsByCompanyIdAsync_ReturnsNoProjects()
        {
            //Arrange - none to arrange
            
            //Act
            await _projectService.GetAllProjectsByCompanyIdAsync(1);

            //Assert
            _context.Projects.Should().HaveCount(0);
        }

        /// <summary>
        /// Tests Add user to assert true
        /// </summary>
        [Fact]
        public async void AddUserToProjectAsync_ReturnsTrue()
        {
            //Arrange - Add user and add one project
            var project = ProjectGenerator.GenerateProject(1);

            var user = new BTUser()
            {
                FirstName = "Benci",
                LastName = "Magpoc"
            };

            _context.Users.Add(user);
            _context.Add(project[0]);
            _context.SaveChanges();

            //Act
            var result = await _projectService.AddUserToProjectAsync(user.Id, 1);

            //Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests Add user to assert exception
        /// </summary>
        [Fact]
        public async void AddUserToProjectAsync_ReturnsException()
        {
            //Arrange - Add user 
            var user = new BTUser()
            {
                FirstName = "Benci",
                LastName = "Magpoc"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            //Act
            Func<Task> result = async () => await _projectService.AddUserToProjectAsync(user.Id, 1);

            //Assert
            await result.Should().ThrowAsync<NullReferenceException>();
        }

    }
}
