using BugTrackerByBenci.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BugTrackerByBenci.Extensions;
using BugTrackerByBenci.Models.ChartModels;
using BugTrackerByBenci.Models.Enums;
using BugTrackerByBenci.Models.ViewModels;
using BugTrackerByBenci.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace BugTrackerByBenci.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBTProjectService _projectService;
        private readonly IBTCompanyInfoService _companyInfoService;
        private readonly IBTTicketService _ticketService;
        public HomeController(ILogger<HomeController> logger, 
                                IBTProjectService projectService, 
                                IBTTicketService ticketService, 
                                IBTCompanyInfoService companyInfoService)
        {
            _logger = logger;
            _projectService = projectService;
            _ticketService = ticketService;
            _companyInfoService = companyInfoService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> CompanyMembers()
        {
            int companyId = User.Identity!.GetCompanyId();
            List<BTUser> btUsers = await _companyInfoService.GetAllMembersAsync(companyId);

            return View(btUsers);
        }
        public async Task<IActionResult> Dashboard()
        {
            DashboardViewModel model = new DashboardViewModel();
            int companyId = User.Identity!.GetCompanyId();

            model.Projects = await _projectService.GetAllProjectsByCompanyIdAsync(companyId);
            model.Tickets = await _ticketService.GetAllTicketsByCompanyIdAsync(companyId);
            model.Members = await _companyInfoService.GetAllMembersAsync(companyId);
            model.Company = await _companyInfoService.GetCompanyInfoById(companyId);

            return View(model);
        }
        public IActionResult LandingPage()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> PlotlyBarChart()
        {
            PlotlyBarData plotlyData = new();
            List<PlotlyBar> barData = new();

            int companyId = User.Identity!.GetCompanyId();

            List<Project> projects = await _projectService.GetAllProjectsByCompanyIdAsync(companyId);

            //Bar One
            PlotlyBar barOne = new()
            {
                X = projects.Select(p => p.Name).ToArray()!,
                Y = projects.SelectMany(p => p.Tickets).GroupBy(t => t.ProjectId).Select(g => g.Count()).ToArray(),
                Name = "Tickets",
                Type = "bar"
            };

            //Bar Two
            PlotlyBar barTwo = new()
            {
                X = projects.Select(p => p.Name).ToArray()!,
                Y = projects.Select(async p => (await _projectService.GetProjectMembersByRoleAsync(p.Id, nameof(BTRoles.Developer))).Count).Select(c => c.Result).ToArray(),
                Name = "Developers",
                Type = "bar"
            };

            barData.Add(barOne);
            barData.Add(barTwo);

            plotlyData.Data = barData;

            return Json(plotlyData);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}