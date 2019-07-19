namespace JudgeSystem.Web.ViewModels.Search
{
    using JudgeSystem.Services.Mapping;
    using JudgeSystem.Data.Models;

    public class SearchProblemViewModel : IMapFrom<Problem>
    {
        public string Name { get; set; }

        public string LessonName { get; set; }

        public int LessonId { get; set; }
    }
}
