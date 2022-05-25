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

        public async Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int companyId)
        {
            var tickets = _context.Tickets
                .Include(t => t.SubmitterUser)
                //.ThenInclude(u => u.Company).ThenInclude(c=>c.Members.Where(m=>m.CompanyId == companyId))
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Where(t=>t.Project!.CompanyId==companyId);

            return await tickets.ToListAsync();
        }
    }
}
