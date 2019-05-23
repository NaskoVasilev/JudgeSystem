namespace JudgeSystem.Web.ViewModels.Resource
{
	using Data.Models.Enums;
	using Services.Mapping;
	using Data.Models;

	using Microsoft.AspNetCore.Http;
	using System.ComponentModel.DataAnnotations;

	public class ResourceEditInputModel : IMapTo<Resource>, IMapFrom<Resource>
	{
		public int Id { get; set; }

		[Required]
		[MinLength(3)]
		public string Name { get; set; }

		public ResourceType ResourceType { get; set; }

		public IFormFile File { get; set; }
	}
}
