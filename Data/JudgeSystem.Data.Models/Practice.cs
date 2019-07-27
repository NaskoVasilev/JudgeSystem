using JudgeSystem.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JudgeSystem.Data.Models
{
    public class Practice : BaseDeletableModel<int>
    {
        public Practice()
        {
            this.Submissions = new List<Submission>();
            this.UserPractices = new List<UserPractice>();
        }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public ICollection<Submission> Submissions { get; set; }

        public ICollection<UserPractice> UserPractices { get; set; }
    }
}
