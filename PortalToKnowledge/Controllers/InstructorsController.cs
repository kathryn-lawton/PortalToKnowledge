using Microsoft.AspNet.Identity;
using PortalToKnowledge.Models;
using PortalToKnowledge.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
		public ActionResult CreateCourse()
		{
			return View();
		}

		[HttpPost]
		public ActionResult CreateCourse(Course model)
		{
			var currentUserId = User.Identity.GetUserId();
			var foundInstructor = db.Instrutor.Where(i => i.ApplicationUserId == currentUserId).FirstOrDefault();

			if(foundInstructor == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			model.InstructorId = foundInstructor.InstructorId;
			db.Course.Add(model);
			db.SaveChanges();

			return RedirectToAction("ViewCourses");
		}

		[HttpGet]
		public ActionResult ViewCourses()
		{
			var currentUserId = User.Identity.GetUserId();
			var foundInstructor = db.Instrutor.Where(i => i.ApplicationUserId == currentUserId).FirstOrDefault();

			if(foundInstructor == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			return View(foundInstructor.Courses.ToList());
		}

		[HttpGet]
		public ActionResult ViewStudents()
		{
			var currentUserId = User.Identity.GetUserId();
			var foundInstructor = db.Instrutor.Where(i => i.ApplicationUserId == currentUserId).FirstOrDefault();

			var courseStudentProgressMap = new Dictionary<int, Dictionary<int, int>>();
			foreach (var course in foundInstructor.Courses)
			{
				Dictionary<int, int> percentageMap = new Dictionary<int, int>();
				foreach (var student in course.Students)
				{
					int completedAssignments = 0;
					foreach (var assignment in course.Assignments)
					{
						var progress = db.Progress.Where(p => p.AssignmentId == assignment.AssignmentId && p.StudentId == student.StudentId).FirstOrDefault();
						if(progress.Status)
						{
							completedAssignments++;
						}
					}
					int wholePercentage = 0;
					if (course.Assignments.Count > 0)
					{
						wholePercentage = (completedAssignments * 100) / course.Assignments.Count();
					}
					percentageMap.Add(student.StudentId, wholePercentage);
				}
				courseStudentProgressMap.Add(course.CourseId, percentageMap);
			}

			var model = new InstructorCourseViewModel()
			{
				Courses = foundInstructor.Courses.ToList(),
				CourseStudentProgress = courseStudentProgressMap
			};
			return View(model);
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

		// GET: Instructor/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Instructor instructor = db.Instrutor.Find(id);
			if (instructor == null)
			{
				return HttpNotFound();
			}
			return View(instructor);
		}

		// POST: Instructor/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "InstructorId, FirstName, LastName")] Instructor instructor)
		{
			if (ModelState.IsValid)
			{
				db.Entry(instructor).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(instructor);
		}

		// GET: Instructor/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Instructor instructor = db.Instrutor.Find(id);
			if (instructor == null)
			{
				return HttpNotFound();
			}
			return View(instructor);
		}

		// POST: Instructor/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Instructor instructor = db.Instrutor.Find(id);
			db.Instrutor.Remove(instructor);
			db.SaveChanges();
			return RedirectToAction("Index");
		}
	}


}
