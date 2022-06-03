using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTrackerByBenci.Models.ViewModels
{
    public class AssignPMToProjectViewModel
    {
        public Project? Project { get; set; }
        public string? PMID { get; set; }
        public SelectList? PMList { get; set; }
    }
}
