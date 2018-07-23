using Microsoft.AspNet.Identity;
using PortalToKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
			var students = db.Student.ToList();	
			return View(students);
        }

		// GET: Students/Create
		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "StudentId, FirstName, LastName, ApplicationUserId")] Student student)
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

		public ActionResult Details(int? id)
		{
			Student student;
			if (id == null)
			{
				var currentUserId = User.Identity.GetUserId();
				if(currentUserId == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				student = db.Student.Where(s => s.ApplicationUserId == currentUserId).FirstOrDefault();
			}
			else
			{
				student = db.Student.Where(s => s.StudentId == id).FirstOrDefault();
			}

			if (student == null)
			{
				return HttpNotFound();
			}
			return View(student);
		}

		//public ActionResult Edit()
		//{

		//}

		public ActionResult Classes()
		{
			return View();
		}

		public ActionResult Flashcards()
		{
			return View();
		}

		public ActionResult Notes()
		{
			return View();
		}
	}
}