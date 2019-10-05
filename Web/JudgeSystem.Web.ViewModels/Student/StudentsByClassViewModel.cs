using System.Collections.Generic;

using JudgeSystem.Web.Infrastructure.Pagination;

namespace JudgeSystem.Web.ViewModels.Student
{
    public class StudentsByClassViewModel
    {
        public IEnumerable<StudentProfileViewModel> Students { get; set; }

        public PaginationData PaginationData { get; set; }
    }
}
