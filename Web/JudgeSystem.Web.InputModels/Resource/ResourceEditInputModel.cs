using System.ComponentModel.DataAnnotations;

using JudgeSystem.Services.Mapping;
using JudgeSystem.Common;

using Microsoft.AspNetCore.Http;
using AutoMapper;

namespace JudgeSystem.Web.InputModels.Resource
{
    public class ResourceEditInputModel : IMapTo<Data.Models.Resource>, IMapFrom<Data.Models.Resource>
	{
		public int Id { get; set; }

		[Required]
        [StringLength(ModelConstants.ResourceNameMaxLength, MinimumLength = ModelConstants.ResourceNameMinLength)]
        public string Name { get; set; }

        [IgnoreMap]
		public IFormFile File { get; set; }

        [Range(ModelConstants.OrderByMinValue, ModelConstants.OrderByMaxValue)]
        public int OrderBy { get; set; }
    }
}
