using BugTrackerByBenci.Models;

namespace BugTrackerByBenci.Services.Interfaces
{
    public interface IBTProjectService
    {
        public Task AddNewProjectAsync(Project project);
        public Task ArchiveProjectAsync(Project project);
        public Task<bool> AddProjectManagerAsync(string userId, int projectId);
        public Task<bool> AddUserToProjectAsync(string userId, int projectId);
        public Task<List<Project>> GetAllProjectsByCompanyIdAsync(int companyId);
        public Task<BTUser> GetProjectManagerAsync(int projectId);
        public Task<Project?> GetProjectByIdAsync(int projectId, int companyId);
        public Task<bool> IsUserOnProjectAsync(string userId, int projectId);
        public Task RemoveProjectManagerAsync(int projectId);
        public Task<bool> RemoveUserFromProjectAsync(string userId, int projectId);
        public Task UpdateProjectAsync(Project project);
        
    }
}
