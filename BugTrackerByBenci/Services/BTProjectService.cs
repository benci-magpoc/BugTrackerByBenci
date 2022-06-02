﻿using BugTrackerByBenci.Data;
using BugTrackerByBenci.Models;
using BugTrackerByBenci.Models.Enums;
using BugTrackerByBenci.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerByBenci.Services
{
    public class BTProjectService : IBTProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IRolesService _rolesService;
        public BTProjectService(ApplicationDbContext context, UserManager<BTUser> userManager, IRolesService rolesService)
        {
            _context = context;
            _userManager = userManager;
            _rolesService = rolesService;
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

        #region Add Project Manager to the Project
        public async Task<bool> AddProjectManagerAsync(string userId, int projectId)
        {
            BTUser currentProjectManager = await GetProjectManagerAsync(projectId);

            if (currentProjectManager != null)
            {
                try
                {
                    // Remove Project Manager
                    await RemoveProjectManagerAsync(projectId);
                }
                catch (Exception)
                {
                    throw;
                } 
            }
            // Add the new PM
            try
            {
                // Add Project Manager
                return await AddUserToProjectAsync(userId, projectId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add User To Project Async
        public async Task<bool> AddUserToProjectAsync(string userId, int projectId)
        {
            BTUser? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user != null)
            {
                Project? project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                if (!await IsUserOnProjectAsync(userId, projectId))
                {
                    try
                    {
                        project!.Members!.Add(user);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        } 
        #endregion

        #region Archive Project
        public async Task ArchiveProjectAsync(Project project)
        {
            try
            {
                project.Archived = true;
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
                    .Include(p=>p.Company)
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
        
        #region Get a Project By Id
        public async Task<Project?> GetProjectByIdAsync(int projectId, int companyId)
        {
            try
            {
                Project? project = new();

                project = await _context.Projects
                    .Include(p => p.Tickets)!
                        .ThenInclude(t=>t.TicketStatus)
                    .Include(p => p.Tickets)!
                        .ThenInclude(t => t.TicketType)
                    .Include(p => p.Tickets)!
                        .ThenInclude(t => t.DeveloperUser)
                    .Include(p => p.Tickets)!
                        .ThenInclude(t => t.SubmitterUser)
                    .Include(p => p.Members)!
                    .Include(p=>p.Priority)
                    .FirstOrDefaultAsync(p=> p.Id == projectId && p.CompanyId == companyId);

                return project;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Project Manager
        public async Task<BTUser> GetProjectManagerAsync(int projectId)
        {
            try
            {
                Project? project = await _context.Projects
                                                .Include(p => p.Members)
                                                .FirstOrDefaultAsync(p => p.Id == projectId);

                foreach (BTUser member in project?.Members!)
                {
                    if (await _rolesService.IsUserInRoleAsync(member, nameof(BTRoles.ProjectManager)))
                    {
                        return member;
                    }
                }

                return null!;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Checks if user is on the Project
        public async Task<bool> IsUserOnProjectAsync(string userId, int projectId)
        {
            try
            {
                Project? project = await _context.Projects
                                                .Include(p => p.Members)
                                                .FirstOrDefaultAsync(p => p.Id == projectId);

                bool result = false;

                if (project != null)
                {
                    result = project.Members!.Any(m => m.Id == userId);
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        } 
        #endregion

        #region Remove User From Project
        public async Task<bool> RemoveUserFromProjectAsync(string userId, int projectId)
        {
            try
            {
                BTUser? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                Project? project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                try
                {
                    if (await IsUserOnProjectAsync(userId, projectId))
                    {
                        project?.Members?.Remove(user!);
                        await _context.SaveChangesAsync();
                        return true;
                    }

                    return false;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public async Task RemoveProjectManagerAsync(int projectId)
        {
            try
            {
                Project? project = await _context.Projects
                                                .Include(p => p.Members)
                                                .FirstOrDefaultAsync(p => p.Id == projectId);

                foreach (BTUser member in project?.Members!)
                {
                    if (await _rolesService.IsUserInRoleAsync(member, nameof(BTRoles.ProjectManager)))
                    {
                        // Remove PM from Project
                        await RemoveUserFromProjectAsync(member.Id, projectId);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

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

    }
}