using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using JudgeSystem.Services.Mapping;


namespace JudgeSystem.Web.InputModels.Test
{
    public class TestInputModel : IMapTo<Data.Models.Test>
	{
		public int Id { get; set; }
		
		public int ProblemId { get; set; }

		[Required]
		public string InputData { get; set; }

		[Required]
		public string OutputData { get; set; }

		public bool IsTrialTest { get; set; }

        [NotMapped]
        public int LessonId { get; set; }
    }
}
