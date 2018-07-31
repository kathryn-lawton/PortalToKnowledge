using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortalToKnowledge.Models;

namespace PortalToKnowledge.ViewModels
{
	public class AssignmentNoteViewModel
	{
		public Assignment Assignment { get; set; }
		public Note Note { get; set; }
	}
}