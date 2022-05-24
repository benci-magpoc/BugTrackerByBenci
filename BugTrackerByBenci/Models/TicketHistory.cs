using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerByBenci.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }

        [DisplayName("Updated Ticket Priority")]
        public string?  PropertyName { get; set; }

        [DisplayName("Description of Change")]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Created")]
        public DateTime Created { get; set; }

        [DisplayName("Previous Value")]
        public string? OldValue { get; set; }
        
        [DisplayName("Current Value")]
        public string? NewValue { get; set; }
        public int TicketId { get; set; }

        [Required]
        public string? UserId { get; set; }

        // Navigational Properties
        public virtual Ticket? Ticket { get; set; }
        public virtual BTUser? User { get; set; }
    }
}