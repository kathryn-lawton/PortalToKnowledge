using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.ViewModels
{
	public class QuizletResponse
	{
		public string id { get; set; }
		public List<TermItem> terms { get; set; }
		public int term_count { get; set; }
		public string title { get; set; }
		public string url { get; set; }
	}

	public class TermItem
	{
		public string id { get; set; }
		public string term { get; set; }
		public string definition { get; set; }
		public int rank { get; set; }
	}
}