using DeveloperStore.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DeveloperStore.Application.Tests.Services
{
    public class EmailServiceTests
    {
        private readonly EmailService _emailService;

        public EmailServiceTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["App:BaseUrl"] = "https://localhost:5001"
                })
                .Build();

            _emailService = new EmailService(configuration);
        }

        [Fact]
        public async Task SendEmailAsync_ShouldCompleteWithoutErrors()
        {
            // Arrange
            var to = "test@email.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act & Assert (should not throw)
            await _emailService.SendEmailAsync(to, subject, body);
        }

        [Fact]
        public async Task SendPasswordResetEmailAsync_ShouldCompleteWithoutErrors()
        {
            // Arrange
            var to = "test@email.com";
            var resetToken = "reset-token-123";

            // Act & Assert (should not throw)
            await _emailService.SendPasswordResetEmailAsync(to, resetToken);
        }
    }
}