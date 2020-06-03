using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


//hs8-w5L-MSg-BPs
//Key ID: a2b91229-c9912672 
namespace Epam.AspNet.Module1.Controllers
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string title, string content)
        {
            MailAddress from = new MailAddress("noreply@epam.aspneteducation.com");
            MailAddress to = new MailAddress(email);

            MailMessage message = new MailMessage(from, to) { Subject = title, Body = content };

            using (var client = new SmtpClient())
            {
                client.Host = "smtp.mailgun.org";
                client.Port = 587;
                client.Credentials = new NetworkCredential("postmaster@sandboxa080315de35f40f1b067486fba858c0a.mailgun.org", "99bcbe1594b6b750da8c091baf91c5d3-a2b91229-760e2578");
                client.EnableSsl = true;
                await client.SendMailAsync(message);
            }
        }
    }
}