namespace JudgeSystem.Web.InputModels.Resource
{
	using System.ComponentModel.DataAnnotations;

	using Data.Models.Enums;
	using Services.Mapping;
	using Data.Models;

	using Microsoft.AspNetCore.Http;

	public class ResourceEditInputModel : IMapTo<Resource>, IMapFrom<Resource>
	{
		public int Id { get; set; }

		[Required]
		[MinLength(3)]
		public string Name { get; set; }

		public IFormFile File { get; set; }
	}
}
