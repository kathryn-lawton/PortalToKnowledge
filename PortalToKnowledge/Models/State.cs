﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.Models
{
	public class State
	{
		[Key]
		public int StateId { get; set; }
		[Display (Name = "State")]
		public string Abbreviation { get; set; }
	}
}