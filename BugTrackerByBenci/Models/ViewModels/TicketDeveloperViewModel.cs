using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTrackerByBenci.Models.ViewModels
{
    public class TicketDeveloperViewModel
    {
        public Ticket? Ticket { get; set; }
        public string? DeveloperId { get; set; }
        public SelectList? DeveloperList { get; set; }

    }
}
