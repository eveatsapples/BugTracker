using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Domain
{
    public class Ticket
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual Project Project { get; set; }
        public int ProjectID { get; set; }
        public virtual TicketType Type { get; set; }
        public int TypeID { get; set; }
        public virtual TicketPriority Priority { get; set; }
        public int PriorityID { get; set; }
        public virtual TicketStatus Status { get; set; }
        public int StatusID { get; set; }
        public virtual ApplicationUser OwnerUser { get; set; }
        public string OwnerUserID { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }
        public string AssignedToUserID { get; set; }
    }
}