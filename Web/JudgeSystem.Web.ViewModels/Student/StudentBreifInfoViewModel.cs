namespace JudgeSystem.Web.ViewModels.Student
{
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;
	using AutoMapper;

	public class StudentBreifInfoViewModel : IMapFrom<Student>
	{
		public string SchoolClassName { get; set; }

		public int NumberInCalss { get; set; }

		public string FullName { get; set; }
	}
}
