using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BugTrackerByBenci.Extensions;

namespace BugTrackerByBenci.Models
{
    public class TicketAttachment
    {
        // Primary Key
        public int Id { get; set; }

        [DisplayName("File Description")]
        [StringLength(200)]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Added")]
        public DateTime Created { get; set; }

        // Foreign Key
        public int TicketId { get; set; }

        // Foreign Key
        [Required]
        public string? UserId { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        [MaxFileSize(1024 * 1024)]
        [AllowedExtensions(new string[] {".jpg",".jpeg",".png",".doc",".docx",".xls",".xlsx",".pdf",".ppt",".pptx",".html",".svg",".webp"})]
        public IFormFile? FormFile { get; set; }
        [DisplayName("File Name")]
        public string? FileName { get; set; }
        [DisplayName("File Attachment")]
        public byte[]? FileData { get; set; }
        [DisplayName("File Extension")]
        public string? FileContentType { get; set; }

        // Navigational Properties
        [DisplayName("Ticket")]
        public virtual Ticket? Ticket { get; set; }
        [DisplayName("Team Member")]
        public virtual BTUser? User { get; set; }
    }
}