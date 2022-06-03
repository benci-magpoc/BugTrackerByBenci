using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTrackerByBenci.Data;
using BugTrackerByBenci.Extensions;
using BugTrackerByBenci.Models;
using BugTrackerByBenci.Models.Enums;
using BugTrackerByBenci.Models.ViewModels;
using BugTrackerByBenci.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BugTrackerByBenci.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTProjectService _projectService;
        private readonly IRolesService _rolesService;
        private readonly IBTFileService _fileService;
        public ProjectsController(ApplicationDbContext context, UserManager<BTUser> userManager, IBTProjectService projectService, IRolesService rolesService, IBTFileService fileService)
        {
            _context = context;
            _userManager = userManager;
            _projectService = projectService;
            _rolesService = rolesService;
            _fileService = fileService;
        }

        // GET: Projects
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //BTUser btUser = await _userManager.GetUserAsync(User);
            int companyId = User.Identity!.GetCompanyId();

            ProjectsWithPmViewModel model = new();

            List<Project> projects = await _projectService.GetAllProjectsByCompanyIdAsync(companyId);
            
            model.Projects = projects;

            foreach (var project in projects)
            {
                if ((await _projectService.GetProjectManagerAsync(project!.Id)) != null)
                {
                    model.ProjectId!.Add((int)project!.Id);
                    model.ProjectManager = (await _projectService.GetProjectManagerAsync(project!.Id)).FullName;
                }
                
            }

            //ViewData["PMinProject"] = projectManagers;

            return View(model);
        }

        public async Task<IActionResult> ArchivedProjects()
        {
            //BTUser btUser = await _userManager.GetUserAsync(User);
            int companyId = User.Identity!.GetCompanyId();

            List<Project> projects = await _projectService.GetArchivedProjectsByCompanyAsync(companyId);

            return View(projects);
        }

        // Get: Assign Project Manager
        public async Task<IActionResult> AssignProjectManager(int? id)
        {
            int companyId = User.Identity!.GetCompanyId();

            // Refactor for ViewModel
            AddProjectWithPMViewModel model = new();
            Project? project = await _projectService.GetProjectByIdAsync(id!.Value, companyId);

            model.Project = project;

            BTUser? projectManager = await _projectService.GetProjectManagerAsync(project!.Id);

            List<BTUser> pmInProject =
                await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), companyId);

            ViewData["PMinProject"] = pmInProject;

            if (projectManager != null)
            {
                model.PMList =
                    new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), companyId), "Id", "FullName", projectManager.Id);
            }
            else
            {
                model.PMList =
                    new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), companyId), "Id", "FullName");
            }

            model.PriorityList = new SelectList(_context.ProjectPriorities, "Id", "Name");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignProjectManager(AssignPMToProjectViewModel model)
        {
            if (model.Project == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Assigning values to project object
                    model.Project!.Created = DateTime.SpecifyKind(model.Project!.Created, DateTimeKind.Utc);
                    model.Project!.StartDate = DateTime.SpecifyKind(model.Project!.StartDate, DateTimeKind.Utc);
                    model.Project!.EndDate = DateTime.SpecifyKind(model.Project!.EndDate, DateTimeKind.Utc);

                    await _projectService.UpdateProjectAsync(model.Project);

                    if (!string.IsNullOrEmpty(model.PMID))
                    {
                        await _projectService.AddProjectManagerAsync(model.PMID, model.Project!.Id);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(model.Project!.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            int companyId = User.Identity!.GetCompanyId();

            BTUser? projectManager = await _projectService.GetProjectManagerAsync(model.Project!.Id);

            if (projectManager != null)
            {
                model.PMList =
                    new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), companyId), "Id", "FullName", projectManager.Id);
            }
            else
            {
                model.PMList =
                    new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), companyId), "Id", "FullName");
            }


            return View(model);
        }

        // Assign Project Members
        [HttpGet]
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> AssignProjectMembers(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProjectMembersViewModel model = new();

            int companyId = User.Identity!.GetCompanyId();

            model.Project = await _projectService.GetProjectByIdAsync(id.Value, companyId);
            // Get available members
            List<BTUser> developers = await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.Developer), companyId);
            List<BTUser> submitters = await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.Submitter), companyId);
            // Assign members 
            List<BTUser> teamMembers = developers.Concat(submitters).ToList();
            // Get any current members
            List<string> projectMembers = model.Project!.Members!.Select(m => m.Id).ToList();

            model.UsersList = new MultiSelectList(teamMembers, "Id", "FullName", projectMembers);
            return View(model);
        }

        // Post method for Assign Project Members
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignProjectMembers(ProjectMembersViewModel model)
        {
            if (model.SelectedUsers != null)
            {
                List<string> memberIds = (await _projectService.GetAllProjectMembersExceptPMAsync(model.Project!.Id))
                                                                .Select(m => m.Id).ToList();

                // Remove current members
                foreach (string member in memberIds)
                {
                    await _projectService.RemoveUserFromProjectAsync(member, model.Project!.Id);
                }

                // Add selected members
                foreach (string member in model.SelectedUsers!)
                {
                    await _projectService.AddUserToProjectAsync(member, model.Project!.Id);
                }
                
                return RedirectToAction(nameof(Details), new { id = model.Project!.Id });
            }

            return RedirectToAction(nameof(Details), new { id = model.Project!.Id });
        }


        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            int companyId = User.Identity!.GetCompanyId();

            Project? project = await _projectService.GetProjectByIdAsync(id.Value, companyId);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public async Task<IActionResult> Create()
        {
            int companyId = User.Identity!.GetCompanyId();

            // Refactor for ViewModel
            AddProjectWithPMViewModel model = new();

            model.PMList =
                new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), companyId), "Id","FullName");
            model.PriorityList = new SelectList(_context.ProjectPriorities, "Id", "Name");
            
            return View(model);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddProjectWithPMViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check for Image in the Form
                if (model.Project?.ProjectImage != null)
                {
                    model.Project.FileData = await _fileService.ConvertFileToByteArrayAsync(model.Project.ProjectImage);
                    model.Project.FileName = model.Project.ProjectImage.FileName;
                    model.Project.FileContentType = model.Project.ProjectImage.ContentType;
                }

                // Assigning values to project object
                model.Project!.CompanyId = User.Identity!.GetCompanyId();
                model.Project!.Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                model.Project!.StartDate = DateTime.SpecifyKind(model.Project!.StartDate, DateTimeKind.Utc);
                model.Project!.EndDate = DateTime.SpecifyKind(model.Project!.EndDate, DateTimeKind.Utc);
                
                // Calling Add New Project Method of Project Service
                await _projectService.AddNewProjectAsync(model.Project!);

                // Allow Admin to add ProjectManager on create
                if (!string.IsNullOrEmpty(model.PMID))
                {
                    await _projectService.AddProjectManagerAsync(model.PMID, model.Project!.Id);
                }

                return RedirectToAction(nameof(Index));
            }
            
            int companyId = User.Identity!.GetCompanyId();
            model.PMList =
                new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), companyId), "Id", "FullName");
            model.PriorityList = new SelectList(_context.ProjectPriorities, "Id", "Name");

            return View(model);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            int companyId = User.Identity!.GetCompanyId();

            // Refactor for ViewModel
            AddProjectWithPMViewModel model = new();

            Project? project = await _projectService.GetProjectByIdAsync(id.Value, companyId);

            model.Project = project;

            BTUser? projectManager = await _projectService.GetProjectManagerAsync(project!.Id);

            if (projectManager != null)
            {
                model.PMList =
                    new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), companyId), "Id", "FullName", projectManager.Id);
            }
            else
            {
                model.PMList =
                    new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), companyId), "Id", "FullName");
            }
            
            model.PriorityList = new SelectList(_context.ProjectPriorities, "Id", "Name");

            return View(model);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AddProjectWithPMViewModel model)
        {
            if (model.Project == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check for Image in the Form
                    if (model.Project?.ProjectImage != null)
                    {
                        model.Project.FileData = await _fileService.ConvertFileToByteArrayAsync(model.Project.ProjectImage);
                        model.Project.FileName = model.Project.ProjectImage.FileName;
                        model.Project.FileContentType = model.Project.ProjectImage.ContentType;
                    }

                    // Assigning values to project object
                    model.Project!.Created = DateTime.SpecifyKind(model.Project!.Created, DateTimeKind.Utc);
                    model.Project!.StartDate = DateTime.SpecifyKind(model.Project!.StartDate, DateTimeKind.Utc);
                    model.Project!.EndDate = DateTime.SpecifyKind(model.Project!.EndDate, DateTimeKind.Utc);

                    await _projectService.UpdateProjectAsync(model.Project);

                    if (!string.IsNullOrEmpty(model.PMID))
                    {
                        await _projectService.AddProjectManagerAsync(model.PMID, model.Project!.Id);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(model.Project!.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            int companyId = User.Identity!.GetCompanyId();

            BTUser? projectManager = await _projectService.GetProjectManagerAsync(model.Project!.Id);

            if (projectManager != null)
            {
                model.PMList =
                    new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), companyId), "Id", "FullName", projectManager.Id);
            }
            else
            {
                model.PMList =
                    new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), companyId), "Id", "FullName");
            }

            model.PriorityList = new SelectList(_context.ProjectPriorities, "Id", "Name");

            return View(model);
        }

        // GET: Projects/Archive/5
        public async Task<IActionResult> Archive(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            int companyId = User.Identity!.GetCompanyId();

            Project? project = await _projectService.GetProjectByIdAsync(id.Value, companyId);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchivedConfirmed(int? id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Projects'  is null.");
            }

            int companyId = User.Identity!.GetCompanyId();

            Project? project = await _projectService.GetProjectByIdAsync(id!.Value, companyId);

            if (project != null)
            {
                await _projectService.ArchiveProjectAsync(project);
                return RedirectToAction(nameof(Index));
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MyProjects()
        {
            string userId = _userManager.GetUserId(User);

            List<Project> projects = await _projectService.GetUserProjectsAsync(userId);

            return View(projects);
        }

        // GET: Projects/Restore/5
        public async Task<IActionResult> RestoreProject(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            int companyId = User.Identity!.GetCompanyId();

            Project? project = await _projectService.GetProjectByIdAsync(id.Value, companyId);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int? id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Projects'  is null.");
            }

            int companyId = User.Identity!.GetCompanyId();

            Project? project = await _projectService.GetProjectByIdAsync(id!.Value, companyId);

            if (project != null)
            {
                await _projectService.RestoreArchivedProject(project);
                return RedirectToAction(nameof(ArchivedProjects));
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ArchivedProjects));
        }

        private bool ProjectExists(int id)
        {
          return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
