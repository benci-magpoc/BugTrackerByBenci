using BugTrackerByBenci.Models;

namespace BugTrackerByBenci.Services.Interfaces
{
    public interface IBTProjectService
    {
        public Task AddNewProjectAsync(Project project);
        public Task ArchiveProjectAsync(Project project);
        public Task<List<Project>> GetAllProjectsByCompanyIdAsync(int companyId);
        public Task<Project?> GetProjectByIdAsync(int projectId);
        public Task UpdateProjectAsync(Project project);
        
    }
}
