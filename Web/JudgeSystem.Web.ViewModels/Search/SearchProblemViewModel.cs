using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.ViewModels.Search
{
    public class SearchProblemViewModel : IMapFrom<Data.Models.Problem>
    {
        public string Name { get; set; }

        public string LessonName { get; set; }

        public int LessonId { get; set; }

        public int LessonPracticeId { get; set; }
    }
}
