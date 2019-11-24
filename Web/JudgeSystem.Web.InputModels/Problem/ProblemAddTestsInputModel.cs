using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace JudgeSystem.Web.InputModels.Problem
{
    public class ProblemAddTestsInputModel
    {
        public int ProblemId { get; set; }

        public int LessonId { get; set; }

        public string ProblemName { get; set; }

        [Required]
        public IFormFile Tests { get; set; }
    }
}
