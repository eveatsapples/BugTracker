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
using BugTracker.Helpers;

namespace BugTracker.Controllers
{
    public class TicketController : Controller
    {
        private ApplicationDbContext DbContext;

        public TicketController()
        {
            DbContext = new ApplicationDbContext();
        }

        [Authorize(Roles="Admin, Project Manager")]
        public ActionResult ShowAll()
        {

            string currentUserID = User.Identity.GetUserId();
            var model = DbContext.Tickets
              .Where(t => t.Project.Archived == false)
              .Select(p => new ShowTicketViewModel
              {
                  Slug = p.Slug,
                  ID = p.ID,
                  CurrentUserID = currentUserID,
                  ProjectID = p.ProjectID,
                  Project = p.Project.ProjectName,
                  Title = p.Title,
                  Description = p.Description,
                  DateCreated = p.DateCreated.ToString(),
                  DateUpdated = p.DateUpdated.ToString(),
                  Status = DbContext.TicketStatuses.FirstOrDefault(t => t.ID == p.StatusID).Name,
                  Priority = DbContext.TicketPriorities.FirstOrDefault(t => t.ID == p.PriorityID).Name,
                  Type = DbContext.TicketTypes.FirstOrDefault(t => t.ID == p.TypeID).Name,
                  OwnerUser = DbContext.Users.FirstOrDefault(u => u.Id == p.OwnerUserID).UserName,
                  AssignedToUser = DbContext.Users.FirstOrDefault(u => u.Id == p.AssignedToUserID).UserName,
                  OwnerUserID = DbContext.Users.FirstOrDefault(u => u.Id == p.OwnerUserID).Id,
                  AssignedToUserID = DbContext.Users.FirstOrDefault(u => u.Id == p.AssignedToUserID).Id
              }).ToList();

            return View(model);
        }

        [Authorize]
        public ActionResult Show(int id)
        {
            string currentUserID = User.Identity.GetUserId();
            var project = DbContext.Projects
                .Where(p => p.ID == id
                && p.Archived == false)
                .FirstOrDefault();

            var model = DbContext.Tickets
              .Where(p => p.ProjectID == id)
              .Select(p => new ShowTicketViewModel
              {
                  Slug = p.Slug,
                  ID = p.ID,
                  CurrentUserID = currentUserID,
                  ProjectID = p.ProjectID,
                  Title = p.Title,
                  Description = p.Description,
                  DateCreated = p.DateCreated.ToString(),
                  DateUpdated = p.DateUpdated.ToString(),
                  Status = DbContext.TicketStatuses.FirstOrDefault(t => t.ID == p.StatusID).Name,
                  Priority = DbContext.TicketPriorities.FirstOrDefault(t => t.ID == p.PriorityID).Name,
                  Type = DbContext.TicketTypes.FirstOrDefault(t => t.ID == p.TypeID).Name,
                  OwnerUser = DbContext.Users.FirstOrDefault(u => u.Id == p.OwnerUserID).UserName,
                  AssignedToUser = DbContext.Users.FirstOrDefault(u => u.Id == p.AssignedToUserID).UserName,
                  OwnerUserID = DbContext.Users.FirstOrDefault(u => u.Id == p.OwnerUserID).Id,
                  AssignedToUserID = DbContext.Users.FirstOrDefault(u => u.Id == p.AssignedToUserID).Id
              }).ToList();

            return PartialView(model);
        }

