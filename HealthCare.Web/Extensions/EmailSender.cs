using Microsoft.AspNetCore.Identity.UI.Services;

namespace HealthCare.Web.Extensions
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine($"Email enviado para: {email}");
            Console.WriteLine($"Assunto: {subject}");
            Console.WriteLine($"Mensagem: {htmlMessage}");
            return Task.CompletedTask;
        }
    }
}
