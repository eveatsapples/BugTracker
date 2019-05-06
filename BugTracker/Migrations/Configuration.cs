namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using BugTracker.Models.Domain;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "BugTracker.Models.ApplicationDbContext";
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var roleManager =
                new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(context));

            var userManager =
                new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(context));

            if (!context.Roles.Any(p => p.Name == "Admin"))
            {
                var adminRole = new IdentityRole("Admin");
                roleManager.Create(adminRole);
            }

            ApplicationUser adminUser;

            if (!context.Users.Any(
                p => p.UserName == "admin@mybugtracker.com"))
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@mybugtracker.com",
                    Email = "admin@mybugtracker.com"
                };

                userManager.Create(adminUser, "Password-1");
            }
            else
            {
                adminUser = context
                    .Users
                    .First(p => p.UserName == "admin@myBugTracker.com");
            }

            if (!userManager.IsInRole(adminUser.Id, "Admin"))
            {
                userManager.AddToRole(adminUser.Id, "Admin");
            }

            if (!context.Roles.Any(p => p.Name == "Project Manager"))
            {
                var projectManagerRole = new IdentityRole("Project Manager");
                roleManager.Create(projectManagerRole);
            }

            ApplicationUser projectManager;

            if (!context.Users.Any(
                p => p.UserName == "projectmanager@mybugtracker.com"))
            {
                projectManager = new ApplicationUser
                {
                    UserName = "projectmanager@mybugtracker.com",
                    Email = "projectmanager@mybugtracker.com"
                };

                userManager.Create(projectManager, "Password-1");
            }
            else
            {
                projectManager = context
                    .Users
                    .First(p => p.UserName == "projectmanager@myBugTracker.com");
            }

            if (!userManager.IsInRole(projectManager.Id, "Project Manager"))
            {
                userManager.AddToRole(projectManager.Id, "Project Manager");
            }

            if (!context.Roles.Any(p => p.Name == "Developer"))
            {
                var developerRole = new IdentityRole("Developer");
                roleManager.Create(developerRole);
            }

            ApplicationUser developer;

            if (!context.Users.Any(
                p => p.UserName == "developer@mybugtracker.com"))
            {
                developer = new ApplicationUser
                {
                    UserName = "developer@mybugtracker.com",
                    Email = "developer@mybugtracker.com"
                };

                userManager.Create(developer, "Password-1");
            }
            else
            {
                developer = context.Users
                    .First(p => p.UserName == "developer@myBugTracker.com");
            }

            if (!userManager.IsInRole(developer.Id, "Admin"))
            {
                userManager.AddToRole(developer.Id, "Admin");
            }

            if (!context.Roles.Any(p => p.Name == "Submitter"))
            {
                var submitterRole = new IdentityRole("Submitter");
                roleManager.Create(submitterRole);
            }

            ApplicationUser submitter;

            if (!context.Users.Any(
                p => p.UserName == "submitter@mybugtracker.com"))
            {
                submitter = new ApplicationUser
                {
                    UserName = "submitter@mybugtracker.com",
                    Email = "submitter@mybugtracker.com"
                };

                userManager.Create(submitter, "Password-1");
            }
            else
            {
                submitter = context
                    .Users
                    .First(p => p.UserName == "admin@myBugTracker.com");
            }

            if (!userManager.IsInRole(submitter.Id, "Admin"))
            {
                userManager.AddToRole(submitter.Id, "Admin");
            }

            // Ticket Types
            if (!context.TicketTypes.Any(p => p.Name == "Bug"))
            {
                var bug = new TicketType
                {
                    Name = "Bug"
                };
                context.TicketTypes.Add(bug);
            }

            if (!context.TicketTypes.Any(p => p.Name == "Feature"))
            {
                var feature = new TicketType
                {
                    Name = "Feature"
                };
                context.TicketTypes.Add(feature);
            }

            if (!context.TicketTypes.Any(p => p.Name == "Database"))
            {
                var database = new TicketType
                {
                    Name = "Database"
                };
                context.TicketTypes.Add(database);
            }

            if (!context.TicketTypes.Any(p => p.Name == "Support"))
            {
                var support = new TicketType
                {
                    Name = "Support"
                };
                context.TicketTypes.Add(support);
            }

            // Ticket Priorities
            if (!context.TicketPriorities.Any(p => p.Name == "Low"))
            {
                var low = new TicketPriority
                {
                    Name = "Low"
                };
                context.TicketPriorities.Add(low);
            }

            if (!context.TicketPriorities.Any(p => p.Name == "Medium"))
            {
                var medium = new TicketPriority
                {
                    Name = "Medium"
                };
                context.TicketPriorities.Add(medium);
            }

            if (!context.TicketPriorities.Any(p => p.Name == "High"))
            {
                var high = new TicketPriority
                {
                    Name = "High"
                };
                context.TicketPriorities.Add(high);
            }

            // Ticket Statuses
            if (!context.TicketStatuses.Any(p => p.Name == "Open"))
            {
                var open = new TicketStatus
                {
                    Name = "Open"
                };
                context.TicketStatuses.Add(open);
            }

            if (!context.TicketStatuses.Any(p => p.Name == "Resolved"))
            {
                var resolved = new TicketStatus
                {
                    Name = "Resolved"
                };
                context.TicketStatuses.Add(resolved);
            }

            if (!context.TicketStatuses.Any(p => p.Name == "Rejected"))
            {
                var rejected = new TicketStatus
                {
                    Name = "Rejected"
                };
                context.TicketStatuses.Add(rejected);
            }
        }
    }
}
