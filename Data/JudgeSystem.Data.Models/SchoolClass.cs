namespace JudgeSystem.Data.Models
{
	using JudgeSystem.Data.Common.Models;
    using JudgeSystem.Data.Models.Enums;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SchoolClass : BaseDeletableModel<int>
	{
		public SchoolClass()
		{
			this.Students = new HashSet<Student>();
		}

		public int ClassNumber { get; set; }

		public SchoolClassType ClassType { get; set; }

		[NotMapped]
		public string Name => $"{ClassNumber} {ClassType}";

		public ICollection<Student> Students { get; set; }
	}
}
