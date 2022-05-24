using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BugTrackerByBenci.Models
{
    public class BTUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(40, ErrorMessage = "The {0} must be a minimum of {2} characters and a max of {1}.", MinimumLength = 3)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(40, ErrorMessage = "The {0} must be a minimum of {2} characters and a max of {1}.", MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile? AvatarFormFile { get; set; }

        [DisplayName("Avatar")] 
        public string? AvatarName { get; set; }
        public byte[]? AvatarData { get; set; }

        [DisplayName("File Extension")] 
        public string? AvatarContentType { get; set; }

        public int CompanyId { get; set; }

        // Navigational Properties
        public virtual Company? Company { get; set; }
        public virtual ICollection<Project>? Projects { get; set; } = new HashSet<Project>();

    }
}
