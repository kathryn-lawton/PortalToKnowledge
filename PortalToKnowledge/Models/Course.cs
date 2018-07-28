using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.Models
{
	public class Course
	{
		public Course()
		{
			this.Students = new HashSet<Student>();
			this.Assignments = new HashSet<Assignment>();
		}

		[Key]
		public int CourseId { get; set; }
		public string Name { get; set; }

		[ForeignKey("Instructor")]
		public int InstructorId { get; set; }
		public Instructor Instructor { get; set; }

		public virtual ICollection<Student> Students { get; set; }
		public virtual ICollection<Assignment> Assignments { get; set; }
	}
}