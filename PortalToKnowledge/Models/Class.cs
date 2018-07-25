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
			this.Medias = new HashSet<Media>();
		}

		[Key]
		public int ClassId { get; set; }
		public string Name { get; set; }

		[ForeignKey("Instructor")]
		public int InstructorId { get; set; }
		public Instructor Instructor { get; set; }

		public virtual ICollection<Student> Students { get; set; }
		public virtual ICollection<Media> Medias { get; set; }
	}
}