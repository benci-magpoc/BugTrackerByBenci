using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerByBenci.Models
{
    public class Project
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Key
        [Required]
        public int CompanyId { get; set; }

        [Required]
        [DisplayName("Project Name")]
        [StringLength(240, ErrorMessage = "The {0} must be a minimum of {2} characters and a max of {1}.", MinimumLength = 3)]
        public string? Name { get; set; }
        
        [Required]
        [DisplayName("Project Description")]
        [StringLength(240, ErrorMessage = "The {0} must be a minimum of {2} characters and a max of {1}.", MinimumLength = 3)]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Created")]
        public DateTime Created { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayName("Project Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Project End Date")]
        public DateTime EndDate { get; set; }

        public int ProjectPriorityId { get; set; }

        // Image Properties
        [NotMapped]
        [DataType(DataType.Upload)]
        // TODO: Add Custom Attributes
        public IFormFile? FormFile { get; set; }
        [DisplayName("File Name")]
        public string? FileName { get; set; }
        [DisplayName("File Attachment")]
        public byte[]? FileData { get; set; }
        [DisplayName("File Extension")]
        public string? FileContentType { get; set; }

        public bool Archived { get; set; }

        // Navigational Properties
        [DisplayName("Company")]
        public virtual Company? Company  { get; set; }

        [DisplayName("Priority")] 
        public virtual ProjectPriority? Priority { get; set; }

        // Navigational Collections
        public virtual ICollection<BTUser>? Members { get; set; } = new HashSet<BTUser>();
        public virtual ICollection<Ticket>? Tickets { get; set; } = new HashSet<Ticket>();
    }
}