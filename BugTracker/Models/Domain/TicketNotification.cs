﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Domain
{
    public class TicketNotification
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int TicketID { get; set; }
        public virtual Ticket Ticket { get; set; }
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}