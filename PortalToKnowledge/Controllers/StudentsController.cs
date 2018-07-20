using Microsoft.AspNet.Identity;
using PortalToKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalToKnowledge.Controllers
{
    public class StudentsController : Controller
    {
		ApplicationDbContext db = new ApplicationDbContext();

        // GET: Students
        public ActionResult Index()
        {
            return View();
        }

		// GET: Students/Create
		[HttpGet]
		public ActionResult Create()
		{
			Student student = new Student();

			return View(student);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "StudentId, FirstName, LastName")] Student student)
		{
			if (ModelState.IsValid)
			{
				student.ApplicationUserId = User.Identity.GetUserId();
				db.Student.Add(student);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(student);
		}
	}
}