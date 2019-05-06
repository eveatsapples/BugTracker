using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class NumberOfTicketsWidget
    {
        public int Number { get; set; }
        public int Open { get; set; }
        public int Resolved { get; set; }
        public int Rejected { get; set; }
    }
}