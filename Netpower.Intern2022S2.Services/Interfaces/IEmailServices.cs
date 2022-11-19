using Netpower.Intern2022S2.Entities.Models;

namespace Netpower.Intern2022S2.Services.Interfaces
{
    public interface IEmailServices
    {
        public Task SendSmtpMail(EmailTemplate data);
        public void SendEmailVerify(VerificationToken token, string email);
        public void SendEmailForgotPassword(VerificationToken token, string email);

    }
}