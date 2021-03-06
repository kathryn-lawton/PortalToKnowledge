﻿using Microsoft.AspNet.Identity;
using PortalToKnowledge.Models;
using PortalToKnowledge.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PortalToKnowledge.Controllers
{
    public class StudentsController : Controller
    {
		ApplicationDbContext db = new ApplicationDbContext();

        // GET: Students
        public ActionResult Index()
        {
			var students = db.Student.ToList();	
			return View(students);
        }

		// GET: Students/Create
		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "StudentId, FirstName, LastName, ApplicationUserId, ProgressId")] Student student)
		{
			if (ModelState.IsValid)
			{
				student.ApplicationUserId = User.Identity.GetUserId();
				db.Student.Add(student);
				db.SaveChanges();
				return RedirectToAction("Details");
			}

			return View(student);
		}

		public async Task<ActionResult> Details(int? id)
		{
			Student student;
			if (id == null)
			{
				var currentUserId = User.Identity.GetUserId();
				if(currentUserId == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				student = db.Student.Where(s => s.ApplicationUserId == currentUserId).FirstOrDefault();
			}
			else
			{
				student = db.Student.Where(s => s.StudentId == id).FirstOrDefault();
			}

			if (student == null)
			{
				return HttpNotFound();
			}

			Dictionary<int, int> percentageMap = new Dictionary<int, int>();
			foreach(var course in student.Courses)
			{
				int completedAssignments = 0;

				foreach (var assignment in course.Assignments)
				{
					var progress = await db.Progress.Where(p => p.StudentId == student.StudentId && p.AssignmentId == assignment.AssignmentId).FirstOrDefaultAsync();
					if(progress.Status)
					{
						completedAssignments++;
					}
				}

				int wholePercentage = 0;
				if(course.Assignments.Count() > 0)
				{
					wholePercentage = (completedAssignments * 100) / course.Assignments.Count();
				}
				percentageMap.Add(course.CourseId, wholePercentage);
			}

			var model = new StudentViewModel()
			{
				Student = student,
				progressPercentageMap = percentageMap
			};


			return View(model);
		}

		[HttpGet]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Student student = db.Student.Find(id);
			if (student == null)
			{
				return HttpNotFound();
			}
			return View(student);
		}
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "StudentId, FirstName, LastName")] Student student)
		{
			if (ModelState.IsValid)
			{
				db.Entry(student).State = System.Data.Entity.EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(student);
		}

		[HttpGet]
		public ActionResult Courses()
		{
			var currentUserId = User.Identity.GetUserId();
			var foundStudent = db.Student.Where(s => s.ApplicationUserId == currentUserId).FirstOrDefault();

			if (foundStudent == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var foundCourses = foundStudent.Courses.ToList();
			return View(foundCourses);
		}

		public ActionResult Notes()
		{
			return View();
		}

		[HttpGet]
		public ActionResult Flashcards()
		{
			QuizletResponse quizletResponse;
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri("https://api.quizlet.com/");
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				string api = $"2.0/sets/205753987?client_id=ZdN3F9y6eV&whitespace=1";
				var response = client.GetAsync(api).Result;
				//pass in string id
				//figure out id, add field to class model

				string responseString;
				if (response.IsSuccessStatusCode)
				{
					responseString = response.Content.ReadAsStringAsync().Result;
					JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
					quizletResponse = javaScriptSerializer.Deserialize<QuizletResponse>(responseString);

					//var values = javaScriptSerializer.DeserializeObject<QuizletResponse>(responseString);
					//string test = values;
					Console.WriteLine(quizletResponse.id);
				}
				else
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
			}

			return View(quizletResponse);
		}

		[HttpGet]
		public ActionResult ViewAssignment(int? id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var currentUserId = User.Identity.GetUserId();
			var foundStudent = db.Student.Where(s => s.ApplicationUserId == currentUserId).FirstOrDefault();

			if (foundStudent == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var assignment = db.Assignment.Where(a => a.AssignmentId == id).Include(a => a.MediaType).FirstOrDefault();
			var note = new Note()
			{
				StudentId = foundStudent.StudentId,
				AssignmentId = assignment.AssignmentId
			};
			var model = new AssignmentNoteViewModel()
			{
				Assignment = assignment,
				Note = note
			};
			return View(model);
		}

		[HttpPost]
		public ActionResult ViewAssignment(AssignmentNoteViewModel model)
		{
			if(model == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			if(model.Note == null)
			{
				var currentUserId = User.Identity.GetUserId();
				var foundStudent = db.Student.Where(s => s.ApplicationUserId == currentUserId).FirstOrDefault();

				if (foundStudent == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}

				var progress = db.Progress.Where(p => p.AssignmentId == model.Assignment.AssignmentId && p.StudentId == foundStudent.StudentId).FirstOrDefault();
				progress.Status = true;
				db.SaveChanges();

				return RedirectToAction("Courses");
			}
			else
			{
				model.Note.Timestamp = DateTime.Now;
				db.Note.Add(model.Note);
				db.SaveChanges();
				var newNote = new Note()
				{
					AssignmentId = model.Note.AssignmentId,
					StudentId = model.Note.StudentId
				};
				model.Note = newNote;

				var assignment = db.Assignment.Where(a => a.AssignmentId == newNote.AssignmentId).Include(a => a.MediaType).FirstOrDefault();
				model.Assignment = assignment;
				model.Note.Content = null;

				return View(model);
			}
		}

	}
}