using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerByBenci.Models
{
    public class TicketType
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Ticket Type")]
        public string? Name { get; set; }

    }
}