using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.Models
{
	public class Class
	{
		public Class()
		{
			this.Students = new HashSet<Student>();
			this.ClassTasks = new HashSet<ClassTask>();
		}

		[Key]
		public int ClassId { get; set; }
		public string Name { get; set; }

		[ForeignKey("Instructor")]
		public int InstructorId { get; set; }
		public Instructor Instructor { get; set; }

		public virtual ICollection<Student> Students { get; set; }
		public virtual ICollection<ClassTask> ClassTasks { get; set; }
	}
}