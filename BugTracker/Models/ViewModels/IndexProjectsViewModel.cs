using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models.ViewModels
{
    public class IndexProjectsViewModel
    {
        public int ID { get; set; }
        public string Slug { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string DateCreated { get; set; }
        public string DateUpdated { get; set; }
        public int ProjectUsers { get; set; }
        public int Tickets { get; set; }
    }
}