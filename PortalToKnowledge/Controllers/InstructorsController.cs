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

			model.Instructors.Add(foundInstructor);
			db.Class.Add(model);
			db.SaveChanges();

			return RedirectToAction("Index", "Classes");
			//return RedirectToAction("Details", "Class", new { id = model.ClassId });
		}

		[HttpGet]
		public ActionResult CreateMedia()
		{
			return View();
		}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult CreateMedia(Media model)
		//{
		//	var currentUserId = User.Identity.GetUserId();
		//	var foundInstructor = db.Instrutor.Where(i => i.ApplicationUserId == currentUserId).FirstOrDefault();

		//	if (foundInstructor == null)
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	}

		//	foundInstructor.Classes.ToList();
		//	//get class instructor wants to add link to
		//	//instructor selects type from drop dowm
		//	//instructor adds
		}

	}
}