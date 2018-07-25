using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.Models
{
	public class City
	{
		[Key]
		public int CityId { get; set; }
		[Display(Name = "City")]
		public string Name { get; set; }
	}
}