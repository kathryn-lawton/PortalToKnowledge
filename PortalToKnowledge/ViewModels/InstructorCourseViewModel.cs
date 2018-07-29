using PortalToKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalToKnowledge.ViewModels
{
	public class InstructorCourseViewModel
	{
		public IEnumerable<Course> Courses { get; set; }
		public Dictionary<int, Dictionary<int, int>> CourseStudentProgress { get; set; }
	}
}