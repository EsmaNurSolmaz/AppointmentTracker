using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RandevuYonetimSistemi.Services
{
    public class BrevoMailService
    {
        private readonly string _apiKey;
        private readonly string _senderEmail;
        private readonly string _senderName;

        public BrevoMailService()
        {
            _apiKey = ConfigurationManager.AppSettings["BrevoApiKey"];
            _senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
            _senderName = ConfigurationManager.AppSettings["SenderName"];
        }

        public async Task SendVerificationCode(string toEmail, string code)
        {
            using var client = new HttpClient();

            client.DefaultRequestHeaders.Add("api-key", _apiKey);
            client.DefaultRequestHeaders.Add("accept", "application/json");

            var body = new
            {
                sender = new
                {
                    email = _senderEmail,
                    name = _senderName
                },
                to = new[]
                {
                    new { email = toEmail }
                },
                subject = "Verification Code",
                htmlContent = $@"
                    <p>Hello,</p>
                    <p>Your verification code:</p>
                    <h2>{code}</h2>
                    <p>This code is valid for 3 minutes</p>
                "
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                "https://api.brevo.com/v3/smtp/email",
                content
            );

            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception("Brevo Mail Error: " + responseText);
        }
    }
}
