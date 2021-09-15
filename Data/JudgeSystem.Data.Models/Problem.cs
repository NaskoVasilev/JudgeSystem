﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Data.Common.Models;
using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Data.Models
{
    public class Problem : BaseDeletableModel<int>, IOrderable
    {
		public Problem()
		{
			Tests = new HashSet<Test>();
			Submissions = new HashSet<Submission>();
		}

		[Required]
		[MaxLength(ModelConstants.ProblemNameMaxLength)]
		public string Name { get; set; }

        public int OrderBy { get; set; }

        public bool IsExtraTask { get; set; }

		public int MaxPoints { get; set; }

        public int AllowedTimeInMilliseconds { get; set; }

        public double AllowedMemoryInMegaBytes { get; set; }

        public int TimeIntervalBetweenSubmissionInSeconds { get; set; }

        public int AllowedMinCodeDifferenceInPercentage { get; set; }

        public int LessonId { get; set; }
		public Lesson Lesson { get; set; }

        public SubmissionType SubmissionType { get; set; }

        public TestingStrategy TestingStrategy { get; set; }

        public byte[] AutomatedTestingProject { get; set; }

        public ICollection<Test> Tests { get; set; }

		public ICollection<Submission> Submissions { get; set; }
	}
}
