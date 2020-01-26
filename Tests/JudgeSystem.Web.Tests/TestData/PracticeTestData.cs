using System;
using System.Collections.Generic;
using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.Tests.TestData
{
    public class PracticeTestData
    {
        public static Practice GetEntity() => new Practice()
        {
            Id = 1
        };

        public static Practice GetPracticeWithUserPractices()
        {
            int practiceId = 11;
            var lesson = new Lesson { Id = 10 };
            var problems = new List<Problem>
            {
                new Problem { Id = 1, Name = "problem1", LessonId = lesson.Id, IsExtraTask = false },
                new Problem { Id = 2, Name = "problem2", LessonId = lesson.Id, IsExtraTask = false },
                new Problem { Id = 3, Name = "problem3", LessonId = lesson.Id, IsExtraTask = true },
            };

            lesson.Problems = problems;

            return new Practice
            {
                Id = practiceId,
                Lesson = lesson,
                UserPractices = new List<UserPractice>
                {
                    new UserPractice
                    {
                        User = new ApplicationUser
                        {
                            Id = "nasko_id",
                            Name = "Atanas",
                            Surname = "Vasilev",
                            UserName = "nasko",
                            Submissions = new List<Submission>
                            {
                                new Submission { PracticeId = practiceId, ProblemId =  problems[0].Id, ActualPoints = 40},
                                new Submission { PracticeId = practiceId, ProblemId =  problems[0].Id, ActualPoints = 50},
                                new Submission { PracticeId = 12, ProblemId =  15, ActualPoints = 100},
                                new Submission { PracticeId = practiceId, ProblemId =  problems[2].Id, ActualPoints = 100},
                                new Submission { PracticeId = practiceId, ProblemId =  problems[2].Id, ActualPoints = 100},
                                new Submission { PracticeId = practiceId, ProblemId =  problems[2].Id, ActualPoints = 20},
                            }
                        },
                        PracticeId = practiceId
                    }
                }
            };
        }
    }
}
