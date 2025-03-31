using HealthCare.Business.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HealthCare.Data.Email
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey = Environment.GetEnvironmentVariable("SG_API_KEY");
        private readonly string _fromEmail = "contato@jbsoft.io";
        private readonly string _fromName = "JBSoft";

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            var response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to send email: {response.StatusCode}");
            }
        }
    }
}