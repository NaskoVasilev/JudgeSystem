using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.Dtos.Student
{
    public class StudentDto : IMapFrom<Data.Models.Student>
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string ActivationKeyHash { get; set; }

        public int NumberInCalss { get; set; }

        public bool IsActivated { get; set; } = false;

        public int SchoolClassId { get; set; }
    }
}
