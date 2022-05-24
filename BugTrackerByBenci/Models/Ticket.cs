using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerByBenci.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Ticket Title")]
        public string? Title { get; set; }

        [Required]
        [DisplayName("Ticket Description")]
        [StringLength(200)]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Created Date")]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Updated Date")]
        public DateTime Updated { get; set; }

        public bool Archived { get; set; }
        public bool ArchivedByProject { get; set; }

        [Required] public int ProjectId { get; set; }
        [Required] public int TicketPriorityId { get; set; }
        [Required] public int TicketStatusId { get; set; }
        [Required] public int TicketTypeId { get; set; }

        // Foreign Keys
        [Required] string? SubmitterUserId { get; set; }
        [Required] string? DeveloperUserId { get; set; }

        // Navigational Property
        public virtual Project? Project { get; set; }
        public virtual TicketPriority? TicketPriority { get; set; }
        public virtual TicketStatus? TicketStatus { get; set; }
        public virtual TicketType? TicketType { get; set; }
        public virtual BTUser? SubmitterUser { get; set; }
        public virtual BTUser? DeveloperUser { get; set; }

        // Navigational Collections
        public virtual ICollection<TicketComment> Comments { get; set; } = new HashSet<TicketComment>();
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; } = new HashSet<TicketAttachment>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();

    }
}