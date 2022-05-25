using BugTrackerByBenci.Data;
using BugTrackerByBenci.Models;
using BugTrackerByBenci.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerByBenci.Services
{
    public class BTProjectService : IBTProjectService
    {
        private readonly ApplicationDbContext _context;
        
        public BTProjectService(ApplicationDbContext context)
        {
            _context = context;
        }


        #region Add New Project
        public async Task AddNewProjectAsync(Project project)
        {
            try
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        } 
        #endregion

        #region Archive Project
        public async Task ArchiveProjectAsync(Project project)
        {
            try
            {
                project.Archived = true;
                _context.Update(project);
                await UpdateProjectAsync(project);
            }
            catch (Exception)
            {

                throw;
            }
        } 
        #endregion

        #region Get All Projects By Company Id
        public async Task<List<Project>> GetAllProjectsByCompanyIdAsync(int companyId)
        {
            try
            {
                List<Project>? projects = new();

                projects = await _context.Projects.Where(p => p.CompanyId == companyId && p.Archived == false)
                    .Include(p => p.Members)
                    .Include(p => p.Tickets)!
                    .ThenInclude(p => p.Comments)
                    .Include(p => p.Tickets)!
                    .ThenInclude(p => p.Attachments)
                    .Include(p => p.Tickets)!
                    .ThenInclude(p => p.History)
                    .Include(p => p.Tickets)!
                    .ThenInclude(p => p.Notifications)
                    .Include(p => p.Tickets)!
                    .ThenInclude(p => p.DeveloperUser)
                    .Include(p => p.Tickets)!
                    .ThenInclude(p => p.SubmitterUser)
                    .Include(p => p.Tickets)!
                    .ThenInclude(p => p.TicketStatus)
                    .Include(p => p.Tickets)!
                    .ThenInclude(p => p.TicketPriority)
                    .Include(p => p.Tickets)!
                    .ThenInclude(p => p.TicketType)
                    .Include(p => p.Priority)
                    .ToListAsync();

                return projects;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Project
        public async Task UpdateProjectAsync(Project project)
        {
            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        } 
        #endregion

        //TODO Make project by ID method

    }
}
