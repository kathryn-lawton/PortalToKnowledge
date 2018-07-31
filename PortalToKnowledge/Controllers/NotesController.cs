using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PortalToKnowledge.Models;

namespace PortalToKnowledge.Controllers
{
    public class NotesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Notes
        public ActionResult Index()
        {
			var currentUserId = User.Identity.GetUserId();
			var currentStudent = db.Student.Where(s => s.ApplicationUserId == currentUserId).FirstOrDefault();
			if (currentStudent == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var test = currentStudent.Notes.ToList();
			var notes = db.Note.Where(n => n.StudentId == currentStudent.StudentId).Include(n => n.Assignment).Include(n => n.Student).ToList();
            return View(notes);
        }

        // GET: Notes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Notes/Create
        public ActionResult Create()
        {
            ViewBag.AssignmentId = new SelectList(db.Assignment, "AssignmentId", "Name");
            ViewBag.StudentId = new SelectList(db.Student, "StudentId", "FirstName");
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NoteId,Timestamp,Content,StudentId,AssignmentId")] Note note)
        {
            if (ModelState.IsValid)
            {
                db.Note.Add(note);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignmentId = new SelectList(db.Assignment, "AssignmentId", "Name", note.AssignmentId);
            ViewBag.StudentId = new SelectList(db.Student, "StudentId", "FirstName", note.StudentId);
            return View(note);
        }

        // GET: Notes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignmentId = new SelectList(db.Assignment, "AssignmentId", "Name", note.AssignmentId);
            ViewBag.StudentId = new SelectList(db.Student, "StudentId", "FirstName", note.StudentId);
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NoteId,Timestamp,Content,StudentId,AssignmentId")] Note note)
        {
            if (ModelState.IsValid)
            {
                db.Entry(note).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignmentId = new SelectList(db.Assignment, "AssignmentId", "Name", note.AssignmentId);
            ViewBag.StudentId = new SelectList(db.Student, "StudentId", "FirstName", note.StudentId);
            return View(note);
        }

        // GET: Notes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Note.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = db.Note.Find(id);
            db.Note.Remove(note);
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
