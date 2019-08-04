using System.ComponentModel.DataAnnotations;

using JudgeSystem.Services.Mapping;
using JudgeSystem.Common;

namespace JudgeSystem.Web.InputModels.Student
{
    public class StudentCreateInputModel : IMapTo<Data.Models.Student>
	{
		[Required]
		[MinLength(ModelConstants.StudentFullNameMinLength), MaxLength(ModelConstants.StudentFullNameMaxLength)]
        [Display(Name = ModelConstants.StudentFullNameDisplayName)]
        public string FullName { get; set; }

		[Required]
		[EmailAddress]
        [MaxLength(ModelConstants.StudentEmailMaxLength)]
		public string Email { get; set; }

		[Range(GlobalConstants.MinStudentsInClass, GlobalConstants.MaxStudentsInClass)]
        [Display(Name = ModelConstants.StudentNumberInClassDisplayName)]
        public int NumberInCalss { get; set; }

        [Display(Name = ModelConstants.StudentSchoolClassIdDisplayName)]
        public int SchoolClassId { get; set; }
	}
}
