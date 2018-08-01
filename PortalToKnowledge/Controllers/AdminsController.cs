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
	public class AdminsController : Controller
	{
		ApplicationDbContext db = new ApplicationDbContext();

		// GET: Admins
		public ActionResult Index()
		{
			var admins = db.Admin.ToList();
			return View(admins);
		}

		// GET: Admins/Create
		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "AdminId, FirstName, LastName, ApplicationUserId")] Admin admin)
		{
			if (ModelState.IsValid)
			{
				admin.ApplicationUserId = User.Identity.GetUserId();
				db.Admin.Add(admin);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(admin);
		}

		[HttpGet]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Admin admin = db.Admin.Find(id);
			if (admin == null)
			{
				return HttpNotFound();
			}
			return View(admin);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "AdminId, FirstName, LastName")] Admin admin)
		{
			if (ModelState.IsValid)
			{
				db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(admin);
		}

		public ActionResult Instructors()
		{
			var instructors = db.Instrutor.ToList();
			return View(instructors);
		}
	}
}