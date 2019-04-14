using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Domain
{
    public class TicketAttachment
    {
        public string ID { get; set; }
        public string TicketID { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string UserID { get; set; }
        public string FileUrl { get; set; }
    }
}