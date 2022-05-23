using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerByBenci.Models
{
    public class TicketComment
    {
        // Primary Key
        public int Id { get; set; }
        [Required]
        [DisplayName("Member Comment")]
        [StringLength(2000)]
        public string? Comment { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Created Date")]
        public DateTime Created { get; set; }

        // Foreign Key
        public int TicketId { get; set; }
        
        // Foreign Key
        [Required]
        public string? UserId { get; set; }

        // Navigational Properties
        [DisplayName("Ticket")]
        public virtual Ticket? Ticket { get; set; }
        [DisplayName("Team Member")]
        public virtual BTUser? User { get; set; }

    }
}