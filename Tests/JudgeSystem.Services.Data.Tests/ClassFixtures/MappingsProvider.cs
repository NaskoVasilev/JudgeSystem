using System.Reflection;

using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Dtos.Lesson;
using JudgeSystem.Web.InputModels.Course;
using JudgeSystem.Web.ViewModels;

namespace JudgeSystem.Services.Data.Tests.ClassFixtures
{
    public class MappingsProvider
    {
        public MappingsProvider()
        {
            //Register all mappings in the app
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly, 
                typeof(CourseInputModel).GetTypeInfo().Assembly, typeof(ContestLessonDto).Assembly);
        }
    }
}
