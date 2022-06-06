using BugTrackerByBenci.Models;

namespace BugTrackerByBenci.Services.Interfaces
{
    public interface IBTTicketService
    {
        public Task AddNewTicketAsync(Ticket ticket);
        public Task ArchiveTicketAsync(Ticket ticket);
        public Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int companyId);
        public Task<List<Ticket>> GetArchivedTicketsByCompanyIdAsync(int companyId);
        public Task<Ticket> GetTicketByIdAsync(int ticketId);
        public Task<List<Ticket>> GetTicketsByUserIdAsync(string userId, int companyId);
        public Task RestoreTicketAsync(Ticket ticket);
        public Task UpdateTicketAsync(Ticket ticket);

    }
}
