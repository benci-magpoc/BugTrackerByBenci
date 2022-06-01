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
        public ProjectsController(ApplicationDbContext context, UserManager<BTUser> userManager, IBTProjectService projectService, IRolesService rolesService)
        {
            _context = context;
            _userManager = userManager;
            _projectService = projectService;
            _rolesService = rolesService;
        }

        // GET: Projects
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //BTUser btUser = await _userManager.GetUserAsync(User);
            int companyId = User.Identity!.GetCompanyId();
            
            List<Project> projects = await _projectService.GetAllProjectsByCompanyIdAsync(companyId);
            
            return View(projects);
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
                // Assigning values to project object
                model.Project!.CompanyId = User.Identity!.GetCompanyId();
                model.Project!.Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                model.Project!.StartDate = DateTime.SpecifyKind(model.Project!.StartDate, DateTimeKind.Utc);
                model.Project!.EndDate = DateTime.SpecifyKind(model.Project!.EndDate, DateTimeKind.Utc);
                
                // Calling Add New Project Method of Project Service
                await _projectService.AddNewProjectAsync(model.Project!);

                // TODO: Allow Admin to add ProjectManager on create
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

            Project? project = await _projectService.GetProjectByIdAsync(id.Value, companyId);

            if (project == null)
            {
                return NotFound();
            }
            //ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Description", project.CompanyId);
            ViewData["ProjectPriorityId"] = new SelectList(_context.ProjectPriorities, "Id", "Name", project.ProjectPriorityId);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyId,Name,Description,Created,StartDate,EndDate,ProjectPriorityId,FileName,FileData,FileContentType,Archived")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int companyId = User.Identity!.GetCompanyId();

                    // Assigning values to project object
                    project.CompanyId = companyId;
                    project.Created = DateTime.SpecifyKind(project.Created, DateTimeKind.Utc);
                    project.StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc);
                    project.EndDate = DateTime.SpecifyKind(project.EndDate, DateTimeKind.Utc);

                    await _projectService.UpdateProjectAsync(project);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            //ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Description", project.CompanyId);
            ViewData["ProjectPriorityId"] = new SelectList(_context.ProjectPriorities, "Id", "Name", project.ProjectPriorityId);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Projects'  is null.");
            }

            int companyId = User.Identity!.GetCompanyId();

            Project? project = await _projectService.GetProjectByIdAsync(id.Value, companyId);

            if (project != null)
            {
                await _projectService.ArchiveProjectAsync(project);
                return RedirectToAction(nameof(Index));
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
          return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
