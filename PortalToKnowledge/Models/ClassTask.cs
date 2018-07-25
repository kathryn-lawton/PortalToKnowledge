using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.Models
{
	public class ClassTask
	{
		[Key]
		public int ClassTaskId { get; set; }

		[Display (Name = "Task Name")]
		public string TaskName { get; set; }

		[ForeignKey("Class")]
		public int ClassId { get; set; }
		public Class Class { get; set; }
	}
}