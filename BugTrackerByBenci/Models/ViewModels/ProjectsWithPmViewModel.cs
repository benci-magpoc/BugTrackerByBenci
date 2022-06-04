using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTrackerByBenci.Models.ViewModels
{
    public class ProjectsWithPMViewModel
    {
        public List<Project>? Projects { get; set; }
        public Dictionary<int, string> PMofProjectId{ get; set; } = new Dictionary<int, string>();
    }
}
