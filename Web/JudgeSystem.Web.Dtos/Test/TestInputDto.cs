using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.Dtos.Test
{
    public class TestInputDto : IMapFrom<Data.Models.Test>
    {
        public string InputData { get; set; }
    }
}
