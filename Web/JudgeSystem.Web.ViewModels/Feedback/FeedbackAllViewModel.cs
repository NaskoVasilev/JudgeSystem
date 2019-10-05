using System;

using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.ViewModels.Feedback
{
    public class FeedbackAllViewModel : IMapFrom<Data.Models.Feedback>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string SenderUsername { get; set; }

        public string SenderEmail { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
