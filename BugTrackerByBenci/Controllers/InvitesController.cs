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
using BugTrackerByBenci.Models.Enums;
using BugTrackerByBenci.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = nameof(BTRoles.Admin))]
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
        [Authorize(Roles = nameof(BTRoles.Admin))]
        public async Task<IActionResult> Create([Bind("Id,ProjectId,InviteeId,InviteeEmail,InviteeFirstName,InviteeLastName,Message")] Invite invite)
        {
            ModelState.Remove("InvitorId");

            int companyId = User.Identity!.GetCompanyId();

            if (ModelState.IsValid)
            {
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
                
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    throw;
                }
            }

            ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(companyId),
                "Id", "Name");
            return View(invite);
        }

        [HttpGet]
        public async Task<IActionResult> ProcessInvite(string token, string email, string company)
        {
            if (token == null)
            {
                return NotFound();
            }

            Guid companyToken = Guid.Parse(_protector.Unprotect(token));
            string inviteeEmail = _protector.Unprotect(email);
            int companyId = int.Parse(_protector.Unprotect(company));

            try
            {
                Invite invite = await _inviteService.GetInviteAsync(companyToken, inviteeEmail, companyId);

                if (invite != null)
                {
                    return View(invite);
                }

                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
