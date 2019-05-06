using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class FullTicketViewModel
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string DateCreated { get; set; }
        public string DateUpdated { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string OwnerUser { get; set; }
        public string OwnerUserID { get; set; }
        public string AssignedToUser { get; set; }
        public string AssignedToUserID { get; set; }
        public bool Watching { get; set; }
    }
}