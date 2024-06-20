using System.Net;
using System.Net.Mail;

namespace CarWorkshopSystem.WebAPI.Utility
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var email = new MailMessage();
            email.From = new MailAddress(_configuration["SmtpSettings:SenderEmail"], _configuration["SmtpSettings:SenderName"]);

            email.Subject = subject;
            email.IsBodyHtml = true;
            email.Body = message;
            email.To.Add(toEmail);

            using var smtp = new SmtpClient();
            smtp.Host = _configuration["SmtpSettings:Host"];
            smtp.Port = 2525;
            smtp.Credentials = new NetworkCredential(_configuration["SmtpSettings:UserName"], _configuration["SmtpSettings:Password"]);
            smtp.EnableSsl = true;

            try
            {
                await smtp.SendMailAsync(email);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}