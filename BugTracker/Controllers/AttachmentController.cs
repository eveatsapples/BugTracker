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
            var model = DbContext.TicketAttachments
              .Select(p => new IndexAttachmentViewModel
              {
                  ID = p.ID,
                  TicketID = p.TicketID,
                  FilePath = p.FilePath,
                  Description = p.Description,
                  Created = p.Created.ToString(),
                  User = p.User.UserName,
                  FileUrl = p.FileUrl
              }).ToList();


            return View(model);
        }

        public ActionResult ShowTicketAttachments(int Id)
        {
            var model = DbContext.TicketAttachments
              .Where(a => a.TicketID == Id)
              .Select(p => new IndexAttachmentViewModel
              {
                  ID = p.ID,
                  TicketID = p.TicketID,
                  FilePath = p.FilePath,
                  Description = p.Description,
                  Created = p.Created.ToString(),
                  User = p.User.UserName,
                  FileUrl = p.FileUrl
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

            DbContext.SaveChanges();
            return RedirectToAction("FullTicketBySlug", "Ticket", new { slug = ticket.Slug });
        }
    }
}