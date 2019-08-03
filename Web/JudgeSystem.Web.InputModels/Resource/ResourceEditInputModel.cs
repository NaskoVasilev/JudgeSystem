using System.ComponentModel.DataAnnotations;

using JudgeSystem.Services.Mapping;

using Microsoft.AspNetCore.Http;
using AutoMapper;

namespace JudgeSystem.Web.InputModels.Resource
{
    public class ResourceEditInputModel : IMapTo<Data.Models.Resource>, IMapFrom<Data.Models.Resource>
	{
		public int Id { get; set; }

		[Required]
		[MinLength(3)]
		public string Name { get; set; }

        [IgnoreMap]
		public IFormFile File { get; set; }
	}
}
