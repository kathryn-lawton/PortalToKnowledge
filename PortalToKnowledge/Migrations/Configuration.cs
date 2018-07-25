namespace PortalToKnowledge.Migrations
{
	using Microsoft.AspNet.Identity.EntityFramework;
	using PortalToKnowledge.Models;
	using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PortalToKnowledge.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PortalToKnowledge.Models.ApplicationDbContext context)
        {
			context.Roles.AddOrUpdate(
					 r => r.Name,
					 new IdentityRole { Name = "Student" },
					 new IdentityRole { Name = "Instructor" },
					 new IdentityRole { Name = "Admin" }
					 );

			context.MediaType.AddOrUpdate(
				t => t.Type,
					new MediaType { Type = "Video" },
					new MediaType { Type = "Document" }
					);

			context.City.AddOrUpdate(
							c => c.Name,
							new Models.City { Name = "Milwaukee" }
							);

			context.State.AddOrUpdate(
				s => s.Abbreviation,
				new Models.State { Abbreviation = "WI" }
				);

			context.Zipcode.AddOrUpdate(
				z => z.Zip,
				new Models.Zipcode { Zip = "53202" },
				new Models.Zipcode { Zip = "53203" }
				);
		}
    }
}
