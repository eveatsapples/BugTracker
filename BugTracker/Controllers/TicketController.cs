﻿using System;
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
    public class TicketController : Controller
    {
        private ApplicationDbContext DbContext;

        public TicketController()
        {
            DbContext = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Show(int id)
        {
            var project = DbContext.Projects
                .Where(p => p.ID == id)
                .FirstOrDefault();

            var model = DbContext.Tickets
              .Where(p => p.ProjectID == id)
              .Select(p => new ShowTicketViewModel
              {
                  Slug = p.Slug,
                  ID = p.ID,
                  Title = p.Title,
                  Description = p.Description,
                  DateCreated = p.DateCreated.ToString(),
                  DateUpdated = p.DateUpdated.ToString(),

              }).ToList();

            return PartialView(model);
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

            Ticket ticket = new Ticket();
            ticket.ProjectID = projectID;
            return PartialView(ticket);
        }

        [HttpPost]
        [Authorize(Roles = "Submitter")]
        public ActionResult Create(EditTicketViewModel formData)
        {
            return Save(null, formData);
        }


        [HttpGet]
        [Authorize(Roles = "Submitter")]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var userId = User.Identity.GetUserId();

            var ticket = DbContext.Tickets.FirstOrDefault(
                p => p.ID == id && p.ID == id);

            if (ticket == null)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var model = new EditTicketViewModel
            {
                Title = ticket.Title,
                Description = ticket.Description
            };

            DbContext.SaveChanges();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Submitter")]
        public ActionResult Edit(int id, EditTicketViewModel formData)
        {
            return Save(id, formData);
        }

        [Authorize(Roles = "Submitter")]
        private ActionResult Save(int? id, EditTicketViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var userId = User.Identity.GetUserId();

            var project = DbContext.Projects
                .Where(p => p.ID == formData.ProjectID)
                .FirstOrDefault();

            Ticket ticket;

            if (!id.HasValue)
            {
                ticket = new Ticket
                {
                    DateCreated = DateTime.Now
                };
                DbContext.Tickets.Add(ticket);
            }
            else
            {
                ticket = DbContext.Tickets.FirstOrDefault(
                    p => p.ID == id);

                if (ticket == null)
                {
                    return RedirectToAction(nameof(ProjectController.Index));
                }

                ticket.Slug = ProjectController.Slugify(formData.Title);
                ticket.DateUpdated = DateTime.Now;
            }

            ticket.ProjectID = formData.ProjectID;
            ticket.Title = formData.Title;
            ticket.Description = formData.Description;

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

        [HttpPost]
        [Authorize(Roles = "Submitter")]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var ticket = DbContext.Tickets.FirstOrDefault(p => p.ID == id);

            if (ticket != null)
            {
                DbContext.Tickets.Remove(ticket);
                DbContext.SaveChanges();
            }

            return RedirectToAction(nameof(ProjectController.Index));
        }

    }
}