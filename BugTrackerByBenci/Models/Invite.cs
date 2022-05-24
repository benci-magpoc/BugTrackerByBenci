using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerByBenci.Models
{
    public class Invite
    {
        // Primary Key
        public int Id { get; set; }

        [DisplayName("Date Sent")]
        [DataType(DataType.Date)]
        public DateTime InviteDate { get; set; }
        
        [DisplayName("Date Joined")]
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }
        
        public Guid CompanyToken { get; set; }
        
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        [Required]
        public string? InvitorId { get; set; }
        public string? InviteeId { get; set; }
        
        [Required]
        [DisplayName("Email")]
        public string? InviteeEmail { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string? InviteeFirstName{ get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string? InviteeLastName { get; set; }

        public string? Message { get; set; }
        public bool IsValid { get; set; }

        // Navigation Properties
        public virtual Company? Company { get; set; }
        public virtual Project? Project { get; set; }
        public virtual BTUser? Invitor { get; set; }
        public BTUser? Invitee { get; set; }

    }
}