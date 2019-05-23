namespace JudgeSystem.Web.ViewModels.Resource
{
	using Data.Models.Enums;
	using Microsoft.AspNetCore.Http;
	using System.ComponentModel.DataAnnotations;

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
