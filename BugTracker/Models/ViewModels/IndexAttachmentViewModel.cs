using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class IndexAttachmentViewModel
    {
        public int ID { get; set; }
        public int TicketID { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public string Created { get; set; }
        public string User { get; set; }
        public string UserID { get; set; }
        public string CurrentUserID { get; set; }
        public string FileUrl { get; set; }
    }
}