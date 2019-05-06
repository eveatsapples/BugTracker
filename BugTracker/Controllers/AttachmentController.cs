using BugTracker.Models;
using BugTracker.Models.Domain;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class AttachmentController : Controller
    {
        private ApplicationDbContext DbContext;

        public AttachmentController()
        {
            DbContext = new ApplicationDbContext();
        }

        public ActionResult ShowAllAttachments()
        {
            string currentUserID = User.Identity.GetUserId();

            var model = DbContext.TicketAttachments
              .Where(a => a.Ticket.Project.Archived == false)
              .Select(p => new IndexAttachmentViewModel
              {
                  ID = p.ID,
                  TicketID = p.TicketID,
                  FilePath = p.FilePath,
                  Description = p.Description,
                  Created = p.Created.ToString(),
                  User = p.User.UserName,
                  UserID = p.User.Id,
                  FileUrl = p.FileUrl,
                  CurrentUserID = currentUserID
              }).ToList();


            return View(model);
        }

        public ActionResult ShowTicketAttachments(int Id)
        {
            string currentUserID = User.Identity.GetUserId();

            var model = DbContext.TicketAttachments
              .Where(a => a.TicketID == Id
              && a.Ticket.Project.Archived == false)
              .Select(p => new IndexAttachmentViewModel
              {
                  ID = p.ID,
                  TicketID = p.TicketID,
                  FilePath = p.FilePath,
                  Description = p.Description,
                  Created = p.Created.ToString(),
                  User = p.User.UserName,
                  UserID = p.User.Id,
                  FileUrl = p.FileUrl,
                  CurrentUserID = currentUserID
              }).ToList();


            return PartialView(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Add(int Id)
        {
            var userId = User.Identity.GetUserId();

            var ticket = DbContext.Tickets.FirstOrDefault(t => t.ID == Id);

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


            var model = new AddFileViewModel
            {
                TicketID = Id
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add(AddFileViewModel formData)
        {
            var userId = User.Identity.GetUserId();

            var ticket = DbContext.Tickets.FirstOrDefault(t => t.ID == formData.TicketID);

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

            if (formData.File != null)
            {
                if (!Directory.Exists(Constants.MappedUploadFolder))
                {
                    Directory.CreateDirectory(Constants.MappedUploadFolder);
                }

                var fileName = formData.File.FileName;
                var fullPathWithName = Constants.MappedUploadFolder + fileName;

                formData.File.SaveAs(fullPathWithName);

                var attachment = new TicketAttachment
                {
                    TicketID = formData.TicketID,
                    FilePath = fullPathWithName, 
                    Description = formData.Description,
                    Created = DateTime.Now, 
                    UserID = userId, 
                    FileUrl = Constants.UploadFolder + fileName,
                };

                DbContext.TicketAttachments.Add(attachment);
            }

            var notifyUsers = DbContext.TicketNotifications.Where(u => u.TicketID == ticket.ID).ToList();
            foreach (var notifyuser in notifyUsers)
            {
                var user = DbContext.Users.FirstOrDefault(u => u.Id == notifyuser.UserID);
                string alert = $"new attachment on a ticket you are watching: {ticket.Title}";
                if (user == ticket.AssignedToUser)
                {
                    alert = $"new attachment on a ticket assigned to you: {ticket.Title}";
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
            var attachment = DbContext.TicketAttachments.FirstOrDefault(p => p.ID == id);
            var ticket = DbContext.Tickets.FirstOrDefault(t => t.ID == attachment.TicketID);

            if ((User.IsInRole("Admin") || User.IsInRole("Project Manager")) 
                || (User.IsInRole("Developer") && currentUserID == attachment.UserID) 
                || (User.IsInRole("Submitter") && currentUserID == attachment.UserID))
            {
                // continue
            }
            else
            {
                return RedirectToAction("FullTicketBySlug", "Ticket", new { slug = ticket.Slug });
            }


            if (attachment != null)
            {
                DbContext.TicketAttachments.Remove(attachment);
                DbContext.SaveChanges();
            }

            return RedirectToAction("FullTicketBySlug", "Ticket", new { slug = ticket.Slug });
        }
    }
}