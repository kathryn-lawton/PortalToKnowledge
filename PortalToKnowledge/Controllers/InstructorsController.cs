using Microsoft.AspNet.Identity;
using PortalToKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalToKnowledge.Controllers
{
    public class InstructorsController : Controller
    {
		ApplicationDbContext db = new ApplicationDbContext();
		
		// GET: Instructors
        public ActionResult Index()
        {
			var instructors = db.Instrutor.ToList();
            return View(instructors);
        }

		// GET: Instructors/Create
		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "InstructorId, FirstName, LastName")] Instructor instructor)
		{
			if (ModelState.IsValid)
			{
				instructor.ApplicationUserId = User.Identity.GetUserId();
				db.Instrutor.Add(instructor);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(instructor);
		}
	}
}