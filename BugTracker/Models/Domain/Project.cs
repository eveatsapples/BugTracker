using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models.Domain
{
    public class Project
    {
        public int ID { get; set; }
        public string Slug { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ICollection<ApplicationUser> ProjectUsers { get; set; }
    }
}