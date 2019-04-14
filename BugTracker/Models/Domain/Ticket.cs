using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Domain
{
    public class Ticket
    {
        public int ID { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int ProjectID { get; set; }
        public string TypeID { get; set; }
        public string PriorityID { get; set; }
        public string StatusID { get; set; }
        public string OwnerUserID { get; set; }
        public string AssignedToUserID { get; set; }
    }
}