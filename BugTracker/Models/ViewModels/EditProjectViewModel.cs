using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BugTracker.Models.ViewModels
{
    public class EditProjectViewModel
    {
        [Required]
        public string ProjectName { get; set; }

        [Required]
        [AllowHtml]
        public string Description { get; set; }
    }
}