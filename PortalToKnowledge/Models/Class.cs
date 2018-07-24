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
			this.Instructors = new HashSet<Instructor>();
		}

		[Key]
		public int ClassId { get; set; }
		public string Name { get; set; }

		public virtual ICollection<Instructor> Instructors { get; set; }

		public virtual ICollection<Media> Medias { get; set; }
	}
}