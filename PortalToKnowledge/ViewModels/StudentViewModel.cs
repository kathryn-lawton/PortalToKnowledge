using PortalToKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.ViewModels
{
	public class StudentViewModel
	{
		public Student Student { get; set; }
		public Dictionary<int, int> progressPercentageMap;
	}
}