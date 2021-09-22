using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Data.Common.Models;

namespace JudgeSystem.Data.Models
{
    public class Resource : BaseModel<int>, IOrderable
	{
		[Required]
		[MaxLength(ModelConstants.ResourceNameMaxLength)]
		public string Name { get; set; }

        public int OrderBy { get; set; }

        [Required]
		public string FilePath { get; set; }

		public int LessonId { get; set; }
		public Lesson Lesson { get; set; }
	}
}
