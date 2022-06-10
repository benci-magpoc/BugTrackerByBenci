using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTrackerByBenci.Data;
using BugTrackerByBenci.Extensions;
using BugTrackerByBenci.Models;
using BugTrackerByBenci.Models.Enums;
using BugTrackerByBenci.Models.ViewModels;
using BugTrackerByBenci.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BugTrackerByBenci.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTTicketService _ticketService;
        private readonly IBTProjectService _projectService;
        private readonly IRolesService _rolesService;
        private readonly IBTFileService _fileService;
        private readonly ITicketHistoryService _ticketHistoryService;
        private readonly IBTNotificationService _notificationService;
        private readonly IBTLookupService _lookupService;
        public TicketsController(ApplicationDbContext context, 
                                    UserManager<BTUser> userManager, 
                                    IBTTicketService ticketService, 
                                    IBTProjectService projectService, 
                                    IRolesService rolesService, 
                                    IBTFileService fileService, 
                                    ITicketHistoryService ticketHistoryService, 
                                    IBTNotificationService notificationService, IBTLookupService lookupService)
        {
            _context = context;
            _userManager = userManager;
            _ticketService = ticketService;
            _projectService = projectService;
            _rolesService = rolesService;
            _fileService = fileService;
            _ticketHistoryService = ticketHistoryService;
            _notificationService = notificationService;
            _lookupService = lookupService;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            int companyId = User.Identity!.GetCompanyId();

            List<Ticket> tickets = await _ticketService.GetAllTicketsByCompanyIdAsync(companyId); 

            return View(tickets);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicketAttachment([Bind("Id,FormFile,Description,TicketId")] TicketAttachment ticketAttachment)
        {
            string statusMessage;

            if (ModelState.IsValid && ticketAttachment.FormFile != null)
            {
                ticketAttachment.FileData = await _fileService.ConvertFileToByteArrayAsync(ticketAttachment.FormFile);
                ticketAttachment.FileName = ticketAttachment.FormFile.FileName;
                ticketAttachment.FileContentType = ticketAttachment.FormFile.ContentType;

                ticketAttachment.Created = DateTime.UtcNow;
                ticketAttachment.UserId = _userManager.GetUserId(User);

                await _ticketService.AddTicketAttachmentAsync(ticketAttachment);
                statusMessage = "Success: New attachment added to Ticket.";
            }
            else
            {
                statusMessage = "Error: Invalid data.";

            }

            return RedirectToAction("Details", new { id = ticketAttachment.TicketId, message = statusMessage });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicketComment([Bind("Id,Comment,Created,TicketId,UserId")] TicketComment ticketComment)
        {
            if (ModelState.IsValid)
            {

                _context.Add(ticketComment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Tickets", new { Id = ticketComment.Id });
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComment.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticketComment.UserId);
            return RedirectToAction("Details", "Tickets", new { Id = ticketComment.Id });
        }
        public async Task<IActionResult> ArchivedTickets()
        {
            int companyId = User.Identity!.GetCompanyId();

            List<Ticket> tickets = await _ticketService.GetArchivedTicketsByCompanyIdAsync(companyId);

            return View(tickets);
        }
        //GET: Assign Tickets 
        public async Task<IActionResult> AssignDeveloper(int? id)
        {
            int companyId = User.Identity!.GetCompanyId();

            List<BTUser> developers = await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.Developer), companyId);

            TicketDeveloperViewModel model = new();

            model.Ticket = await _ticketService.GetTicketByIdAsync(id!.Value);

            if (model.Ticket != null)
            {
                model.DeveloperList = new SelectList(developers, "Id", "FullName", model.Ticket.DeveloperUserId);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Post: Assign Developer
        public async Task<IActionResult> AssignDeveloper(TicketDeveloperViewModel model)
        {
            //BTUser btUser = await _userManager.GetUserAsync(User);
            //Ticket? oldTicket = await _ticketService.GetTicketAsNoTrackingAsync(model.Ticket!.Id);

            //if (!string.IsNullOrEmpty(model.DeveloperId))
            //{
            //    await _ticketService.AddDeveloperToTicketAsync(model.DeveloperId!, model.Ticket!.Id);
            //    return RedirectToAction(nameof(Details), new { id = model.Ticket!.Id });
            //}
            if (model.DeveloperId != null)
            {
                BTUser btUser = await _userManager.GetUserAsync(User);
                //oldTicket
                Ticket? oldTicket = await _ticketService.GetTicketAsNoTrackingAsync(model.Ticket!.Id);
                try
                {
                    await _ticketService.AddDeveloperToTicketAsync(model.DeveloperId, model.Ticket.Id);
                }
                catch (Exception)
                {
                    throw;
                }
                //newTicket
                Ticket? newTicket = await _ticketService.GetTicketAsNoTrackingAsync(model.Ticket.Id);
                // Add History
                await _ticketHistoryService.AddHistoryAsync(oldTicket, newTicket, btUser.Id);
                //Send Notifications
                //Notify Developer
                if (model.Ticket.DeveloperUserId != null)
                {
                    Notification devNotification = new()
                    {
                        TicketId = model.Ticket.Id,
                        NotificationTypeId = (await _lookupService.LookupNotificationTypeIdAsync(nameof(BTNotificationTypes.Ticket)))!.Value,
                        Title = "Ticket Updated",
                        Message = $"Ticket: {model.Ticket.Title}, was updated by {btUser.FullName}",
                        Created = DateTime.UtcNow,
                        SenderId = btUser.Id,
                        RecipientId = model.Ticket.DeveloperUserId
                    };
                    await _notificationService.AddNotificationAsync(devNotification);
                    await _notificationService.SendEmailNotificationAsync(devNotification, "Ticket Updated");
                }

                return RedirectToAction(nameof(Details), new { id = model.Ticket?.Id });
            }
            return RedirectToAction(nameof(Details), new { id = model.Ticket!.Id });
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            Ticket ticket = await _ticketService.GetTicketByIdAsync(id.Value);
            
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            int companyId = User.Identity!.GetCompanyId();
            string userId = _userManager.GetUserId(User);

            if (User.IsInRole(nameof(BTRoles.Admin)))
            {
                ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(companyId), "Id", "Name");
            }
            else
            {
                ViewData["ProjectId"] = new SelectList(await _projectService.GetUserProjectsAsync(userId), "Id", "Name");
            }

            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");

            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Created,ProjectId,TicketPriorityId,TicketTypeId")] Ticket ticket)
        {
            ModelState.Remove("SubmitterUserId");

            if (ModelState.IsValid)
            {
                // Assign Values
                ticket.SubmitterUserId = _userManager.GetUserId(User);
                ticket.Created = DateTime.UtcNow;
                ticket.TicketStatusId = (await _context.TicketStatuses.FirstOrDefaultAsync(t => t.Name == "New"))!.Id;

                await _ticketService.AddNewTicketAsync(ticket);
                // TODO: Add Ticket History
                Ticket? newTicket = await _ticketService.GetTicketAsNoTrackingAsync(ticket.Id);
                await _ticketHistoryService.AddHistoryAsync(null, newTicket, ticket.SubmitterUserId);

                // TODO: Add Ticket Notification
                return RedirectToAction(nameof(Index));
            }
            // Reset values in case it didn't make valid on modelstate
            int companyId = User.Identity!.GetCompanyId();
            string userId = _userManager.GetUserId(User);

            if (User.IsInRole(nameof(BTRoles.Admin)))
            {
                ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(companyId), "Id", "Name");
            }
            else
            {
                ViewData["ProjectId"] = new SelectList(await _projectService.GetUserProjectsAsync(userId), "Id", "Name");
            }
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            Ticket ticket = await _ticketService.GetTicketByIdAsync(id.Value);

            if (ticket == null)
            {
                return NotFound();
            }

            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Created,Updated,ProjectId,SubmitterUserId,TicketPriorityId,TicketStatusId,TicketTypeId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);
                Ticket? oldTicket = await _ticketService.GetTicketAsNoTrackingAsync(ticket.Id);

                try
                {
                    ticket.SubmitterUserId = ticket.SubmitterUserId;
                    ticket.Created = DateTime.SpecifyKind(ticket.Created, DateTimeKind.Utc);
                    ticket.Updated = DateTime.UtcNow;
                    ticket.TicketStatusId = ticket.TicketStatusId;

                    await _ticketService.UpdateTicketAsync(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //TODO: Add History
                Ticket? newTicket = await _ticketService.GetTicketAsNoTrackingAsync(ticket.Id);
                await _ticketHistoryService.AddHistoryAsync(oldTicket, newTicket, userId);

                return RedirectToAction(nameof(Index));
            }

            int companyId = User.Identity!.GetCompanyId();

            ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(companyId), "Id", "Description", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            //ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
            //ViewData["SubmitterUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.SubmitterUserId);
            //ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            return View(ticket);
        }

        // GET: Tickets/Restore/5
        public async Task<IActionResult> Archive(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }
            
            Ticket ticket = await _ticketService.GetTicketByIdAsync(id.Value);
            
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Restore/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tickets'  is null.");
            }

            Ticket ticket = await _ticketService.GetTicketByIdAsync(id);

            if (ticket != null)
            {
                await _ticketService.ArchiveTicketAsync(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Get: My Tickets (current logged in user)
        public async Task<IActionResult> MyTickets()
        {
            int companyId = User.Identity!.GetCompanyId();
            string userId = _userManager.GetUserId(User);

            List<Ticket> tickets = await _ticketService.GetTicketsByUserIdAsync(userId, companyId);

            return View(tickets);
        }

        // GET: Tickets/Restore/5
        public async Task<IActionResult> RestoreTicket(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            Ticket ticket = await _ticketService.GetTicketByIdAsync(id.Value);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tickets'  is null.");
            }

            Ticket ticket = await _ticketService.GetTicketByIdAsync(id);

            if (ticket != null)
            {
                await _ticketService.RestoreTicketAsync(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ArchivedTickets));
        }

        // Get: Show File
        public async Task<IActionResult> ShowFile(int id)
        {
            TicketAttachment ticketAttachment = await _ticketService.GetTicketAttachmentByIdAsync(id);
            string? fileName = ticketAttachment.FileName;
            byte[]? fileData = ticketAttachment.FileData;
            string? ext = Path.GetExtension(fileName)!.Replace(".", "");

            Response.Headers.Add("Content-Disposition", $"inline; filename={fileName}");
            return File(fileData!, $"application/{ext}");
        }

        // Get: Unassigned Tickets
        public async Task<IActionResult> UnassignedTickets()
        {
            int companyId = User.Identity!.GetCompanyId();

            List<Ticket> tickets = await _ticketService.GetUnassignedTicketsByCompanyIdAsync(companyId);

            return View(tickets);
        }
        private bool TicketExists(int id)
        {
          return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
