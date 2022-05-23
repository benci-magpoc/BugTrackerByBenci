using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerByBenci.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Notification")]
        public string? Name { get; set; }
    }
}