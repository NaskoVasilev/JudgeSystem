using System.Collections.Generic;
using System.Linq;

using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.Tests.TestData
{
    public class UserContestTestData
    {
        public static UserContest GetEntity() => new UserContest
        {
            Contest = ContestTestData.GetEntity(),
            UserId = TestApplicationUser.Id
        };

        public static IEnumerable<UserContest> GetEntities()
        {
            Problem problem = ProblemTestData.GetEntity();
            Contest contest = ContestTestData.GetEntity();
            contest.Lesson.Practice = null;
            problem.Lesson = contest.Lesson;
            contest.Lesson.Problems.Add(problem);
            var submissions = SubmissionTestData.GenerateSubmissions().ToList();

            foreach (Submission submission in submissions.Skip(submissions.Count / 2).ToList())
            {
                submission.ContestId = contest.Id;
                contest.Submissions.Add(submission);
            }

            return new List<UserContest>
            {
               new UserContest
               {
                   Contest = contest
               }
            };
        }

        public static IEnumerable<UserContest> GenerateUserContests()
        {
            Contest contest = ContestTestData.GetEntity();

            for (int i = 0; i < 70; i++)
            {
                string studentId = null;
                if (i % 2 == 0)
                {
                    studentId = $"student_id_{i}";
                }

                yield return new UserContest
                {
                    ContestId = contest.Id,
                    User = new ApplicationUser
                    {
                        Id = $"user_id_{i}",
                        StudentId = studentId
                    }
                };
            }
        }
    }
}
