using HealthCare.Business.Interfaces;
using RestSharp;

namespace HealthCare.Data.Email
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey = Environment.GetEnvironmentVariable("SG_API_KEY");
        private readonly string _fromEmail = "contato@jbsoft.io";
        private readonly string _fromName = "JBSoft";

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var client = new RestClient("https://send.api.mailtrap.io/api/send");
            var request = new RestRequest();
            request.AddHeader("Authorization", "Bearer 56f68cd31f256b1e70a4de82ce3888b3");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json",
                $"{{\"from\":{{\"email\":\"hello@demomailtrap.co\",\"name\":\"Mailtrap Test\"}},\"to\":[{{\"email\":\"jbrunomf@outlook.com\"}}],\"subject\":\"{subject}\",\"text\":\"{message}\",\"category\":\"Integration Test\"}}",
                ParameterType.RequestBody);
            var response = client.Post(request);
        }
    }
}