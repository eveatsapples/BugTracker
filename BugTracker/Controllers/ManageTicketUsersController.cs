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
    public class ManageTicketUsersController : Controller
    {
        private ApplicationDbContext DbContext;

        public ManageTicketUsersController()
        {
            DbContext = new ApplicationDbContext();
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult EditTicketUsers(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(MyProjectsController.Index));
            }

            var ticket = DbContext.Tickets
              .FirstOrDefault(p => p.ID == id);

            var model = new EditTicketUsersViewModel
            {
                ID = ticket.ID,
            };

            DbContext.SaveChanges();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult CurrentTicketUsers(int id)
        {
            var model = DbContext.Tickets
              .Where(p => p.ID == id)
              .Select(p => new TicketUserViewModel
              {
                  ID = id
              }).ToList();

            DbContext.SaveChanges();
            return PartialView(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult AddTicketUsers(int id)
        {
            var model = DbContext.Users
              .Where(p => p.Projects.All(i => i.ID != id))
              .Select(p => new TicketUserViewModel
              {
                  ID = id,
                  UserID = p.Id,
                  UserName = p.UserName,
                  FirstName = p.FirstName,
                  LastName = p.LastName
              }).ToList();

            DbContext.SaveChanges();
            return PartialView(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult AddTicketUsers(TicketUserViewModel ticketUser)
        {
            var user = DbContext.Users.FirstOrDefault(
                p => p.Id == ticketUser.UserID);

            var ticket = DbContext.Tickets.FirstOrDefault(
                p => p.ID == ticketUser.ID);

            if (user == null)
            {
                return RedirectToAction(nameof(ManageProjectUsersController.EditProjectUsers));
            }

            //user.tickets.Add(ticket);

            DbContext.SaveChanges();
            return RedirectToAction("EditTicketUsers", "ManageTicketUsers", new { id = ticketUser.ID });
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult RemoveTicketUsers(TicketUserViewModel ticketUser)
        {
            var user = DbContext.Users.FirstOrDefault(
                p => p.Id == ticketUser.UserID);

            var project = DbContext.Projects.FirstOrDefault(
                p => p.ID == ticketUser.ID);

            if (user == null)
            {
                return RedirectToAction(nameof(ManageProjectUsersController.EditProjectUsers));
            }

            user.Projects.Remove(project);

            DbContext.SaveChanges();
            return RedirectToAction("EditTicketUsers", "ManageTicketUsers", new { id = ticketUser.ID });
        }
    }
}
