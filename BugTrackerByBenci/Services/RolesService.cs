using BugTrackerByBenci.Data;
using BugTrackerByBenci.Models;
using BugTrackerByBenci.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerByBenci.Services
{
    public class RolesService : IRolesService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BTUser> _userManager;

        public RolesService(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<BTUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<BTUser>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            try
            {
                List<BTUser>? users = (await _userManager.GetUsersInRoleAsync(roleName)).ToList();
                List<BTUser>? results = users.Where(u => u.CompanyId == companyId).ToList();

                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> IsUserInRoleAsync(BTUser user, string roleName)
        {
            try
            {
                // user = _userManager.GetUserIdAsync()
                bool result = await _userManager.IsInRoleAsync(user, roleName);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetUserRoleNameAsync(BTUser user)
        {
            try
            {
                var roleId = _context.UserRoles.First(u=>u.UserId==user.Id).RoleId;
                var roleName = _context.Roles.Find(roleId)!.Name;
                
                return roleName!;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
