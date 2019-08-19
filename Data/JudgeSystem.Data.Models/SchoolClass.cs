using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using JudgeSystem.Data.Common.Models;
using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Data.Models
{
    public class SchoolClass : BaseDeletableModel<int>
	{
		public SchoolClass()
		{
			Students = new HashSet<Student>();
		}

		public int ClassNumber { get; set; }

		public SchoolClassType ClassType { get; set; }

		[NotMapped]
		public string Name => $"{ClassNumber} {ClassType}";

		public ICollection<Student> Students { get; set; }
	}
}
