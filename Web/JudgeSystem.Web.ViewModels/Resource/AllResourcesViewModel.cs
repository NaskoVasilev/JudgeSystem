using System.Collections.Generic;

namespace JudgeSystem.Web.ViewModels.Resource
{
    public class AllResourcesViewModel
    {
        public IEnumerable<ResourceViewModel> Resources { get; set; }

        public int LessonId { get; set; }

        public int PracticeId { get; set; }
    }
}
