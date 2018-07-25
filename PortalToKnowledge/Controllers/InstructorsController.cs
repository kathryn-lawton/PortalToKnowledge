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

		[HttpGet]
		public ActionResult CreateClass()
		{
			return View();
		}

		[HttpPost]
		public ActionResult CreateClass(Class model)
		{
			var currentUserId = User.Identity.GetUserId();
			var foundInstructor = db.Instrutor.Where(i => i.ApplicationUserId == currentUserId).FirstOrDefault();

			if(foundInstructor == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			model.InstructorId = foundInstructor.InstructorId;
			db.Class.Add(model);
			db.SaveChanges();

			return RedirectToAction( "ViewClasses");
		}

		[HttpGet]
		public ActionResult AddMedia(int? id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var model = new Media() { ClassId = id.Value };
			ViewBag.MediaTypeId = new SelectList(db.MediaType, "Id", "Type");
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddMedia(Media model)
		{
			var currentUserId = User.Identity.GetUserId();
			var foundInstructor = db.Instrutor.Where(i => i.ApplicationUserId == currentUserId).FirstOrDefault();

			if (foundInstructor == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			db.Media.Add(model);
			db.SaveChanges();

			return RedirectToAction("ViewClasses"); // View Class Media
		}

		[HttpGet]
		public ActionResult ViewClasses()
		{
			var currentUserId = User.Identity.GetUserId();
			var foundInstructor = db.Instrutor.Where(i => i.ApplicationUserId == currentUserId).FirstOrDefault();

			return View(foundInstructor.Classes.ToList());
		}

		[HttpGet]
		public ActionResult ViewStudents()
		{
			var currentUserId = User.Identity.GetUserId();
			var foundInstructor = db.Instrutor.Where(i => i.ApplicationUserId == currentUserId).FirstOrDefault();

			return View(foundInstructor.Classes.ToList());
		}

		[HttpGet]
		public ActionResult AddStudent(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var model = db.Student.ToList();
			return View(model);
		}

		[HttpGet]
		public ActionResult AddStudent()
		{
			var currentUserId = User.Identity.GetUserId();
			var foundInstructor = db.Instrutor.Where(i => i.ApplicationUserId == currentUserId).FirstOrDefault();

			if(foundInstructor == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var students = db.Student.ToList(); //.Where(s => s.Instructors.Where(i => i.InstructorId == foundInstructor.InstructorId).Count() == 0).ToList();
			return View(students);
		}

		[HttpPost]
		public ActionResult AddStudent(Student model)
		{
			var currentUserId = User.Identity.GetUserId();
			var foundInstructor = db.Instrutor.Where(i => i.ApplicationUserId == currentUserId).FirstOrDefault();

			if (model == null || foundInstructor == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			// foundInstructor.Students.Add(model);
			db.SaveChanges();

			return RedirectToAction("ViewStudents");
		}

		[HttpGet]
		public ActionResult Details(int? id)
		{
			Instructor instructor;
			if (id == null)
			{
				var currentUserId = User.Identity.GetUserId();
				if (currentUserId == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				instructor = db.Instrutor.Where(s => s.ApplicationUserId == currentUserId).FirstOrDefault();
			}
			else
			{
				instructor = db.Instrutor.Where(s => s.InstructorId == id).FirstOrDefault();
			}

			if (instructor == null)
			{
				return HttpNotFound();
			}
			return View(instructor);
		}
	}


}
