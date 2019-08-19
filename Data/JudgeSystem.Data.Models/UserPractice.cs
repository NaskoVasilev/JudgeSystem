using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Data.Models
{
    public class UserPractice
    {
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int PracticeId { get; set; }
        public Practice Practice { get; set; }
    }
}
