namespace JudgeSystem.Web.ViewModels.Submission
{
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;
	using System.Collections.Generic;
	using JudgeSystem.Web.ViewModels.ExecutedTest;
	using AutoMapper;
	using System.Text;
	using System.Globalization;
	using JudgeSystem.Common;

	public class SubmissionViewModel : IMapFrom<Submission>, IHaveCustomMappings
	{
		public string Code { get; set; }

		public string CompilationErrors { get; set; }

		[IgnoreMap]
		public bool CompiledSucessfully => string.IsNullOrEmpty(CompilationErrors);

		public string ProblemName { get; set; }

		public string UserUsername { get; set; }

		public List<ExecutedTestViewModel> ExecutedTests { get; set; }

		public string SubmissionDate { get; set; }

		public void CreateMappings(IMapperConfigurationExpression configuration)
		{
			configuration.CreateMap<Submission, SubmissionViewModel>()
				.ForMember(svm => svm.Code, y => y.MapFrom(s => s.Code == null ? "" : Encoding.UTF8.GetString(s.Code)))
				.ForMember(svm => svm.CompilationErrors, y => y.MapFrom(s => 
				s.CompilationErrors == null ? "" : Encoding.UTF8.GetString(s.CompilationErrors)))
				.ForMember(svm => svm.SubmissionDate, y => y.MapFrom(s => s.SubmisionDate
				.ToString(GlobalConstants.StandardDateFormat, CultureInfo.InvariantCulture)));
		}
	}
}
