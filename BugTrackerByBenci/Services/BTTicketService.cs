using BugTrackerByBenci.Data;
using BugTrackerByBenci.Models;
using BugTrackerByBenci.Models.Enums;
using BugTrackerByBenci.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerByBenci.Services
{
    public class BTTicketService : IBTTicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRolesService _rolesService;
        private readonly IBTProjectService _projectService;

        public BTTicketService(ApplicationDbContext context, IRolesService rolesService, IBTProjectService projectService)
        {
            _context = context;
            _rolesService = rolesService;
            _projectService = projectService;
        }

        #region Add New Ticket
        public async Task AddNewTicketAsync(Ticket ticket)
        {
            try
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AddTicketAttachmentAsync(TicketAttachment ticketAttachment)
        {
            try
            {
                _context.Add(ticketAttachment);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Archive Ticket
        public async Task ArchiveTicketAsync(Ticket ticket)
        {
            try
            {
                ticket.Archived = true;
                await UpdateTicketAsync(ticket);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Add Developer to Ticket Async
        public async Task<bool> AddDeveloperToTicketAsync(string userId, int ticketId)
        {
            BTUser? currentDeveloper = await GetCurrentDeveloperAsync(ticketId);
            BTUser? newDeveloper = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            Ticket? ticket= await _context.Tickets.FirstOrDefaultAsync(p => p.Id == ticketId);

            if (currentDeveloper != null)
            {
                try
                {
                    // Remove Developer from Ticket
                    await RemoveDeveloperFromTicketAsync(ticketId);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            // Add the new Developer
            try
            {
                // Add Developer to Ticket
                ticket!.DeveloperUser = newDeveloper;
                ticket.DeveloperUserId = userId;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get All Tickets By Company Id
        public async Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int companyId)
        {
            try
            {
                var tickets = _context.Tickets.Where(t => t.Archived == false)
                    .Include(t => t.SubmitterUser)!
                    .Include(t => t.DeveloperUser)!
                    .Include(t => t.TicketPriority)!
                    .Include(t => t.TicketStatus)!
                    .Include(t => t.TicketType)!
                    .Include(t => t.Project)!
                    .ThenInclude(c=>c!.Company)!
                    .Where(t => t.Project!.CompanyId == companyId)
                        .OrderByDescending(t=>t.Created);

                return await tickets.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Archived Tickets By Company Id
        public async Task<List<Ticket>> GetArchivedTicketsByCompanyIdAsync(int companyId)
        {
            try
            {
                var tickets = _context.Tickets.Where(t => t.Archived == true)
                    .Include(t => t.SubmitterUser)!
                    .Include(t => t.TicketPriority)!
                    .Include(t => t.TicketStatus)!
                    .Include(t => t.TicketType)!
                    .Include(t => t.Project)!
                    .ThenInclude(c => c!.Company)!
                    .Where(t => t.Project!.CompanyId == companyId)
                    .OrderByDescending(t => t.Created);

                return await tickets.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Ticket By Id
        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            try
            {
                Ticket? ticket = await _context.Tickets
                                        .Include(t => t.DeveloperUser)
                                        .Include(t => t.Project)
                                        .Include(t => t.SubmitterUser)
                                        .Include(t => t.TicketPriority)
                                        .Include(t => t.TicketStatus)
                                        .Include(t => t.TicketType)
                                        .Include(t => t.Comments)
                                        .Include(t => t.Attachments)
                                        .Include(t => t.History)
                                        .FirstOrDefaultAsync(m => m.Id == ticketId);

                return ticket!;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Ticket Attachment By Id
        public async Task<TicketAttachment> GetTicketAttachmentByIdAsync(int ticketAttachmentId)
        {
            try
            {
                TicketAttachment? ticketAttachment = await _context.TicketAttachments
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => t.Id == ticketAttachmentId);
                return ticketAttachment!;
            }
            catch (Exception)
            {

                throw;
            }
        }  
        #endregion

        #region Get Tickets By User Id
        public async Task<List<Ticket>> GetTicketsByUserIdAsync(string userId, int companyId)
        {
            BTUser? btUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            List<Ticket>? tickets = new();

            try
            {
                if (await _rolesService.IsUserInRoleAsync(btUser!, nameof(BTRoles.Admin)))
                {
                    tickets = (await _projectService.GetAllProjectsByCompanyIdAsync(companyId))
                                                    .SelectMany(p => p.Tickets!).ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(btUser!, nameof(BTRoles.Developer)))
                {
                    tickets = (await _projectService.GetAllProjectsByCompanyIdAsync(companyId))
                                                    .SelectMany(p => p.Tickets!)
                                                    .Where(t => t.DeveloperUserId == userId || t.SubmitterUserId == userId).ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(btUser!, nameof(BTRoles.Submitter)))
                {
                    tickets = (await _projectService.GetAllProjectsByCompanyIdAsync(companyId))
                                                    .SelectMany(t => t.Tickets!).Where(t => t.SubmitterUserId == userId).ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(btUser!, nameof(BTRoles.ProjectManager)))
                {
                    List<Ticket>? projectTickets = (await _projectService.GetUserProjectsAsync(userId)).SelectMany(t => t.Tickets!).ToList();
                    List<Ticket>? submittedTickets = (await _projectService.GetAllProjectsByCompanyIdAsync(companyId))
                                                    .SelectMany(p => p.Tickets!).Where(t => t.SubmitterUserId == userId).ToList();
                    tickets = projectTickets.Concat(submittedTickets).ToList();
                }

                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Current Developer
        public async Task<BTUser> GetCurrentDeveloperAsync(int ticketId)
        {
            try
            {
                Ticket? ticket = await _context.Tickets
                    .Include(t => t.DeveloperUser)
                    .FirstOrDefaultAsync(t => t.Id == ticketId);

                if (ticket!.DeveloperUser != null)
                {
                    return ticket.DeveloperUser;
                }

                return null!;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get All Unassigned Tickets By Company Id
        public async Task<List<Ticket>> GetUnassignedTicketsByCompanyIdAsync(int companyId)
        {
            try
            {
                var tickets = _context.Tickets.Where(t => t.Archived == false && t.DeveloperUser == null)
                    .Include(t => t.SubmitterUser)!
                    .Include(t => t.TicketPriority)!
                    .Include(t => t.TicketStatus)!
                    .Include(t => t.TicketType)!
                    .Include(t => t.Project)!
                    .ThenInclude(c => c!.Company)!
                    .Where(t => t.Project!.CompanyId == companyId)
                    .OrderByDescending(t => t.Created);

                return await tickets.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Checks if Developer is on the Ticket
        public async Task<bool> IsDeveloperOnTicketAsync(string userId, int ticketId)
        {
            try
            {
                Ticket? ticket = await _context.Tickets
                    .Include(t => t.DeveloperUser)
                    .FirstOrDefaultAsync(p => p.Id == ticketId);

                bool result = false;

                if (ticket != null)
                {
                    result = ticket.DeveloperUser is not null;
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Remove Developer from Ticket
        public async Task RemoveDeveloperFromTicketAsync(int ticketId)
        {
            try
            {
                Ticket? ticket = await _context.Tickets
                    .Include(t => t.DeveloperUser)
                    .FirstOrDefaultAsync(p => p.Id == ticketId);

                if (ticket!.DeveloperUser != null)
                {
                    // Remove Developer from Ticket
                    ticket.DeveloperUser = null;
                    await _context.SaveChangesAsync();
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Restore Ticket
        public async Task RestoreTicketAsync(Ticket ticket)
        {
            try
            {
                ticket.Archived = false;
                await UpdateTicketAsync(ticket);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update Ticket
        public async Task UpdateTicketAsync(Ticket ticket)
        {
            try
            {
                _context.Update(ticket);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region As No Tracking 
        public async Task<Ticket?> GetTicketAsNoTrackingAsync(int ticketId)
        {
            try
            {
                return await _context.Tickets
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.Project)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == ticketId);
            }
            catch (Exception)
            {
                throw;
            }
        } 
        #endregion
    }
}
