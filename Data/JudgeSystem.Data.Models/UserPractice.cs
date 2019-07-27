namespace JudgeSystem.Data.Models
{
    public class UserPractice
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int PracticeId { get; set; }
        public Practice Practice { get; set; }
    }
}
