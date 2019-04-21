using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class AddFileViewModel
    {
        public int TicketID { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public HttpPostedFileBase File { get; set; }
    }
}