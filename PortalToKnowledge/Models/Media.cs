using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.Models
{
	public class Media
	{
		[Key]
		public int MediaId { get; set; }
		public string Link { get; set; }

		[ForeignKey("Class")]
		public int ClassId { get; set; }
		public Class Class { get; set; }

		[ForeignKey("MediaType")]
		public int MediaTypeId { get; set; }
		public MediaType MediaType { get; set; }
	}
}