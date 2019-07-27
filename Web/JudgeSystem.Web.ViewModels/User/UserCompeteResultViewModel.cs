using System;
using System.Collections.Generic;
using System.Text;

namespace JudgeSystem.Web.ViewModels.User
{
	public class UserCompeteResultViewModel
	{
		public string ContestName { get; set; }

		public int LessonId { get; set; }

		public int MaxPoints { get; set; }

		public int ActualPoints { get; set; }

        public int ContestId { get; set; }
    }
}
