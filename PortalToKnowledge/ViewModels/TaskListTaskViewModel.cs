using Google.Apis.Tasks.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.ViewModels
{
	public class TaskListTaskViewModel
	{
		public IList<TaskList> TaskLists { get; set; }
		public Dictionary<string, IList<Task>> TasksMap { get; set; }
	}
}