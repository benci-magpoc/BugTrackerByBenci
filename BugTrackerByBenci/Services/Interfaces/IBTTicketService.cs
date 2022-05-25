using BugTrackerByBenci.Models;

namespace BugTrackerByBenci.Services.Interfaces
{
    public interface IBTTicketService
    {
        public Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int companyId);
    }
}
