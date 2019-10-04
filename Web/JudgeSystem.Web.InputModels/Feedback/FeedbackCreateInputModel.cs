using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.InputModels.Feedback
{
    public class FeedbackCreateInputModel : IMapTo<Data.Models.Feedback>
    {
        [MaxLength(ModelConstants.FeedbackContentMaxLength)]
        [MinLength(ModelConstants.FeedbackContentMinLength)]
        public string Content { get; set; }
    }
}
