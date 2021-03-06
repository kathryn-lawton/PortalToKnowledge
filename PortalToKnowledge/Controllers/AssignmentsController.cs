﻿using System;
using System.Collections;
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
    public class AssignmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Assignments
        public ActionResult Index()
        {
			if (User.IsInRole("Student"))
			{
				var currentUserId = User.Identity.GetUserId();
				var foundUser = db.Student.Where(s => s.ApplicationUserId == currentUserId).FirstOrDefault();
				var foundClasses = foundUser.Courses.ToList();
				return View(foundClasses);
			}
			else if (User.IsInRole("Instructor"))
			{
				var currentUserId = User.Identity.GetUserId();
				var foundUser = db.Instrutor.Where(i => i.ApplicationUserId == currentUserId).FirstOrDefault();
				var foundClasses = foundUser.Courses.ToList();

				return View(foundClasses);
			}

			return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

		// GET: Assignments/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignment.Where(a => a.AssignmentId == id).Include(a => a.Course).Include(a => a.MediaType).FirstOrDefault();
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // GET: Assignments/Create
        public ActionResult Create(int? id)
        {
			if(id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var assignment = new Assignment()
			{
				CourseId = id.Value
			};

			ViewBag.MediaTypeId = new SelectList(db.MediaType, "Id", "Type", assignment.MediaTypeId);

			return View(assignment);
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssignmentId,Name,DueDate,Link,CourseId,MediaTypeId")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                db.Assignment.Add(assignment);
				db.SaveChanges();
				var students = db.Course.Where(c => c.CourseId == assignment.CourseId).FirstOrDefault().Students.ToList();
				foreach(var student in students)
				{
					Progress progress = new Progress()
					{
						StudentId = student.StudentId,
						AssignmentId = assignment.AssignmentId,
						Status = false
					};
					db.Progress.Add(progress);
				}
				
				// Create and Add progress record for each student for this assignment
				db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(assignment);
        }

        // GET: Assignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignment.Where(a => a.AssignmentId == id).Include(a => a.MediaType).Include(a => a.Course).FirstOrDefault();
            if (assignment == null)
            {
                return HttpNotFound();
            }

            ViewBag.CourseId = new SelectList(db.Course, "CourseId", "Name", assignment.CourseId);
			ViewBag.MediaTypeId = new SelectList(db.MediaType, "Id", "Type", assignment.MediaTypeId);
			return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssignmentId,Name,Link,MediaTypeId,DueDate,CourseId")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Course, "CourseId", "Name", assignment.CourseId);
			ViewBag.MediaTypeId = new SelectList(db.MediaType, "MediaTypeId", "Type", assignment.MediaTypeId);
			return View(assignment);
        }

        // GET: Assignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignment.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Assignment assignment = db.Assignment.Find(id);
            db.Assignment.Remove(assignment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

		public ActionResult CompleteProgress(int? id)
		{

			if(id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var progressRecord = db.Progress.Where(p => p.ProgressId == id).FirstOrDefault();
			progressRecord.Status = true;
			db.SaveChanges();
			return RedirectToAction("Index", "Students");
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
