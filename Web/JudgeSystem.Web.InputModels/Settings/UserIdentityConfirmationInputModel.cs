using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Web.InputModels.Settings
{
    public class UserIdentityConfirmationInputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
