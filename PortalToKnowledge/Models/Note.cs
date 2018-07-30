using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.Models
{
	public class Note
	{
		[Key]
		public int NoteId { get; set; }
		public DateTime Timestamp { get; set; }
		public string Content { get; set; }

		[ForeignKey("Student")]
		public int StudentId { get; set; }
		public Student Student { get; set; }

		[ForeignKey("Assignment")]
		public int AssignmentId {get; set;}
		public Assignment Assignment { get; set; }

	}
}