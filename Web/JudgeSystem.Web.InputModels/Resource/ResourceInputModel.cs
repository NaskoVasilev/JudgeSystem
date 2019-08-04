using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;

using Microsoft.AspNetCore.Http;

namespace JudgeSystem.Web.InputModels.Resource
{
    public class ResourceInputModel
	{
		public int Id { get; set; }

		[Required]
        [StringLength(ModelConstants.ResourceNameMaxLength, MinimumLength = ModelConstants.ResourceNameMinLength)]
        public string Name { get; set; }

		public int LessonId { get; set; }

        public int PracticeId { get; set; }

        [Required]
		public IFormFile File { get; set; }
	}
}
