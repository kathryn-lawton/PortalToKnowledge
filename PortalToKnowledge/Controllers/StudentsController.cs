using Microsoft.AspNet.Identity;
using PortalToKnowledge.Models;
using PortalToKnowledge.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
				return RedirectToAction("Index");
			}

			return View(student);
		}

		public ActionResult Details(int? id)
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
			return View(student);
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
			if (currentUserId == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var foundStudent = db.Student.Where(s => s.ApplicationUserId == currentUserId).FirstOrDefault();
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
			//if(id == null)
			//{
			//	return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			//}
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


	}
}