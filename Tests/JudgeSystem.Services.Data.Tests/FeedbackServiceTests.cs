using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.InputModels.Feedback;
using JudgeSystem.Web.ViewModels.Feedback;

using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class FeedbackServiceTests : TransientDbContextProvider
    {
        private const string Username = "dev_123";
        private const string Email = "dev@dev.bg";

        [Fact]
        public async Task Create_WithValidData_ShouldAddFeedbackToTheDbandSetSenderId()
        {
            IDeletableEntityRepository<Feedback> repository = new EfDeletableEntityRepository<Feedback>(context);
            var service = new FeedbackService(repository);
            string senderId = "dev_id"; ;
            var feedback = new FeedbackCreateInputModel() { Content = "test content" };

            await service.Create(feedback, senderId);

            Assert.True(context.Feedbacks.Any(x => x.Content == feedback.Content && x.SenderId == senderId));
        }

        [Fact]
        public async Task All_WithExistingFeedbacks_ShouldReturnFeedbacks()
        {
            List<Feedback> testData = GetTestData();
            await AddTestData(testData);
            var service = new FeedbackService(new EfDeletableEntityRepository<Feedback>(context));

            var feedbacks = service.All(2, 2).ToList();

            Assert.Equal(2, feedbacks.Count);
            foreach (FeedbackAllViewModel feedback in feedbacks)
            {
                Assert.Equal(Username, feedback.SenderUsername);
                Assert.Equal(Email, feedback.SenderEmail);
            }

            Assert.Equal("content3", feedbacks[0].Content);
            Assert.Equal("content4", feedbacks[1].Content);
        }

        private async Task AddTestData(IEnumerable<Feedback> testData)
        {
            await context.AddRangeAsync(testData);
            await context.SaveChangesAsync();
        }

        private List<Feedback> GetTestData()
        {
            var user = new ApplicationUser
            {
                UserName = Username,
                Id = "dev_123",
                Email = Email
            };

            var feedbacks = new List<Feedback>()
            {
                new Feedback { Content = "content1", Id = 1, SenderId = Username, Sender = user },
                new Feedback { Content = "content2", Id = 2, SenderId = Username, Sender = user },
                new Feedback { Content = "content3", Id = 3, SenderId = Username, Sender = user },
                new Feedback { Content = "content4", Id = 4, SenderId = Username, Sender = user },
                new Feedback { Content = "content5", Id = 5, SenderId = Username, Sender = user },
                new Feedback { Content = "content6", Id = 6, SenderId = Username, Sender = user }
            };
            return feedbacks;
        }
    }
}
