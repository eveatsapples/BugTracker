﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Domain
{
    public class TicketComment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public int TicketID { get; set; }
        public string UserID { get; set; }
    }
}