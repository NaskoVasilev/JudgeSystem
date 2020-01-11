﻿using System.Collections.Generic;

using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.Tests.TestData
{
    public static class FeedbackTestData
    {
        public static Feedback GetEntity() => new Feedback
        {
            Id = 1,
            Content = "test content"
        };

        public static IEnumerable<Feedback> GenerateFeedbacks()
        {
            string[] contents =
            {
                "Works fine",
                "Keep coding",
                "Keep develop and maintain the project",
                "Keep develop and maintain the project",
                "Keep develop and maintain the project",
                "Keep develop and maintain the project",
                "Keep develop and maintain the project",
                "Keep develop and maintain the project",
            };

            ApplicationUser sender = TestApplicationUser.GetDefaultUser();

            for (int i = 0; i < contents.Length; i++)
            {
                yield return new Feedback
                {
                    Content = contents[i],
                    Id = i + 1,
                    Sender = sender
                };
            }
        }
    }
}
