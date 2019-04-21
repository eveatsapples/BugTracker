using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class EditCommentViewModel
    {
        public int ID { get; set; }
        public int TicketID { get; set; }
        public string Comment { get; set; }
        public string User { get; set; }
    }
}