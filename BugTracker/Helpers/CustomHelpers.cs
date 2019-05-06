using BugTracker.Models;
using BugTracker.Models.Domain;
using BugTracker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class CustomHelpers
    {
        private ApplicationDbContext DbContext;

        public CustomHelpers()
        {
            DbContext = new ApplicationDbContext();
        }

        public void MakeTicketHistories(Ticket ticket, EditTicketViewModel formData, string userId)
        {
            var newTicketType = DbContext.TicketTypes.FirstOrDefault(t => t.Name == formData.Type);
            var newTicketPriority = DbContext.TicketPriorities.FirstOrDefault(t => t.Name == formData.Priority);
            var newTicketStatus = DbContext.TicketStatuses.FirstOrDefault(t => t.Name == formData.Status);

            if(ticket.Title != formData.Title)
            {
                var history = new TicketHistory
                {
                    TicketID = ticket.ID,
                    Property = "Title",
                    OldValue = ticket.Title,
                    NewValue = formData.Title,
                    Changed = DateTime.Now,
                    UserID = userId
                };
                DbContext.TicketHistories.Add(history);
            }
            if(ticket.Description != formData.Description)
            {
                var history = new TicketHistory
                {
                    TicketID = ticket.ID,
                    Property = "Description",
                    OldValue = ticket.Description,
                    NewValue = formData.Description,
                    Changed = DateTime.Now,
                    UserID = userId
                };
                DbContext.TicketHistories.Add(history);
            }
            if(ticket.TypeID != newTicketType.ID)
            {
                var history = new TicketHistory
                {
                    TicketID = ticket.ID,
                    Property = "Type",
                    OldValue = ticket.Type.Name,
                    NewValue = formData.Type,
                    Changed = DateTime.Now,
                    UserID = userId
                };
                DbContext.TicketHistories.Add(history);
            }
            if(ticket.PriorityID != newTicketPriority.ID)
            {
                var history = new TicketHistory
                {
                    TicketID = ticket.ID,
                    Property = "Priority",
                    OldValue = ticket.Priority.Name,
                    NewValue = formData.Priority,
                    Changed = DateTime.Now,
                    UserID = userId
                };
                DbContext.TicketHistories.Add(history);
            }
            if(ticket.StatusID != newTicketStatus.ID)
            {
                var history = new TicketHistory
                {
                    TicketID = ticket.ID,
                    Property = "Status",
                    OldValue = ticket.Status.Name,
                    NewValue = formData.Status,
                    Changed = DateTime.Now,
                    UserID = userId
                };
                DbContext.TicketHistories.Add(history);
            }
            DbContext.SaveChanges();
        }

        public void MakeTicketHistories(Ticket ticket, ApplicationUser newAssignedUser, string userId)
        {
            if (ticket.AssignedToUserID != newAssignedUser.Id)
            {
                var history = new TicketHistory
                {
                    TicketID = ticket.ID,
                    Property = "Assigned User",
                    NewValue = newAssignedUser.UserName,
                    Changed = DateTime.Now,
                    UserID = userId
                };
                if (ticket.AssignedToUser != null)
                {
                    history.OldValue = ticket.AssignedToUser.UserName;
                }
                else
                {
                    history.OldValue = "";
                }
                DbContext.TicketHistories.Add(history);
            }
            DbContext.SaveChanges();
        }
        public void MakeTicketHistories(Ticket ticket, string userId)
        {
            var history = new TicketHistory
            {
                TicketID = ticket.ID,
                Property = "Assigned User",
                OldValue = ticket.AssignedToUser.UserName,
                NewValue = "", 
                Changed = DateTime.Now,
                UserID = userId
            };
            DbContext.TicketHistories.Add(history);
            DbContext.SaveChanges();
        }
    }
}