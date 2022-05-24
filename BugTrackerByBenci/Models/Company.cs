using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerByBenci.Models
{

    public class Company
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Company Name")]
        public string? Name { get; set; }

        [Required]
        [DisplayName("Company Description")]
        public string? Description { get; set; }

        // Navigational Properties
        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();
        public virtual ICollection<BTUser> Members { get; set; } = new HashSet<BTUser>();
        public virtual ICollection<Invite> Invites { get; set; } = new HashSet<Invite>();

    }
}