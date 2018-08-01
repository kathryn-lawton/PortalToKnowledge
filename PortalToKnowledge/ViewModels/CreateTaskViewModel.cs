using Google.Apis.Tasks.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.ViewModels
{
	public class CreateTaskViewModel
	{
		public string TaskListId { get; set; }
		public Task Task { get; set; }
	
	}
}