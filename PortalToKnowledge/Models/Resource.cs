using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.Models
{
	public class Resource
	{
		[Key]
		public int ResourceId { get; set; }
		public string Name { get; set; }
		public string StreetAddress { get; set; }

		[ForeignKey("City")]
		public int CityId { get; set; }
		public City City { get; set; }

		[ForeignKey("State")]
		public int StateId { get; set; }
		public State State { get; set; }

		[ForeignKey("Zipcode")]
		public int ZipcodeId { get; set; }
		public Zipcode Zipcode { get; set; }
	}
}