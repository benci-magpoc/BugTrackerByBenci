using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerByBenci.Models
{
    public class ProjectStatus
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Status")]
        public string? Name { get; set; }
    }
}