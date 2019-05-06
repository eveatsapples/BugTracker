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

            var currentUserID = User.Identity.GetUserId();

            var model = DbContext.TicketComments
              .Where(p => p.TicketID == id)
              .Select(p => new ShowCommentViewModel
              {
                  ID = p.ID,
                  TicketID = p.TicketID,
                  Comment = p.Comment,
                  DateCreated = p.DateCreated.ToString(),
                  User = DbContext.Users.FirstOrDefault(u => u.Id == p.UserID).UserName,
                  UserID = p.UserID,
                  CurrentUserID = currentUserID
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

            var comment = new EditCommentViewModel
            {
                TicketID = ticketID
            };
            return PartialView(comment);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(EditCommentViewModel formData)
        {
            return Save(null, formData);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }
            var commentId = id.GetValueOrDefault();

            var userId = User.Identity.GetUserId();
            var commentUser = DbContext.Users.FirstOrDefault(u => u.Id == userId).UserName;
            var commentUserID = DbContext.Users.FirstOrDefault(u => u.Id == userId).Id;

            TicketComment comment;

            if (User.IsInRole("Admin") || User.IsInRole("Project Manager"))
            {
                comment = DbContext.TicketComments
                    .FirstOrDefault(p => p.ID == id);
            }
            else if((User.IsInRole("Developer") || User.IsInRole("Submitter"))&& userId == commentUserID)
            {
                comment = DbContext.TicketComments
                    .Where(t => t.UserID == userId)
                    .FirstOrDefault(p => p.ID == id);
            }
            else { comment = null; }

            if (comment == null)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var ticket = DbContext.Tickets
                .FirstOrDefault(p => p.ID == comment.TicketID);

            var model = new EditCommentViewModel
            {
                ID = commentId,
                TicketID = ticket.ID,
                Comment = comment.Comment,
                User = commentUser
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, int commentId, EditCommentViewModel formData)
        {
            var userId = User.Identity.GetUserId();
            var commentUserID = DbContext.Users.FirstOrDefault(u => u.UserName == formData.User).Id;

            if ((User.IsInRole("Admin") || User.IsInRole("Project Manager"))
                || (User.IsInRole("Developer") && userId == commentUserID)
                || (User.IsInRole("Submitter") && userId == commentUserID))
            {
                // continue
            }
            else
            {
                var ticket = DbContext.Tickets.FirstOrDefault(t => t.ID == formData.TicketID);
                return RedirectToAction("FullTicketBySlug", "Ticket", new { slug = ticket.Slug });
            }
            formData.ID = commentId;

            return Save(id, formData);
        }

        [Authorize]
        private ActionResult Save(int? id, EditCommentViewModel formData)
        {
            var ticket = DbContext.Tickets
                .FirstOrDefault(p => p.ID == formData.TicketID);

            if (!ModelState.IsValid)
            {
                return View();
                //return RedirectToAction("FullTicketBySlug", "Ticket", new { slug = ticket.Slug });
            }

            var userId = User.Identity.GetUserId();

            TicketComment comment;

            if (!id.HasValue)
            {
                comment = new TicketComment
                {
                    DateCreated = DateTime.Now,
                    Comment = formData.Comment,
                    TicketID = formData.TicketID,
                    UserID = userId
                };
                DbContext.TicketComments.Add(comment);
            }
            else
            {
                comment = DbContext.TicketComments.FirstOrDefault(
                    p => p.ID == formData.ID);

                if (comment == null)
                {
                    return RedirectToAction(nameof(ProjectController.Index));
                }

                if ((User.IsInRole("Admin") || User.IsInRole("Project Manager"))
                    || (User.IsInRole("Developer") && userId == comment.UserID)
                    || (User.IsInRole("Submitter") && userId == comment.UserID))
                {
                    // continue
                }
                else
                {
                    return RedirectToAction("FullTicketBySlug", "Ticket", new { slug = ticket.Slug });
                }

                comment.Comment = formData.Comment;
            }

            var notifyUsers = DbContext.TicketNotifications.Where(u => u.TicketID == ticket.ID).ToList();
            foreach (var notifyuser in notifyUsers)
            {
                var user = DbContext.Users.FirstOrDefault(u => u.Id == notifyuser.UserID);
                string alert = $"new comment on a ticket you are watching: {ticket.Title}";
                if (user == ticket.AssignedToUser)
                {
                    alert = $"new comment on a ticket assigned to you: {ticket.Title}";
                }
                var message = new IdentityMessage
                {
                    Destination = $"{user.Email}",
                    Subject = $"{alert}",
                    Body = $"{alert}"
                };
                var emailService = new EmailService();
                emailService.SendAsync(message);
            }

            DbContext.SaveChanges();

            return RedirectToAction("FullTicketBySlug", "Ticket", new { slug = ticket.Slug });
        }

        [HttpPost]
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            string currentUserID = User.Identity.GetUserId();
            var comment = DbContext.TicketComments.FirstOrDefault(p => p.ID == id);
            var ticket = DbContext.Tickets.FirstOrDefault(t => t.ID == comment.TicketID);

            if ((User.IsInRole("Admin") || User.IsInRole("Project Manager")) 
                || (User.IsInRole("Developer") && currentUserID == comment.UserID) 
                || (User.IsInRole("Submitter") && currentUserID == comment.UserID))
            {
                // continue
            }
            else
            {
                return RedirectToAction("FullTicketBySlug", "Ticket", new { slug = ticket.Slug });
            }


            if (comment != null)
            {
                DbContext.TicketComments.Remove(comment);
                DbContext.SaveChanges();
            }

            return RedirectToAction("FullTicketBySlug", "Ticket", new { slug = ticket.Slug });
        }
    }
}