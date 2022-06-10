using BugTrackerByBenci.Models;

namespace BugTrackerByBenci.Services.Interfaces
{
    public interface IBTTicketService
    {
        public Task AddNewTicketAsync(Ticket ticket);
        public Task AddTicketAttachmentAsync(TicketAttachment ticketAttachment);
        public Task ArchiveTicketAsync(Ticket ticket);
        public Task<bool> AddDeveloperToTicketAsync(string userId, int ticketId);
        public Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int companyId);
        public Task<List<Ticket>> GetArchivedTicketsByCompanyIdAsync(int companyId);
        public Task<BTUser> GetCurrentDeveloperAsync(int ticketId);
        public Task<TicketAttachment> GetTicketAttachmentByIdAsync(int ticketAttachmentId);
        public Task<Ticket> GetTicketByIdAsync(int ticketId);
        public Task<List<Ticket>> GetTicketsByUserIdAsync(string userId, int companyId);
        public Task<List<Ticket>> GetUnassignedTicketsByCompanyIdAsync(int companyId);
        public Task<bool> IsDeveloperOnTicketAsync(string userId, int ticketId);
        public Task RemoveDeveloperFromTicketAsync(int ticketId);
        public Task RestoreTicketAsync(Ticket ticket);
        public Task UpdateTicketAsync(Ticket ticket);
        public Task<Ticket?> GetTicketAsNoTrackingAsync(int ticketId);

    }
}
