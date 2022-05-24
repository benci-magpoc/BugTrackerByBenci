using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTrackerByBenci.Data;
using BugTrackerByBenci.Models;

namespace BugTrackerByBenci.Controllers
{
    public class ProjectStatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectStatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectStatus
        public async Task<IActionResult> Index()
        {
              return _context.ProjectStatus != null ? 
                          View(await _context.ProjectStatus.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ProjectStatus'  is null.");
        }

        // GET: ProjectStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProjectStatus == null)
            {
                return NotFound();
            }

            var projectStatus = await _context.ProjectStatus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectStatus == null)
            {
                return NotFound();
            }

            return View(projectStatus);
        }

        // GET: ProjectStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProjectStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ProjectStatus projectStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectStatus);
        }

        // GET: ProjectStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProjectStatus == null)
            {
                return NotFound();
            }

            var projectStatus = await _context.ProjectStatus.FindAsync(id);
            if (projectStatus == null)
            {
                return NotFound();
            }
            return View(projectStatus);
        }

        // POST: ProjectStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ProjectStatus projectStatus)
        {
            if (id != projectStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectStatusExists(projectStatus.Id))
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
            return View(projectStatus);
        }

        // GET: ProjectStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProjectStatus == null)
            {
                return NotFound();
            }

            var projectStatus = await _context.ProjectStatus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectStatus == null)
            {
                return NotFound();
            }

            return View(projectStatus);
        }

        // POST: ProjectStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProjectStatus == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProjectStatus'  is null.");
            }
            var projectStatus = await _context.ProjectStatus.FindAsync(id);
            if (projectStatus != null)
            {
                _context.ProjectStatus.Remove(projectStatus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectStatusExists(int id)
        {
          return (_context.ProjectStatus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
