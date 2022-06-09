using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTrackerByBenci.Data;
using BugTrackerByBenci.Extensions;
using BugTrackerByBenci.Models;
using BugTrackerByBenci.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BugTrackerByBenci.Controllers
{
    public class InvitesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTProjectService _projectService;
        private readonly IBTCompanyInfoService _companyInfoService;
        private readonly IBTInviteService _inviteService;
        private readonly IEmailSender _emailService;
        private IDataProtector _protector;

        public InvitesController(ApplicationDbContext context, 
                                UserManager<BTUser> userManager, 
                                IBTProjectService projectService, 
                                IBTCompanyInfoService companyInfoService, 
                                IBTInviteService inviteService, 
                                IDataProtectionProvider dataProtectionProvider, IEmailSender emailService)
        {
            _context = context;
            _userManager = userManager;
            _projectService = projectService;
            _companyInfoService = companyInfoService;
            _inviteService = inviteService;
            _emailService = emailService;
            _protector = dataProtectionProvider.CreateProtector("Benci.Magpoc.Bug.Tr@cker");
        }

        // GET: Invites
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Invites.Include(i => i.Company).Include(i => i.Invitee).Include(i => i.Invitor).Include(i => i.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Invites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Invites == null)
            {
                return NotFound();
            }

            var invite = await _context.Invites
                .Include(i => i.Company)
                .Include(i => i.Invitee)
                .Include(i => i.Invitor)
                .Include(i => i.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invite == null)
            {
                return NotFound();
            }

            return View(invite);
        }

        // GET: Invites/Create
        public async Task<IActionResult> Create()
        {
            int companyId = User.Identity!.GetCompanyId();

            ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(companyId),
                "Id", "Name");



            return View();
        }

        // POST: Invites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectId,InviteeId,InviteeEmail,InviteeFirstName,InviteeLastName,Message")] Invite invite)
        {
            if (ModelState.IsValid)
            {
                int companyId = User.Identity!.GetCompanyId();

                try
                {
                    Guid guid = Guid.NewGuid();

                    string token = _protector.Protect(guid.ToString());
                    string email = _protector.Protect(invite.InviteeEmail!);
                    string company = _protector.Protect(companyId.ToString());

                    string? callbackUrl = Url.Action("ProcessInvite", "Invites", new { token, email, company },
                        protocol: Request.Scheme);
                    
                    string body = $@"{invite.Message} <br />
                              Please join my Company. <br />
                              Click the following link to join our team. <br />
                              <a href=""{callbackUrl}"">COLLABORATE</a>";

                    string? destination = invite.InviteeEmail;

                    Company btCompany = await _companyInfoService.GetCompanyInfoById(companyId);
                    string subject = $" Benci's Bug Tracker: {btCompany.Name} Invite";

                    await _emailService.SendEmailAsync(destination, subject, body);

                    // Save invite in the DB
                    invite.CompanyToken = guid;
                    invite.CompanyId = companyId;
                    invite.InviteDate = DateTime.UtcNow;
                    invite.InvitorId = _userManager.GetUserId(User);
                    invite.IsValid = true;

                    await _inviteService.AddNewInviteAsync(invite); 
                
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    throw;
                }

                _context.Add(invite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Description", invite.CompanyId);
            ViewData["InviteeId"] = new SelectList(_context.Users, "Id", "Id", invite.InviteeId);
            ViewData["InvitorId"] = new SelectList(_context.Users, "Id", "Id", invite.InvitorId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Description", invite.ProjectId);
            return View(invite);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessInvite(string token, string email, string company)
        {
            return RedirectToAction(nameof(Index));
        }

    }
}
