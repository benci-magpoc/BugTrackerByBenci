using BugTrackerByBenci.Models;

namespace BugTrackerByBenci.Services.Interfaces
{
    public interface IBTTicketService
    {
        public Task AddNewTicketAsync(Ticket ticket);
        public Task ArchiveTicketAsync(Ticket ticket);
        public Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int companyId);
        public Task<Ticket> GetTicketByIdAsync(int ticketId);
        public Task UpdateTicketAsync(Ticket ticket);

    }
}
