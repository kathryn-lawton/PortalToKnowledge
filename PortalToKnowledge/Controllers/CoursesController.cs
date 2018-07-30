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
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        public ActionResult Index(int? id)
        {
			if(id == null)
			{
				return View(db.Course.ToList());
			}
			else
			{
				// Get the current user Id
				// Look up in their role table
				// Get all the classes related to their account - ToList
				// return to view model

				return View(db.Course.ToList());
			}
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course model = db.Course.Where(c => c.CourseId == id).Include(c => c.Instructor).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course model = db.Course.Where(c => c.CourseId == id).Include(c => c.Instructor).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseId,Name,InstructorId")] Course model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course model = db.Course.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course model = db.Course.Find(id);
            db.Course.Remove(model);
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

		[HttpGet]
		public ActionResult RemoveStudent(int? studentId, int? courseId)
		{
			if(studentId == null || courseId == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var model = new StudentCourseViewModel()
			{
				CourseId = courseId.Value,
				StudentId = studentId.Value
			};

			return View(model);
		}

		[HttpPost]
		public ActionResult RemoveStudent(StudentCourseViewModel model)
		{
			var course = db.Course.Where(c => c.CourseId == model.CourseId).FirstOrDefault();
			var student = db.Student.Where(s => s.StudentId == model.StudentId).FirstOrDefault();
			course.Students.Remove(student);
			db.SaveChanges();
			return RedirectToAction("ViewStudents", "Instructors", null); 
		}

		[HttpGet]
		public ActionResult AddStudent(int? id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var model = new StudentCourseViewModel()
			{
				CourseId = id.Value
			};

			var students = db.Student.Where(s => s.Courses.Where(c => c.CourseId == id).Count() == 0).ToList();

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
		public ActionResult AddStudent([Bind(Include = "CourseId,StudentId")] StudentCourseViewModel model)
		{
			if(model == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var foundCourse = db.Course.Where(c => c.CourseId == model.CourseId).FirstOrDefault();
			var foundStudent = db.Student.Where(s => s.StudentId == model.StudentId).FirstOrDefault();

			if(foundCourse == null || foundStudent == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			foundCourse.Students.Add(foundStudent);
			db.SaveChanges();
			var assignments = foundCourse.Assignments.ToList();
			foreach (var assignment in assignments)
			{
				Progress progress = new Progress()
				{
					StudentId = foundStudent.StudentId,
					AssignmentId = assignment.AssignmentId,
					Status = false
				};
				db.Progress.Add(progress);
			}
			db.SaveChanges();

			return RedirectToAction("Details", new { id = model.CourseId });
		}
    }
}
