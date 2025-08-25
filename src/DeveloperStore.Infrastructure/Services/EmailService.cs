using DeveloperStore.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DeveloperStore.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // Implementação de envio de email
            // Aqui você pode integrar com SendGrid, SMTP, etc.
            Console.WriteLine($"Enviando email para: {to}");
            Console.WriteLine($"Assunto: {subject}");
            Console.WriteLine($"Corpo: {body}");

            await Task.CompletedTask;
        }

        public async Task SendPasswordResetEmailAsync(string to, string resetToken)
        {
            var resetUrl = $"{_configuration["App:BaseUrl"]}/reset-password?token={resetToken}";
            var body = $@"
                <h1>Redefinição de Senha</h1>
                <p>Clique no link abaixo para redefinir sua senha:</p>
                <a href='{resetUrl}'>{resetUrl}</a>
                <p>Este link expira em 1 hora.</p>
            ";

            await SendEmailAsync(to, "Redefinição de Senha - DeveloperStore", body);
        }
    }
}