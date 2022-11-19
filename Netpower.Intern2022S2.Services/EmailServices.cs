using Microsoft.Extensions.Options;
using Netpower.Intern2022S2.Entities.Models;
using Netpower.Intern2022S2.Services.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Netpower.Intern2022S2.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly string templatePath = string.Format("{0}\\Netpower.Intern2022S2.Services\\EmailTemplate\\EmailTemplate.html", Directory.GetParent(Directory.GetCurrentDirectory()));
        //C:\Users\phuc.mai\Desktop\isp-2022-be-dotnetcore\Angular_BE\Netpower.Intern2022S2.API\Netpower.Intern2022S2.Services\EmailTemplate\EmailTemplate.html
        private readonly SMTPConfigModel _smtpConfigModel;
        private readonly VerifyTokenConfigModel _verifyTokenConfig;

        public EmailServices(IOptions<SMTPConfigModel> smtpConfigModel, IOptions<VerifyTokenConfigModel> verifyTokenConfig)
        {
            _smtpConfigModel = smtpConfigModel.Value;
            _verifyTokenConfig = verifyTokenConfig.Value;
        }
        private MailMessage getMailMessage(EmailTemplate email)
        {
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = _smtpConfigModel.IsBodyHTML;
            mail.Body = GetEmailBody(email.Body!);
            mail.Subject = email.Subject;
            mail.From = new MailAddress(_smtpConfigModel.SenderAddress!, _smtpConfigModel.SenderDisplayName);
            mail.To.Add(email.To!);
            mail.BodyEncoding = Encoding.Default;
            return mail;
        }

        private string GetEmailBody(string link)
        {
            var body = File.ReadAllText(string.Format(templatePath, "EmailTemplate"));
            return body.Replace("{link}", link);
        }
        public async Task SendSmtpMail(EmailTemplate data)
        {
            try
            {
                MailMessage message = getMailMessage(data);
                var credentials = new NetworkCredential(_smtpConfigModel.SenderAddress, _smtpConfigModel.SenderPassword);
                // Smtp client
                var client = new SmtpClient()
                {
                    Port = _smtpConfigModel.Port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Host = _smtpConfigModel.Host!,
                    EnableSsl = _smtpConfigModel.EnableSSL,
                    Credentials = credentials,

                };

                await client.SendMailAsync(message);
            }
            catch (Exception e)
            {
                throw new("Excrement occurred", e);
            }
        }

        public void SendEmailVerify(VerificationToken token, string email)
        {
            EmailTemplate emailTemplate = new EmailTemplate();
            emailTemplate.Subject = "Email verify account Netpower Management";
            emailTemplate.Body = _verifyTokenConfig.UrlDirect + "verifyEmail/" + token.UserId + "/" + token.Token;
            emailTemplate.To = email;
            SendSmtpMail(emailTemplate);
            //Console.WriteLine(emailTemplate.Body);
        }


        public void SendEmailForgotPassword(VerificationToken token, string email)
        {
            EmailTemplate emailTemplate = new EmailTemplate();

            emailTemplate.Subject = "Email reset passoword from Netpower Management";
            emailTemplate.Body = _verifyTokenConfig.UrlDirect + "reset-password/" + token.UserId + "/" + token.Token;
            emailTemplate.To = email;
            SendSmtpMail(emailTemplate);
            //Console.WriteLine(emailTemplate.Body);
        }

    }
}
