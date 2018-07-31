using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.Models
{
	public class Student
	{
		public Student()
		{
			this.Courses = new HashSet<Course>();
			this.Progresses = new HashSet<Progress>();
			this.Notes = new HashSet<Note>();
		}

		[Key]
		public int StudentId { get; set; }

		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[ForeignKey("ApplicationUser")]
		public string ApplicationUserId { get; set; }
		public ApplicationUser ApplicationUser { get; set; }

		public virtual ICollection<Course> Courses { get; set; }
		public virtual ICollection<Progress> Progresses { get; set; }
		public virtual ICollection<Note> Notes { get; set; }
	}
}