using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class ShowCommentViewModel
    {
        public int ID { get; set; }
        public int TicketID { get; set; }
        public string Comment { get; set; }
        public string DateCreated { get; set; }
        public string User { get; set; }
    }
}