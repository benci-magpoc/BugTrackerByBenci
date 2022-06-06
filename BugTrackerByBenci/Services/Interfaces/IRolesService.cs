using BugTrackerByBenci.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTrackerByBenci.Services.Interfaces
{
    public interface IRolesService
    {
        public Task<bool> IsUserInRoleAsync(BTUser user, string roleName);
        public Task<List<BTUser>> GetUsersInRoleAsync(string roleName, int companyId);
        public string GetUserRoleNameAsync(BTUser user);

    }
}
