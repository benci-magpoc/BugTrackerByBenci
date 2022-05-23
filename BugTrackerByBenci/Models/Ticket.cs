using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerByBenci.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Ticket Name")]
        public string? Name { get; set; }


    }
}