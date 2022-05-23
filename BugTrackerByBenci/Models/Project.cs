using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerByBenci.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Project Name")]
        public string? Name { get; set; }
    }
}