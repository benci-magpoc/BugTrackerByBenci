using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTrackerByBenci.Models.ViewModels
{
    public class ProjectsWithPmViewModel
    {
        public List<Project>? Projects { get; set; }
        public List<int> ProjectId { get; set; } = new List<int>();
        public string? ProjectManager { get; set; }

    }
}
