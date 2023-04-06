using BugTrackerByBenci.Controllers;
using BugTrackerByBenci.Models;
using BugTrackerByBenci.Services.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTrackerByBenci.Models.ViewModels;

namespace BugTrackerUnitTests.Controllers
{
    public class HomeControllerTests
    {
        private readonly HomeController _homeController;
        private readonly ILogger<HomeController> _logger;
        private readonly IBTProjectService _projectService;
        private readonly IBTCompanyInfoService _companyInfoService;
        private readonly IBTTicketService _ticketService;

        public HomeControllerTests()
        {
            //Dependencies
            _projectService = A.Fake<IBTProjectService>();
            _companyInfoService = A.Fake<IBTCompanyInfoService>();
            _ticketService = A.Fake<IBTTicketService>();

            //SUT
            _homeController = new HomeController(_logger, _projectService, _ticketService, _companyInfoService);
        }

        [Fact]
        public void HomeController_CompanyMembers_Returns_Success()
        {
            //Arrange
            var btUsers = A.Fake<List<BTUser>>();
            //var companyId = A.Fake<User.Identity.GetCompanyId()>
            //Act
            A.CallTo(() => _companyInfoService.GetAllMembersAsync(A<int>._)).Returns(btUsers);
            //Func<Task> result = async () => await _homeController.CompanyMembers();
            var result = _homeController.CompanyMembers();
            
            //Assert
            result.Should().BeOfType<Task<IActionResult>>();


        }

        [Fact]
        public void HomeController_Dashboard_Returns_Success()
        {
            //Arrange
            var companyID = 1;
            var dashboardViewModel = A.Fake<DashboardViewModel>();
            
            A.CallTo(() => _projectService.GetAllProjectsByCompanyIdAsync(companyID))!.Returns(dashboardViewModel.Projects);
            A.CallTo(() => _ticketService.GetAllTicketsByCompanyIdAsync(companyID))!.Returns(dashboardViewModel.Tickets);
            A.CallTo(() => _companyInfoService.GetAllMembersAsync(companyID))!.Returns(dashboardViewModel.Members);
            A.CallTo(() => _companyInfoService.GetCompanyInfoById(companyID))!.Returns(dashboardViewModel.Company);
                        
            //Act
            var result = _homeController.Dashboard();

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}
