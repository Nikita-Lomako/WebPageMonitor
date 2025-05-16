using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebPageMonitor.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _settings;

        public EmailService(IConfiguration settings)
        {
            _settings = settings;
        }

        public string SendMsg(string reply)
        {
            string result = string.Empty;
            try
            {
                using (MailMessage TheMailMessage = new MailMessage())
                {
                    TheMailMessage.From = new MailAddress(_settings["EmailSender"]!);
                    TheMailMessage.To.Add(_settings["Email"]!);
                    TheMailMessage.Subject = "Сообщение из WebPageMonitor";
                    TheMailMessage.Body = reply;
                    TheMailMessage.IsBodyHtml = true;

                    using (SmtpClient Smtp = new SmtpClient(_settings["SmtpServer"]))
                    {
                        Smtp.Port = 587;
                        Smtp.EnableSsl = true;
                        Smtp.UseDefaultCredentials = false;
                        Smtp.Credentials = new NetworkCredential(
                            _settings["userMail"],
                            _settings["pwdMail"]
                        );

                        Smtp.Timeout = 10000;
                        Smtp.Send(TheMailMessage);
                    }
                }
            }
            catch (SmtpException ex)
            {
                result = $"SMTP Error: {ex.StatusCode} | {ex.Message}";
            }
            catch (Exception ex)
            {
                result = $"General Error: {ex.GetType().Name} | {ex.Message}";
            }
            return result;
        }
    }
}
