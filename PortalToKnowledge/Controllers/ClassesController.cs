using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PortalToKnowledge.Models;
using PortalToKnowledge.ViewModels;

namespace PortalToKnowledge.Controllers
{
    public class ClassesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Classes
        public ActionResult Index(int? id)
        {
			if(id == null)
			{
				return View(db.Class.ToList());
			}
			else
			{
				// Get the current user Id
				// Look up in their role table
				// Get all the classes related to their account - ToList
				// return to view model

				return View(db.Class.ToList());
			}
        }

        // GET: Classes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class model = db.Class.Where(c => c.ClassId == id).Include(c => c.Instructor).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Classes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class model = db.Class.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClassId,Name")] Class model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Classes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class model = db.Class.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Class model = db.Class.Find(id);
            db.Class.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

		[HttpPost]
		public ActionResult RemoveStudent(int? id)
		{
			return RedirectToAction("Index"); // TODO: implement removing student from a class
		}

		[HttpGet]
		public ActionResult AddStudent(int? id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var model = new AddStudentViewModel()
			{
				ClassId = id.Value
			};

			var students = db.Student.Where(s => s.Classes.Where(c => c.ClassId == id).Count() == 0).ToList();

			ViewBag.StudentId =
				new SelectList((from s in students
								select new
								{
									s.StudentId,
									FullName = s.FirstName + " " + s.LastName
								}),
					"StudentId",
					"FullName",
					null);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddStudent([Bind(Include = "ClassId,StudentId")] AddStudentViewModel model)
		{
			if(model == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var foundClass = db.Class.Where(c => c.ClassId == model.ClassId).FirstOrDefault();
			var foundStudent = db.Student.Where(s => s.StudentId == model.StudentId).FirstOrDefault();

			if(foundClass == null || foundStudent == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			foundClass.Students.Add(foundStudent);
			db.SaveChanges();

			return RedirectToAction("Details", new { id = model.ClassId });
		}
    }
}
