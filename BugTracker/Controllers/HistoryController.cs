using BugTracker.Models;
using BugTracker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class HistoryController : Controller
    {
        private ApplicationDbContext DbContext;

        public HistoryController()
        {
            DbContext = new ApplicationDbContext();
        }

        public ActionResult Show(int Id)
        {
            var histories = DbContext.TicketHistories.Where(h => h.TicketID == Id)
                .Select(h => new TicketHistoryViewModel
                {
                    ID = h.ID,
                    TicketID = h.TicketID,
                    Property = h.Property,
                    OldValue = h.OldValue,
                    NewValue = h.NewValue,
                    Changed = h.Changed.ToString(),
                    User = h.User.UserName
                }).ToList();
            return PartialView(histories);
        }
    }
}