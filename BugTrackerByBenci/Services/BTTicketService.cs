using BugTrackerByBenci.Data;
using BugTrackerByBenci.Models;
using BugTrackerByBenci.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerByBenci.Services
{
    public class BTTicketService : IBTTicketService
    {
        private readonly ApplicationDbContext _context;


        public BTTicketService(ApplicationDbContext context)
        {
            _context = context;
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

        #region Get All Tickets By Company Id
        public async Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int companyId)
        {
            try
            {
                var tickets = _context.Tickets.Where(t => t.Archived == false)
                    .Include(t => t.SubmitterUser)!
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

        #region Get Ticket By Id
        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            try
            {
                Ticket? ticket = new();

                ticket = await _context.Tickets
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.Project)
                    .Include(t => t.SubmitterUser)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .FirstOrDefaultAsync(m => m.Id == ticketId);

                return ticket!;
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
    }
}
