using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.Models
{
	public class Progress
	{
		[Key]
		public int ProgressId { get; set; }

		public bool Status { get; set; }

		[ForeignKey("Student")]
		public int StudentId { get; set; }
		public Student Student { get; set; }

		[ForeignKey("Assignment")]
		public int AssignmentId { get; set; }
		public Assignment Assignment { get; set; }
	}
}