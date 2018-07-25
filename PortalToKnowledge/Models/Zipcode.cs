using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.Models
{
	public class Zipcode
	{
		[Key]
		public int ZipcodeId { get; set; }
		public string Zip { get; set; }
	}
}