        [Authorize(Roles="Developer, Submitter")]
        public ActionResult ShowAllMyProjectsTickets()
        {
            var userId = User.Identity.GetUserId();
            var user = DbContext.Users.FirstOrDefault(u => u.Id == userId);
            var projectIDs = user.Projects
                .Where(p => p.Archived == false)
                .Select(p => p.ID).ToList();

            var model = DbContext.Tickets
              .Where(t => projectIDs.Contains(t.ProjectID))
              .Select(p => new ShowTicketViewModel
              {
                  Slug = p.Slug,
                  ID = p.ID,
                  CurrentUserID = userId,
                  ProjectID = p.ProjectID,
                  Project = p.Project.ProjectName,
                  Title = p.Title,
                  Description = p.Description,
                  DateCreated = p.DateCreated.ToString(),
                  DateUpdated = p.DateUpdated.ToString(),
                  Status = DbContext.TicketStatuses.FirstOrDefault(t => t.ID == p.StatusID).Name,
                  Priority = DbContext.TicketPriorities.FirstOrDefault(t => t.ID == p.PriorityID).Name,
                  Type = DbContext.TicketTypes.FirstOrDefault(t => t.ID == p.TypeID).Name,
                  OwnerUser = DbContext.Users.FirstOrDefault(u => u.Id == p.OwnerUserID).UserName,
                  AssignedToUser = DbContext.Users.FirstOrDefault(u => u.Id == p.AssignedToUserID).UserName,
                  OwnerUserID = DbContext.Users.FirstOrDefault(u => u.Id == p.OwnerUserID).Id,
                  AssignedToUserID = DbContext.Users.FirstOrDefault(u => u.Id == p.AssignedToUserID).Id
              }).ToList();

            return View(model);
        }

        [Authorize(Roles="Developer")]
        public ActionResult ShowAssignedTickets()
        {
            var userId = User.Identity.GetUserId();
            var user = DbContext.Users.FirstOrDefault(u => u.Id == userId);
            var projectIDs = user.Projects.Select(p => p.ID).ToList();

            var model = DbContext.Tickets
              .Where(t => t.AssignedToUserID == userId
              && t.Project.Archived == false)
              .Select(p => new ShowTicketViewModel
              {
                  Slug = p.Slug,
                  ID = p.ID,
                  CurrentUserID = userId,
                  ProjectID = p.ProjectID,
                  Project = p.Project.ProjectName,
                  Title = p.Title,
                  Description = p.Description,
                  DateCreated = p.DateCreated.ToString(),
                  DateUpdated = p.DateUpdated.ToString(),
                  Status = DbContext.TicketStatuses.FirstOrDefault(t => t.ID == p.StatusID).Name,
                  Priority = DbContext.TicketPriorities.FirstOrDefault(t => t.ID == p.PriorityID).Name,
                  Type = DbContext.TicketTypes.FirstOrDefault(t => t.ID == p.TypeID).Name,
                  OwnerUser = DbContext.Users.FirstOrDefault(u => u.Id == p.OwnerUserID).UserName,
                  AssignedToUser = DbContext.Users.FirstOrDefault(u => u.Id == p.AssignedToUserID).UserName,
                  OwnerUserID = DbContext.Users.FirstOrDefault(u => u.Id == p.OwnerUserID).Id,
                  AssignedToUserID = DbContext.Users.FirstOrDefault(u => u.Id == p.AssignedToUserID).Id
              }).ToList();

            return View(model);
        }

