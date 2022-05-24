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
        public string? Name { get; set; }
        
        [Required]
        [DisplayName("Project Description")]
        public string? Description { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayName("Created Date")]
        public DateTime Created { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        [Required] 
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

        [DisplayName("Project Priority")] 
        public virtual ProjectPriority? Priority { get; set; }

        // Navigational Collections
        public virtual ICollection<BTUser>? Members { get; set; } = new HashSet<BTUser>();
        public virtual ICollection<Ticket>? Tickets { get; set; } = new HashSet<Ticket>();
    }
}