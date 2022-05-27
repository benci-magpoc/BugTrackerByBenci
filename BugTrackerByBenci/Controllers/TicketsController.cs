﻿using System;
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
using Microsoft.AspNetCore.Identity;

namespace BugTrackerByBenci.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTTicketService _ticketService;
        private readonly IBTProjectService _projectService;

        public TicketsController(ApplicationDbContext context, UserManager<BTUser> userManager, IBTTicketService ticketService, IBTProjectService projectService)
        {
            _context = context;
            _userManager = userManager;
            _ticketService = ticketService;
            _projectService = projectService;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            int companyId = User.Identity!.GetCompanyId();

            List<Ticket> tickets = await _ticketService.GetAllTicketsByCompanyIdAsync(companyId); 

            return View(tickets);
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

            ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(companyId), "Id", "Name");
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
            //ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id");
            //ViewData["SubmitterUserId"] = new SelectList(_context.Users, "Id", "Id");
            //ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
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
                return RedirectToAction(nameof(Index));
            }

            int companyId = User.Identity!.GetCompanyId();
            
            ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(companyId), "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            //ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
            //ViewData["SubmitterUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.SubmitterUserId);
            //ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }
            int companyId = User.Identity!.GetCompanyId();

            Ticket ticket = await _ticketService.GetTicketByIdAsync(id.Value);

            if (ticket == null)
            {
                return NotFound();
            }

            ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(companyId), "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            //ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
            //ViewData["SubmitterUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.SubmitterUserId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Created,Updated,ProjectId,TicketPriorityId,TicketStatusId,TicketTypeId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            ModelState.Remove("SubmitterUserId");

            if (ModelState.IsValid)
            {
                try
                {
                    ticket.SubmitterUserId = _userManager.GetUserId(User);
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

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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

        private bool TicketExists(int id)
        {
          return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
