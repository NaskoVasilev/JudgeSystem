using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.Dtos.Resource
{
    public class ResourceDto : IMapFrom<Data.Models.Resource>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FilePath { get; set; }

        public int LessonId { get; set; }
    }
}
