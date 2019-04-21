using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;
using BugTracker.Models;
using BugTracker.Models.Domain;
using BugTracker.Models.ViewModels;
using AutoMapper.QueryableExtensions;


namespace BugTracker.Controllers
{
    public class CommentController : Controller
    {
        private ApplicationDbContext DbContext;

        public CommentController()
        {
            DbContext = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Show(int id)
        {
            var Tickets = DbContext.TicketComments
                .Where(p => p.TicketID == id)
                .ToList();

            var model = DbContext.TicketComments
              .Where(p => p.TicketID == id)
              .Select(p => new ShowCommentViewModel
              {
                  ID = p.ID,
                  TicketID = p.TicketID,
                  Comment = p.Comment,
                  DateCreated = p.DateCreated.ToString(),
                  User = DbContext.Users.FirstOrDefault(u => u.Id == p.UserID).UserName
              }).ToList();

            return PartialView(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            int ticketID = id.GetValueOrDefault();

            var ticket = new EditCommentViewModel
            {
                TicketID = ticketID
            };
            return PartialView(ticket);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(EditCommentViewModel formData)
        {
            return Save(null, formData);
        }

        [HttpPost]
        [Authorize(Roles = "Submitter")]
        public ActionResult Edit(int id, int commentId, EditCommentViewModel formData)
        {
            return Save(id, formData);
        }

        [Authorize(Roles = "Submitter")]
        private ActionResult Save(int? id, EditCommentViewModel formData)
        {
            var ticket = DbContext.Tickets
                .FirstOrDefault(p => p.ID == formData.TicketID);

            if (!ModelState.IsValid)
            {
                return RedirectToAction("FullProjectBySlug", "Project", new { slug = ticket.Slug });
            }

            var userId = User.Identity.GetUserId();

            if ((User.IsInRole("Admin") || User.IsInRole("Project Manager")) 
                || (User.IsInRole("Developer") && userId == ticket.AssignedToUserID) 
                || (User.IsInRole("Submitter") && userId == ticket.OwnerUserID))
            {
                // Continue
            }
            else
            {
                return RedirectToAction("FullTicketBySlug", "Ticket", new { slug = ticket.Slug });
            }

            TicketComment comment;

                comment = new TicketComment
                {
                    DateCreated = DateTime.Now,
                    Comment = formData.Comment,
                    TicketID = formData.TicketID,
                    UserID = userId
                };
                DbContext.TicketComments.Add(comment);

            DbContext.SaveChanges();

            return RedirectToAction("FullTicketBySlug", "Ticket", new { slug = ticket.Slug });
        }

        [HttpPost]
        [Authorize(Roles = "Submitter")]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var ticket = DbContext.Tickets.FirstOrDefault(p => p.ID == id);
            var project = DbContext.Projects.FirstOrDefault(p => p.ID == ticket.ProjectID);

            if (ticket != null)
            {
                DbContext.Tickets.Remove(ticket);
                DbContext.SaveChanges();
            }

            //return RedirectToAction(nameof(ProjectController.Index));
            return RedirectToAction("FullProjectBySlug", "Project", new { slug = project.Slug });
        }

    }
}