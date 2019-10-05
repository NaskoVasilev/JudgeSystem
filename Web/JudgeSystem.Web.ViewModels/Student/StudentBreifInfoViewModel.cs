using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.ViewModels.Student
{
    public class StudentBreifInfoViewModel : IMapFrom<Data.Models.Student>
	{
		public int ClassNumber { get; set; }

		public string ClassType { get; set; }

		public int NumberInCalss { get; set; }

		public string FullName { get; set; }

        public string ClassName => $"{ClassNumber} {ClassType}";
	}
}
