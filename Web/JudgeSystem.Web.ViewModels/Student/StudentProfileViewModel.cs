using JudgeSystem.Services.Mapping;

using AutoMapper;

namespace JudgeSystem.Web.ViewModels.Student
{
    public class StudentProfileViewModel : IMapFrom<Data.Models.Student>, IHaveCustomMappings
	{
		public string Id { get; set; }

		public string FullName { get; set; }

		public string Email { get; set; }

        public string UserId { get; set; }

        public int NumberInCalss { get; set; }

        public string SchoolClassName { get; set; }

        public bool IsActivated { get; set; }

        public void CreateMappings(IProfileExpression configuration)
		{
			configuration.CreateMap<Data.Models.Student, StudentProfileViewModel>()
				.ForMember(x => x.SchoolClassName, y => y.MapFrom(s => $"{s.SchoolClass.ClassNumber} {s.SchoolClass.ClassType}"));
		}
	}
}
