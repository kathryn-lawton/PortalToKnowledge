using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.Models
{
	public class Assignment
	{
		[Key]
		public int AssignmentId { get; set; }

		public string Name { get; set; }
		
		[Display(Name = "Due Date")]
		public string DueDate { get; set; }

		public string Link { get; set; }

		[ForeignKey("MediaType")]
		public int MediaTypeId { get; set; }
		public MediaType MediaType { get; set; }

		[ForeignKey("Course")]
		public int CourseId { get; set; }
		public Course Course { get; set; }
	}
}