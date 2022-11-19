using Netpower.Intern2022S2.DTOs;

namespace Netpower.Intern2022S2.Services.Interfaces
{
    public interface IAccountServices
    {
        public Task<ApiResponse> ForgotPassword(string email);
        public Task<ApiResponse> Login(UserLoginDTO userLogin);
        public Task<ApiResponse> Register(UserRegisterDTO register);
        public Task<ApiResponse> CheckVerifyEmail(Guid UserID, string token);
        public Task<ApiResponse> ResendVerifyMail(Guid UserID);
        public Task<ApiResponse> ResetPassword(Guid id, string token, string password);
        public Task<ApiResponse> ResendResetPasswordMail(Guid UserID);

    }
}