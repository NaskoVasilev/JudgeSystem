using System.Collections.Generic;
using System.Linq;

using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.Tests.TestData
{
    public class UserPracticeTestData
    {
        public static IEnumerable<UserPractice> GetEntities()
        {
            Problem problem = ProblemTestData.GetEntity();
            problem.Id = 10;
            Practice practice = PracticeTestData.GetEntity();
            Lesson lesson = LessonTestData.GetEntity();
            lesson.Id = 10;
            lesson.Practice = practice;
            practice.Lesson = lesson;
            practice.LessonId = lesson.Id;
            problem.LessonId = lesson.Id;
            problem.Lesson = lesson;
            lesson.Problems.Add(problem);
            var submissions = SubmissionTestData.GenerateSubmissions().ToList();

            foreach (Submission submission in submissions.Take(submissions.Count / 2).ToList())
            {
                submission.PracticeId = practice.Id;
                submission.ContestId = null;
                submission.ProblemId = problem.Id;
                practice.Submissions.Add(submission);
            }

            return new List<UserPractice>
            {
               new UserPractice
               {
                   Practice = practice
               }
            };
        }
    }
}
