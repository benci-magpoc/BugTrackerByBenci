using BugTrackerByBenci.Extensions;
using BugTrackerByBenci.Models;
using BugTrackerByBenci.Models.ViewModels;
using BugTrackerByBenci.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTrackerByBenci.Controllers
{
    [Authorize(Roles="Admin")]
    public class UserRoles : Controller
    {
        private readonly IRolesService _roleService;
        private readonly IBTCompanyInfoService _companyInfoService;

        // GET: UserRoles
        public UserRoles(IRolesService roleService, IBTCompanyInfoService companyInfoService)
        {
            _roleService = roleService;
            _companyInfoService = companyInfoService;
        }

        public async Task<ActionResult> ManageUserRoles()
        {
            List<ManageUserRolesViewModel> model = new();
            int companyId = User.Identity!.GetCompanyId();

            List<BTUser> btUsers = await _companyInfoService.GetAllMembersAsync(companyId);

            foreach (BTUser btUser in btUsers)
            {
                ManageUserRolesViewModel viewModel = new();
                viewModel.BtUser = btUser;
                IEnumerable<string> currentRoles = await _roleService.GetUserRolesAsync(btUser);
                viewModel.Roles =
                    new MultiSelectList(await _roleService.GetBTRolesAsync(), "Name", "Name", currentRoles);
                model.Add(viewModel);
            }
            //List<IdentityRole> users = 
            return View(model);
        }

        // GET: UserRoles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserRoles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(ManageUserRoles));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserRoles/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserRoles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(ManageUserRoles));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserRoles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserRoles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(ManageUserRoles));
            }
            catch
            {
                return View();
            }
        }
    }
}