        [Authorize(Roles="Submitter")]
        public ActionResult TicketsIMade()
        {
            var userId = User.Identity.GetUserId();
            var user = DbContext.Users.FirstOrDefault(u => u.Id == userId);
            var projectIDs = user.Projects.Select(p => p.ID).ToList();

            var model = DbContext.Tickets
              .Where(t => t.OwnerUserID == userId
              && t.Project.Archived == false)
              .Select(p => new ShowTicketViewModel
              {
                  Slug = p.Slug,
                  ID = p.ID,
                  CurrentUserID = userId,
                  ProjectID = p.ProjectID,
                  Project = p.Project.ProjectName,
                  Title = p.Title,
                  Description = p.Description,
                  DateCreated = p.DateCreated.ToString(),
                  DateUpdated = p.DateUpdated.ToString(),
                  Status = DbContext.TicketStatuses.FirstOrDefault(t => t.ID == p.StatusID).Name,
                  Priority = DbContext.TicketPriorities.FirstOrDefault(t => t.ID == p.PriorityID).Name,
                  Type = DbContext.TicketTypes.FirstOrDefault(t => t.ID == p.TypeID).Name,
                  OwnerUser = DbContext.Users.FirstOrDefault(u => u.Id == p.OwnerUserID).UserName,
                  OwnerUserID = DbContext.Users.FirstOrDefault(u => u.Id == p.OwnerUserID).Id,
                  AssignedToUser = DbContext.Users.FirstOrDefault(u => u.Id == p.AssignedToUserID).UserName,
                  AssignedToUserID = DbContext.Users.FirstOrDefault(u => u.Id == p.AssignedToUserID).Id
              }).ToList();

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Submitter")]
        public ActionResult Create(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            int projectID = id.GetValueOrDefault();
            var userId = User.Identity.GetUserId();
            var projectUser = DbContext.Users.Any(u => u.Projects.Any(p => p.ID == projectID) && u.Id == userId);
            if (projectUser == false)
            {
                return View();
            }

            var ticket = new EditTicketViewModel();
            ticket.ProjectID = projectID;
            return PartialView(ticket);
        }

        [HttpPost]
        [Authorize(Roles = "Submitter")]
        public ActionResult Create(EditTicketViewModel formData)
        {

            int projectID = formData.ID;
            var userId = User.Identity.GetUserId();
            var projectUser = DbContext.Users.Any(u => u.Projects.Any(p => p.ID == projectID) && u.Id == userId);
            if (projectUser == false)
            {
                return View();
            }
            formData.Status = "Open";
            return Save(null, formData);
        }

        [HttpGet]
        [Authorize(Roles="Submitter")]
        public ActionResult EditAsSubmitter(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var userId = User.Identity.GetUserId();

            var ticket = DbContext.Tickets
                .Where(t => t.OwnerUserID == userId)
                .FirstOrDefault(p => p.ID == id);

            if (ticket == null)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var project = DbContext.Projects
                .FirstOrDefault(p => p.ID == ticket.ProjectID);

            var model = new EditTicketViewModel
            {
                Title = ticket.Title,
                Description = ticket.Description,
                ID = ticket.ID,
                ProjectID = ticket.ProjectID,
                Type = DbContext.TicketTypes.FirstOrDefault(t => t.ID == ticket.TypeID).Name,
                Priority = DbContext.TicketPriorities.FirstOrDefault(t => t.ID == ticket.PriorityID).Name,
                Status = DbContext.TicketStatuses.FirstOrDefault(t => t.ID == ticket.StatusID).Name
            };

            DbContext.SaveChanges();
            return View("Edit", model);
            //return RedirectToAction("FullProjectBySlug", "Project", new { slug = project.Slug });
        }

        [HttpGet]
        [Authorize(Roles="Admin, Project Manager")]
        public ActionResult EditAsAdminOrProjectManager(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var userId = User.Identity.GetUserId();

            var ticket = DbContext.Tickets
                .FirstOrDefault(p => p.ID == id);

            if (ticket == null)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var project = DbContext.Projects
                .FirstOrDefault(p => p.ID == ticket.ProjectID);

            var model = new EditTicketViewModel
            {
                Title = ticket.Title,
                Description = ticket.Description,
                ID = ticket.ID,
                ProjectID = ticket.ProjectID,
                Type = DbContext.TicketTypes.FirstOrDefault(t => t.ID == ticket.TypeID).Name,
                Priority = DbContext.TicketPriorities.FirstOrDefault(t => t.ID == ticket.PriorityID).Name,
                Status = DbContext.TicketStatuses.FirstOrDefault(t => t.ID == ticket.StatusID).Name
            };

            DbContext.SaveChanges();
            return View("Edit", model);
            //return RedirectToAction("FullProjectBySlug", "Project", new { slug = project.Slug });
        }

        [HttpGet]
        [Authorize(Roles="Developer")]
        public ActionResult EditAsDeveloper(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var userId = User.Identity.GetUserId();

            var ticket = DbContext.Tickets
                .Where(t => t.AssignedToUserID == userId)
                .FirstOrDefault(p => p.ID == id);

            if (ticket == null)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var project = DbContext.Projects
                .FirstOrDefault(p => p.ID == ticket.ProjectID);

            var model = new EditTicketViewModel
            {
                Title = ticket.Title,
                Description = ticket.Description,
                ID = ticket.ID,
                ProjectID = ticket.ProjectID,
                Type = DbContext.TicketTypes.FirstOrDefault(t => t.ID == ticket.TypeID).Name,
                Priority = DbContext.TicketPriorities.FirstOrDefault(t => t.ID == ticket.PriorityID).Name,
                Status = DbContext.TicketStatuses.FirstOrDefault(t => t.ID == ticket.StatusID).Name
            };

            DbContext.SaveChanges();
            return View("Edit", model);
            //return RedirectToAction("FullProjectBySlug", "Project", new { slug = project.Slug });
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, int projectId, EditTicketViewModel formData)
        {
            return Save(id, formData);
        }

        [Authorize]
        private ActionResult Save(int? id, EditTicketViewModel formData)
        {
            var project = DbContext.Projects
                .FirstOrDefault(p => p.ID == formData.ProjectID);

            if (!ModelState.IsValid)
            {
                //return View(formData.ProjectID);
                return RedirectToAction("FullProjectBySlug", "Project", new { slug = project.Slug });
            }

            var userId = User.Identity.GetUserId();
            
            var ticketType = DbContext.TicketTypes.FirstOrDefault(t => t.Name == formData.Type);

            var ticketPriority = DbContext.TicketPriorities.FirstOrDefault(t => t.Name == formData.Priority);

            Ticket ticket;

            if (!id.HasValue)
            {
                ticket = new Ticket
                {
                    DateCreated = DateTime.Now,
                    OwnerUserID = userId,
                    Slug = ProjectController.Slugify(formData.Title),
                    ProjectID = formData.ProjectID
                };
                DbContext.Tickets.Add(ticket);
            }
            else
            {
                ticket = DbContext.Tickets.FirstOrDefault(
                    p => p.ID == id);

                if ((User.IsInRole("Admin") || User.IsInRole("Project Manager")) 
                    || (User.IsInRole("Developer") && userId == ticket.AssignedToUserID) 
                    || (User.IsInRole("Submitter") && userId == ticket.OwnerUserID))
                {
                    // continue
                }
                else
                {
                    return RedirectToAction("FullProjectBySlug", "Project", new { slug = project.Slug });
                }

                if (ticket == null)
                {
                    return RedirectToAction(nameof(ProjectController.Index));
                }

                ticket.DateUpdated = DateTime.Now;

                var historyWriter = new CustomHelpers();
                historyWriter.MakeTicketHistories(ticket, formData, userId);

                var notifyUsers = DbContext.TicketNotifications.Where(u => u.TicketID == ticket.ID).ToList();
                foreach (var notifyuser in notifyUsers)
                {
                    var user = DbContext.Users.FirstOrDefault(u => u.Id == notifyuser.UserID);
                    string alert = $"there has been a change to a ticket you are watching: {ticket.Title}";
                    if (user == ticket.AssignedToUser)
                    {
                        alert = $"there has been a change to a ticket assign to you: {ticket.Title}";
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
            }

            ticket.Title = formData.Title;
            ticket.Description = formData.Description;
            ticket.TypeID = ticketType.ID;
            ticket.PriorityID = ticketPriority.ID;
            ticket.StatusID = DbContext.TicketStatuses.FirstOrDefault(t => t.Name == formData.Status).ID;

            DbContext.SaveChanges();

            return RedirectToAction("FullProjectBySlug", "Project", new { slug = project.Slug });
        }

        [HttpGet]
        //[Route("{slug}")]
        public ActionResult FullTicketBySlug(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var userId = User.Identity.GetUserId();

            var ticket = DbContext.Tickets.FirstOrDefault(p =>
            p.Slug == slug);

            if (ticket == null)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var isUserWatching = DbContext.TicketNotifications.FirstOrDefault(u => u.TicketID == ticket.ID && u.UserID == userId);
            var model = new FullTicketViewModel
            {
                ID = ticket.ID,
                ProjectID = ticket.ProjectID,
                UserID = userId,
                Title = ticket.Title,
                Slug = ticket.Slug,
                Description = ticket.Description,
                DateCreated = ticket.DateCreated.ToString(),
                DateUpdated = ticket.DateUpdated.ToString(),
                Type = ticket.Type.Name,
                Priority = ticket.Priority.Name,
                Status = ticket.Status.Name,
                OwnerUser = ticket.OwnerUser.UserName,
                OwnerUserID = ticket.OwnerUserID
            };
            if (ticket.AssignedToUserID != null)
            {
                model.AssignedToUser = ticket.AssignedToUser.UserName;
                model.AssignedToUserID = ticket.AssignedToUserID;
            }
            if (isUserWatching != null)
            {
                model.Watching = true;
            }

            return View("FullTicket", model);
        }

        [HttpPost]
        [Authorize]
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