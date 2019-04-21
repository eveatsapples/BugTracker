using BugTracker.Models;
using BugTracker.Models.Domain;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    //[RoutePrefix("Projects")]
    public class ProjectController : Controller
    {
        private ApplicationDbContext DbContext;

        public ProjectController()
        {
            DbContext = new ApplicationDbContext();
        }

        //[Route]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Index()
        {
            var model = DbContext.Projects
              .Select(p => new IndexProjectsViewModel
              {
                  Slug = p.Slug,
                  ID = p.ID,
                  ProjectName = p.ProjectName,
                  Description = p.Description,
                  DateCreated = p.DateCreated.ToString(),
                  DateUpdated = p.DateUpdated.ToString(),
                  ProjectUsers = p.ProjectUsers.Count(),
                  Tickets = DbContext.Tickets.Where(t => t.ProjectID == p.ID).Count()
              }).ToList();

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Create(EditProjectViewModel formData)
        {
            return Save(null, formData);
        }


        [HttpGet]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var userId = User.Identity.GetUserId();

            var post = DbContext.Projects.FirstOrDefault(
                p => p.ID == id && p.ID == id);

            if (post == null)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var model = new EditProjectViewModel
            {
                ProjectName = post.ProjectName,
                Description = post.Description
            };

            DbContext.SaveChanges();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Edit(int id, EditProjectViewModel formData)
        {
            return Save(id, formData);
        }

        [Authorize(Roles = "Admin, Project Manager")]
        private ActionResult Save(int? id, EditProjectViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var userId = User.Identity.GetUserId();

            if (DbContext.Projects.Any(p => p.ProjectName == formData.ProjectName &&
                  (!id.HasValue || p.ID != id.Value)))
            {
                ModelState.AddModelError(nameof(EditProjectViewModel.ProjectName),
                    "Project name should be unique");

                return View();
            }

            Project project;

            if (!id.HasValue)
            {
                project = new Project
                {
                    Slug = Slugify(formData.ProjectName),
                    DateCreated = DateTime.Now
                };
                DbContext.Projects.Add(project);
            }
            else
            {
                project = DbContext.Projects.FirstOrDefault(
                    p => p.ID == id);

                if (project == null)
                {
                    return RedirectToAction(nameof(ProjectController.Index));
                }

                project.DateUpdated = DateTime.Now;
            }

            project.ProjectName = formData.ProjectName;
            project.Description = formData.Description;

            DbContext.SaveChanges();

            return RedirectToAction(nameof(ProjectController.Index));
        }

        [HttpGet]
        //[Route("{slug}")]
        public ActionResult FullProjectBySlug(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var userId = User.Identity.GetUserId();

            var post = DbContext.Projects.FirstOrDefault(p =>
            p.Slug.ToString() == slug);

            if (post == null)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var model = new FullProjectViewModel
            {
                ID = post.ID,
                ProjectName = post.ProjectName,
                Description = post.Description
            };

            return View("FullProject", model);
        }

        public static string Slugify(string str)
        {
            ProjectController pc = new ProjectController();
            str = str.ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s, %*]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = Regex.Replace(str, @"\s", "-");
            string result = str;
            if (pc.DbContext.Projects.Any(p => p.Slug == result))
            {
                int i = 1;
                str = str + "-";
                while (pc.DbContext.Projects.Any(p => p.Slug == result))
                {
                    string numStr = i.ToString();
                    result = str + numStr;
                    i++;
                }
            }
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var post = DbContext.Projects.FirstOrDefault(p => p.ID == id);

            if (post != null)
            {
                DbContext.Projects.Remove(post);
                DbContext.SaveChanges();
            }

            return RedirectToAction(nameof(ProjectController.Index));
        }

    }
}