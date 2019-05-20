namespace JudgeSystem.Web.ViewModels.Lesson
{
	using System.ComponentModel.DataAnnotations;
	using System.Collections.Generic;
	using Microsoft.AspNetCore.Http;

	using Data.Models.Enums;
	using Services.Mapping;
	using Data.Models;
	using AutoMapper;
	using JudgeSystem.Common;

	public class LessonInputModel : IMapTo<Lesson>, IHaveCustomMappings
	{
		public LessonInputModel()
		{
			this.Resources = new List<IFormFile>();
		}

		[Required]
		[MinLength(GlobalConstants.NameMinLength)]
		public string Name { get; set; }

		public int CourseId { get; set; }

		[DataType(DataType.Password)]
		public string LessonPassword { get; set; }

		[Required]
		public LessonType Type { get; set; }

		public List<IFormFile> Resources { get; set; }

		public void CreateMappings(IMapperConfigurationExpression configuration)
		{
			configuration.CreateMap<LessonInputModel, Lesson>()
				.ForMember(x => x.Resources, y => y.Ignore());
		}
	}
}
