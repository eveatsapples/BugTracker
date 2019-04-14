using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models.ViewModels
{
    public class EditTicketViewModel
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string TypeID { get; set; }
        public string PriorityID { get; set; }
        public string StatusID { get; set; }
        public string OwnerUserID { get; set; }
        public string AssignedToUserID { get; set; }
    }
}