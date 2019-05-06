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
    public class HomeController : Controller
    {
        private ApplicationDbContext DbContext;

        public HomeController()
        {
            DbContext = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles ="Admin, Project Manager")]
        public ActionResult NumberOfProjects()
        {
            string userId = User.Identity.GetUserId();
            var user = DbContext.Users.FirstOrDefault(u => u.Id == userId);
            int number = DbContext.Projects
                .Where(p => p.Archived == false)
                .Select(p => p.Archived == false).Count();

            var model = new NumberWidget { Number = number };

            return PartialView(model);
        }

        [Authorize(Roles ="Admin, Project Manager")]
        public ActionResult NumberOfTickets()
        {
            var userId = User.Identity.GetUserId();
            var user = DbContext.Users.FirstOrDefault(u => u.Id == userId);

            var number = DbContext.Tickets
              .Where(t => t.Project.Archived == false)
              .Count(); 

            var open = DbContext.Tickets
              .Where(t => t.Status.Name == "Open"
              && t.Project.Archived == false)
              .Count(); 

            var resolved = DbContext.Tickets
              .Where(t => t.Status.Name == "Resolved"
              && t.Project.Archived == false)
              .Count(); 

            var rejected = DbContext.Tickets
              .Where(t => t.Status.Name == "Rejected"
              && t.Project.Archived == false)
              .Count(); 

            var model = new NumberOfTicketsWidget
            {
               Number = number,
               Open = open,
               Resolved = resolved,
               Rejected = rejected
            };

            return PartialView(model);
        }

        [Authorize(Roles ="Developer, Submitter")]
        public ActionResult NumberOfAssignedProjects()
        {
            string userId = User.Identity.GetUserId();
            var user = DbContext.Users.FirstOrDefault(u => u.Id == userId);
            var number = user.Projects .Where(p => p.Archived == false) .Count();

            var model = new NumberWidget { Number = number };

            return PartialView(model);
        }

        [Authorize(Roles ="Developer")]
        public ActionResult NumberOfAssignedTickets()
        {
            string userId = User.Identity.GetUserId();
            int number = DbContext.Tickets.Where(t => t.AssignedToUserID == userId
            && t.Project.Archived == false).Count();

            var model = new NumberWidget { Number = number };

            return PartialView(model);
        }

        [Authorize(Roles ="Submitter")]
        public ActionResult NumberOfCreatedTickets()
        {
            string userId = User.Identity.GetUserId();
            int number = DbContext.Tickets.Where(t => t.OwnerUserID == userId
            && t.Project.Archived == false).Count();

            var model = new NumberWidget { Number = number };

            return PartialView(model);
        }
    }
}