using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models.ViewModels
{
    public class EditTicketViewModel
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [AllowHtml]
        public string Description { get; set; }
        [Required]
        public string TypeID { get; set; }
        [Required]
        public string PriorityID { get; set; }
        public string StatusID { get; set; }
        public string OwnerUserID { get; set; }
        public string AssignedToUserID { get; set; }
    }
}