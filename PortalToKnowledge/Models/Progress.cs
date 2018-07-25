using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.Models
{
	public class Progress
	{
		[Key]
		public int ProgressId { get; set; }

		public bool TaskStatus { get; set; }
	}
}