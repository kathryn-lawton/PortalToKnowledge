using Microsoft.AspNet.Identity;
using PortalToKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalToKnowledge.Controllers
{
	public class HomeController : Controller
	{
		ApplicationDbContext db = new ApplicationDbContext();

		public ActionResult Index()
		{
			List<Assignment> assignmentsDue = new List<Assignment>();
			if(User.IsInRole("Student"))
			{
				var currentUserId = User.Identity.GetUserId();
				var foundStudent = db.Student.Where(s => s.ApplicationUserId == currentUserId).FirstOrDefault();
				if(foundStudent != null)
				{
					foreach (var courses in foundStudent.Courses)
					{
						foreach (var assignment in courses.Assignments)
						{
							DateTime dueDate = DateTime.Parse(assignment.DueDate);
							var timeLeft = dueDate - DateTime.Now;

							if (timeLeft.Days < 2)
							{
								assignmentsDue.Add(assignment);
							}
						}
					}
				}
			}

			return View(assignmentsDue);
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}