using System.Text;
using System.Collections.Generic;
using System.Globalization;

using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.ViewModels.ExecutedTest;
using JudgeSystem.Common;
using JudgeSystem.Data.Models.Enums;

using AutoMapper;

namespace JudgeSystem.Web.ViewModels.Submission
{
    public class SubmissionViewModel : IMapFrom<Data.Models.Submission>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string CompilationErrors { get; set; }

        [IgnoreMap]
        public bool CompiledSucessfully => string.IsNullOrEmpty(CompilationErrors);

        public string ProblemName { get; set; }

        public string UserUsername { get; set; }

        public SubmissionType ProblemSubmissionType { get; set; }

        public List<ExecutedTestViewModel> ExecutedTests { get; set; }

        public string SubmissionDate { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Data.Models.Submission, SubmissionViewModel>()
                .ForMember(svm => svm.Code, y => y.MapFrom(s => s.Problem.SubmissionType == SubmissionType.ZipFile
                 || s.Code == null ? "" : Encoding.UTF8.GetString(s.Code)))
				.ForMember(svm => svm.CompilationErrors, y => y.MapFrom(s => 
				s.CompilationErrors == null ? "" : Encoding.UTF8.GetString(s.CompilationErrors)))
				.ForMember(svm => svm.SubmissionDate, y => y.MapFrom(s => s.SubmisionDate
				.ToString(GlobalConstants.StandardDateFormat, CultureInfo.InvariantCulture)));
		}
	}
}
