using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.Models.Domain;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;


namespace BugTracker.Controllers
{
    public class MyProjectsController : Controller
    {
        private ApplicationDbContext DbContext;

        public MyProjectsController()
        {
            DbContext = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var model = DbContext.Projects
              .Where((p => p.ProjectUsers.Any(i => i.Id == userId)
              && p.Archived == false))
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

        [Authorize]
        public ActionResult IndexPartial()
        {
            var userId = User.Identity.GetUserId();

            var model = DbContext.Projects
              .Where((p => p.ProjectUsers.Any(i => i.Id == userId)
              && p.Archived == false))
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

            return PartialView("Index", model);
        }
    }
}
