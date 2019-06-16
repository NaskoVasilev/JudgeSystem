namespace JudgeSystem.Web.InputModels.Resource
{
	using System.ComponentModel.DataAnnotations;

	using Data.Models.Enums;

	using Microsoft.AspNetCore.Http;

	public class ResourceInputModel
	{
		public int Id { get; set; }

		[Required]
		[MinLength(3)]
		public string Name { get; set; }

		public ResourceType ResourceType { get; set; }

		public int LessonId { get; set; }

		[Required]
		public IFormFile File { get; set; }
	}
}
