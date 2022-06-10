using BugTrackerByBenci.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BugTrackerByBenci.Extensions;
using BugTrackerByBenci.Models.ViewModels;
using BugTrackerByBenci.Services.Interfaces;

namespace BugTrackerByBenci.Controllers
{
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
            //DashboardViewModel model = new DashboardViewModel();
            //int companyId = User.Identity!.GetCompanyId();

            //model.Projects = await _projectService.GetAllProjectsByCompanyIdAsync(companyId);
            //model.Tickets = await _ticketService.GetAllTicketsByCompanyIdAsync(companyId);
            //model.Members = await _companyInfoService.GetAllMembersAsync(companyId);
            //model.Company = await _companyInfoService.GetCompanyInfoById(companyId);

            return View();
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
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}