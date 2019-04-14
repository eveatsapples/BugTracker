using BugTracker.Models.Domain;
using BugTracker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.App_Start
{
    public class AutoMapperConfig
    {
        public static void Init()
        {
            AutoMapper.Mapper.Initialize(p  => {
                p.CreateMap<Ticket, ShowTicketViewModel>();
            });
        }
    }
}