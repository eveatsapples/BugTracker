using BugTracker.Models;
using BugTracker.Models.Domain;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class TicketNotificationController : Controller
    {
        private ApplicationDbContext DbContext;

        public TicketNotificationController()
        {
            DbContext = new ApplicationDbContext();
        }

        [HttpPost]
        public ActionResult Watch(int id)
        {
            var userId = User.Identity.GetUserId();
            var ticketNotification = new TicketNotification
            {
                UserID = userId,
                TicketID = id
            };
            DbContext.TicketNotifications.Add(ticketNotification);

            DbContext.SaveChanges();
            var ticket = DbContext.Tickets.FirstOrDefault(t => t.ID == id);
            return RedirectToAction("FullTicketBySlug", "Ticket", new { slug = ticket.Slug });
        }

        [HttpPost]
        public ActionResult UnWatch(int id)
        {
            var userId = User.Identity.GetUserId();
            var ticketNotification = DbContext.TicketNotifications.FirstOrDefault(n => n.UserID == userId && n.TicketID == id);
            DbContext.TicketNotifications.Remove(ticketNotification);

            DbContext.SaveChanges();
            var ticket = DbContext.Tickets.FirstOrDefault(t => t.ID == id);
            return RedirectToAction("FullTicketBySlug", "Ticket", new { slug = ticket.Slug });
        }
    }
}