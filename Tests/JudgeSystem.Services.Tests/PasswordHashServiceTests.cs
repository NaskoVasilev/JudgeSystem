using Xunit;

namespace JudgeSystem.Services.Tests
{
    public class PasswordHashServiceTests
    {
        private readonly PasswordHashService passwordHashService;

        public PasswordHashServiceTests()
        {
            this.passwordHashService = new PasswordHashService();
        }

        [Fact]
        public void HashPassword_AfterHashing_OriginalValuesAndHashedValueMustBeDifferent()
        {
            string value = "password";

            string hash = passwordHashService.HashPassword(value);

            Assert.True(hash != value);
        }

        [Fact]
        public void HashPassword_HashSameValueTwice_ShouldProduceEqualHashes()
        {
            string value = "password";

            string firstHash = passwordHashService.HashPassword(value);
            string secondHash = passwordHashService.HashPassword(value);

            Assert.Equal(firstHash, secondHash);
        }
    }
}
