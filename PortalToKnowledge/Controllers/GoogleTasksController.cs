using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using Google.Apis.Util.Store;
using PortalToKnowledge.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace PortalToKnowledge.Controllers
{
    public class GoogleTasksController : Controller
    {
		static string[] Scopes = { TasksService.Scope.Tasks };
		static string ApplicationName = "Portal To Knowledge";

		// GET: GoogleTasks
		public ActionResult Index()
        {
            return View();
        }

		public ActionResult GoogleTasks()

		{ 
			UserCredential credential;

			using (var stream =
				new FileStream("C:\\Users\\klawt\\source\\repos\\GoogleTasksTest\\GoogleTasksTest\\bin\\Debug\\Properties\\client_id.json", FileMode.Open, FileAccess.Read))
			{
				string credPath = "C:\\Users\\klawt\\source\\repos\\GoogleTasksTest\\GoogleTasksTest\\bin\\Debug\\token.json";
				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
								Scopes,
								"user",
								CancellationToken.None,
								new FileDataStore(credPath, true)).Result;
				Console.WriteLine("Credential file saved to: " + credPath);
			}

			// Create Google Tasks API service.
			var service = new TasksService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = ApplicationName,
			});

			// Define parameters of request.
			TasklistsResource.ListRequest listRequest = service.Tasklists.List();
			listRequest.MaxResults = 10;

			// List task lists.
			
			IList<TaskList> taskLists = listRequest.Execute().Items;

			Dictionary<string, IList<Task>> taskMap = new Dictionary<string, IList<Task>>();

			foreach (var taskList in taskLists)
			{
				var request = service.Tasks.List(taskList.Id);
				var tasksList = request.Execute().Items;
				taskMap.Add(taskList.Id, tasksList);
			}

			TaskListTaskViewModel model = new TaskListTaskViewModel()
			{
				TaskLists = taskLists,
				TasksMap = taskMap
			};

			return View(model);
		}

		//public ActionResult AddTask()
		//{
		//	if(User.IsInRole("Instructor"))
		//	{
		//		Task task = new Task { Title = "New Task" };
		//		task.Notes = "Please complete me";
		//		task.Due = "2018-08-15T12:00:00.000Z";

		//		Task result = service.Tasks.Insert(task, "@default").Fetch();
		//		Console.WriteLine(result.Title);
		//	}
		//}
	}
}