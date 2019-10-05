using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Data.Common.Models;

namespace JudgeSystem.Data.Models
{
    public class Feedback : BaseDeletableModel<int>
    {
        [MaxLength(ModelConstants.FeedbackContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
    }
}
