using BugTracker.Models;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class ManageUserRolesController : AccountController
    {
        private ApplicationDbContext DbContext;

        public ManageUserRolesController()
        {
            DbContext = new ApplicationDbContext();
        }

        // GET: ManageUserRoles
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var model = DbContext.Users
              .Select(p => new ManageUserRolesViewModel
              {
                  UserID = p.Id,
                  UserName = p.UserName,
                  FirstName = p.FirstName,
                  LastName = p.LastName,
                  //Roles = p.Roles.Select(r => r.RoleId).ToList()

                  Roles = (from userRole in p.Roles
                               join role in DbContext.Roles on userRole.RoleId
                               equals role.Id
                               select role.Name).ToList()
              }).ToList();

            DbContext.SaveChanges();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddRoleAdmin(string id)
        {
            var user = DbContext.Users.FirstOrDefault(
                p => p.Id == id);

            if (!UserManager.IsInRole(user.Id, "Admin"))
            {
                UserManager.AddToRole(user.Id, "Admin");
            }

            DbContext.SaveChanges();
            return RedirectToAction(nameof(ManageUserRolesController.Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddRoleProjectManager(string id)
        {
            var user = DbContext.Users.FirstOrDefault(
                p => p.Id == id);

            if (!UserManager.IsInRole(user.Id, "Project Manager"))
            {
                UserManager.AddToRole(user.Id, "Project Manager");
            }

            DbContext.SaveChanges();
            return RedirectToAction(nameof(ManageUserRolesController.Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddRoleDeveloper(string id)
        {
            var user = DbContext.Users.FirstOrDefault(
                p => p.Id == id);

            if (!UserManager.IsInRole(user.Id, "Developer"))
            {
                UserManager.AddToRole(user.Id, "Developer");
            }

            DbContext.SaveChanges();
            return RedirectToAction(nameof(ManageUserRolesController.Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddRoleSubmitter(string id)
        {
            var user = DbContext.Users.FirstOrDefault(
                p => p.Id == id);

            if (!UserManager.IsInRole(user.Id, "Submitter"))
            {
                UserManager.AddToRole(user.Id, "Submitter");
            }

            DbContext.SaveChanges();
            return RedirectToAction(nameof(ManageUserRolesController.Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult RemoveRoleAdmin(string id)
        {
            var user = DbContext.Users.FirstOrDefault(
                p => p.Id == id);

            if (UserManager.IsInRole(user.Id, "Admin"))
            {
                UserManager.RemoveFromRole(user.Id, "Admin");
            }

            DbContext.SaveChanges();
            return RedirectToAction(nameof(ManageUserRolesController.Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult RemoveRoleProjectManager(string id)
        {
            var user = DbContext.Users.FirstOrDefault(
                p => p.Id == id);

            if (UserManager.IsInRole(user.Id, "Project Manager"))
            {
                UserManager.RemoveFromRole(user.Id, "Project Manager");
            }

            DbContext.SaveChanges();
            return RedirectToAction(nameof(ManageUserRolesController.Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult RemoveRoleDeveloper(string id)
        {
            var user = DbContext.Users.FirstOrDefault(
                p => p.Id == id);

            if (UserManager.IsInRole(user.Id, "Developer"))
            {
                UserManager.RemoveFromRole(user.Id, "Developer");
            }

            DbContext.SaveChanges();
            return RedirectToAction(nameof(ManageUserRolesController.Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult RemoveRoleSubmitter(string id)
        {
            var user = DbContext.Users.FirstOrDefault(
                p => p.Id == id);

            if (UserManager.IsInRole(user.Id, "Submitter"))
            {
                UserManager.RemoveFromRole(user.Id, "Submitter");
            }

            DbContext.SaveChanges();
            return RedirectToAction(nameof(ManageUserRolesController.Index));
        }

    }
}