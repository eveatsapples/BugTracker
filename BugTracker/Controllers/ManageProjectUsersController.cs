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
    public class ManageProjectUsersController : Controller
    {
        private ApplicationDbContext DbContext;

        public ManageProjectUsersController()
        {
            DbContext = new ApplicationDbContext();
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult EditProjectUsers(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(MyProjectsController.Index));
            }

            var query = DbContext.Projects
              .FirstOrDefault(p => p.ID == id);
              
            var model = new EditProjectUsersViewModel
            {
                ID = query.ID,
            };

            DbContext.SaveChanges();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult CurrentProjectUsers(int id)
        {
            var model = DbContext.Users
              .Where(p => p.Projects.Any(i => i.ID == id))
              .Select(p => new ProjectUserViewModel
              {
                  ID = id,
                  UserID = p.Id,
                  UserName = p.UserName,
                  FirstName = p.FirstName,
                  LastName = p.LastName,
              }).ToList();

            DbContext.SaveChanges();
            return PartialView(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult AddProjectUsers(int id)
        {
            var model = DbContext.Users
              .Where(p => p.Projects.All(i => i.ID != id))
              .Select(p => new ProjectUserViewModel
              {
                  ID = id,
                  UserID = p.Id,
                  UserName = p.UserName,
                  FirstName = p.FirstName,
                  LastName = p.LastName,
              }).ToList();

            DbContext.SaveChanges();
            return PartialView(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult AddProjectUsers(ProjectUserViewModel projectUser)
        {
            var user = DbContext.Users.FirstOrDefault(
                p => p.Id == projectUser.UserID);

            var project = DbContext.Projects.FirstOrDefault(
                p => p.ID == projectUser.ID);

            if (user == null)
            {
                return RedirectToAction(nameof(ManageProjectUsersController.EditProjectUsers));
            }

            user.Projects.Add(project);

            DbContext.SaveChanges();
            return RedirectToAction("EditProjectUsers", "ManageProjectUsers", new { ID = projectUser.ID });
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult RemoveProjectUsers(ProjectUserViewModel projectUser)
        {
            var user = DbContext.Users.FirstOrDefault(
                p => p.Id == projectUser.UserID);

            var project = DbContext.Projects.FirstOrDefault(
                p => p.ID == projectUser.ID);

            if (user == null)
            {
                return RedirectToAction(nameof(ManageProjectUsersController.EditProjectUsers));
            }

            user.Projects.Remove(project);

            DbContext.SaveChanges();
            return RedirectToAction("EditProjectUsers", "ManageProjectUsers", new { ID = projectUser.ID });
        }
    }
}