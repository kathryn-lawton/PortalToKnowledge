using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PortalToKnowledge.Models;

namespace PortalToKnowledge.Controllers
{
    public class ClassTasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ClassTasks
        public ActionResult Index()
        {
            var classTask = db.ClassTask.Include(c => c.Class);
            return View(classTask.ToList());
        }

        // GET: ClassTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassTask classTask = db.ClassTask.Find(id);
            if (classTask == null)
            {
                return HttpNotFound();
            }
            return View(classTask);
        }

        // GET: ClassTasks/Create
        public ActionResult Create(int? id)
        {
			if(id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var classTask = new ClassTask()
			{
				ClassId = id.Value
			};

			ViewBag.MediaTypeId = new SelectList(db.MediaType, "Id", "Type", classTask.MediaTypeId);

			return View(classTask);
        }

        // POST: ClassTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClassTaskId,TaskName, Link, ClassId,MediaTypeId")] ClassTask classTask)
        {
            if (ModelState.IsValid)
            {
                db.ClassTask.Add(classTask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(classTask);
        }

        // GET: ClassTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassTask classTask = db.ClassTask.Find(id);
            if (classTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassId = new SelectList(db.Class, "ClassId", "Name", classTask.ClassId);
            return View(classTask);
        }

        // POST: ClassTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClassTaskId,TaskName,ClassId")] ClassTask classTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassId = new SelectList(db.Class, "ClassId", "Name", classTask.ClassId);
            return View(classTask);
        }

        // GET: ClassTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassTask classTask = db.ClassTask.Find(id);
            if (classTask == null)
            {
                return HttpNotFound();
            }
            return View(classTask);
        }

        // POST: ClassTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClassTask classTask = db.ClassTask.Find(id);
            db.ClassTask.Remove(classTask);
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
    }
}
