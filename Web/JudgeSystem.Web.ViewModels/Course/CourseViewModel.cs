using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.ViewModels.Course
{
    public class CourseViewModel : IMapFrom<Data.Models.Course>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
