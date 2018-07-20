namespace PortalToKnowledge.Migrations
{
	using Microsoft.AspNet.Identity.EntityFramework;
	using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PortalToKnowledge.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "PortalToKnowledge.Models.ApplicationDbContext";
        }

        protected override void Seed(PortalToKnowledge.Models.ApplicationDbContext context)
        {
			context.Roles.AddOrUpdate(
				  r => r.Name,
					  new IdentityRole { Name = "Student" },
					  new IdentityRole { Name = "Instructor" },
					  new IdentityRole { Name = "Admin"}
					  );
		}
    }
}
