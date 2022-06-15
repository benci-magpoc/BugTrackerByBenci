namespace BugTrackerByBenci.Models.ViewModels
{
    public class ProjectDetailsWithHistoryViewModel
    {
        public Project? Project { get; set; }
        public List<TicketHistory>? TicketHistory { get; set; }
    }
}
