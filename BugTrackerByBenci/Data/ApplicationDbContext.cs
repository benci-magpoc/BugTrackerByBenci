using BugTrackerByBenci.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerByBenci.Data
{
    public class ApplicationDbContext : IdentityDbContext<BTUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; } = default!;
        public DbSet<Invite> Invites { get; set; } = default!;
        public DbSet<Notification> Notifications { get; set; } = default!;
        public DbSet<NotificationType> NotificationTypes { get; set; } = default!;
        public DbSet<Project> Projects { get; set; } = default!;
        public DbSet<ProjectStatus> ProjectStatuses { get; set; } = default!;
        public DbSet<Ticket> Tickets { get; set; } = default!;
        public DbSet<TicketAttachment> TicketAttachments { get; set; } = default!;
        public DbSet<TicketComment> TicketComments { get; set; } = default!;
        public DbSet<TicketHistory> TicketHistories { get; set; } = default!;
        public DbSet<TicketPriority> TicketPriorities { get; set; } = default!;
        public DbSet<TicketStatus> TicketStatuses { get; set; } = default!;
        public DbSet<TicketType> TicketTypes { get; set; } = default!;

    }
}