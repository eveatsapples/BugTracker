using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Domain
{
    public class TicketComment
    {
        public string ID { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
        public string TicketID { get; set; }
        public string UserID { get; set; }
    }
}