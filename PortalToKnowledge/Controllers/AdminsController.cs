using Microsoft.AspNet.Identity;
using PortalToKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
	}
}