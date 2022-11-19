using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Netpower.Intern2022S2.DTOs;
using Netpower.Intern2022S2.Entities.Models;
using Netpower.Intern2022S2.Repositories.Interfaces;
using Netpower.Intern2022S2.Services.Interfaces;


namespace Netpower.Intern2022S2.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly VerifyTokenConfigModel _verifyTokenConfig;
        private readonly IEmailServices _emailServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtServices _jwtServices;

        public AccountServices(IOptions<VerifyTokenConfigModel> verifyTokenConfig, IEmailServices emailServices, IUnitOfWork unitOfWork, IJwtServices jwtServices)
        {
            _verifyTokenConfig = verifyTokenConfig.Value;
            _emailServices = emailServices;
            _unitOfWork = unitOfWork;
            _jwtServices = jwtServices;
        }

        // Login
        public async Task<ApiResponse> Login(UserLoginDTO userLogin)
        {
            var user = _unitOfWork.UserRepository.DbSet.SingleOrDefault(x => x.Email.Equals(userLogin.Email));
            if (user == null)
            {
                return new ApiResponse
                {
                    StatusCode = 400,
                    Message = "User or password incorrect!",
                    Data = null!
                };
            }
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(userLogin.Password, user?.Password);
            if (!isValidPassword)
            {
                return new ApiResponse
                {
                    StatusCode = 400,
                    Message = "User or password incorrect!",
                    Data = null!
                };
            }
            if (user?.Status != 1)
            {
                return new ApiResponse
                {
                    StatusCode = 400,
                    Message = "User not yet verify account!",
                    Data = null!
                };
            }
            var token = await _jwtServices.GetJwtAsync(user.UserId);
            if (token == null)
            {
                return new ApiResponse
                {
                    StatusCode = 500,
                    Message = "Can not generate token!",
                    Data = null!
                };
            }
            return new ApiResponse
            {
                StatusCode = 200,
                Message = "Authenticate success",
                Data = token
            };
        }

        // Forgot Password
        public async Task<ApiResponse> ForgotPassword(string email)
        {
            var user = _unitOfWork.UserRepository.DbSet.FirstOrDefault(e => e.Email == email);
            var newToken = new VerificationToken();
            if (user == null)
            {
                return new ApiResponse(400, "Email not exist", null!);

            }
            newToken = await _jwtServices.GenerateToken(user, _verifyTokenConfig.ForgotExpiredHours);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                _emailServices.SendEmailForgotPassword(newToken, user.Email);
            }
            catch (DbUpdateException)
            {
                return new ApiResponse(500, "Error while saving data", null!);
            }
            return new ApiResponse(200, "Please check your email to reset password!", null!);
        }

        // Register
        public async Task<ApiResponse> Register(UserRegisterDTO register)
        {
            var emailExist = CheckEmailExixts(register.Email);
            if (emailExist)
            {
                return new ApiResponse(400, "Email already exists", null!);
            }
            User newUser = new User();
            newUser.Email = register.Email;
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(register.Password);
            newUser.UserId = Guid.NewGuid();
            newUser.FirstName = register.FirstName;
            newUser.LastName = register.LastName;

            VerificationToken newToken = new VerificationToken();

            _unitOfWork.UserRepository.Add(newUser);
            
            
            newToken = await _jwtServices.GenerateToken(newUser, _verifyTokenConfig.VerifyExpiredHours);
            try
            {
                await _unitOfWork.SaveChangesAsync();
                await CreateProfile(newUser.UserId);
                _emailServices.SendEmailVerify(newToken, newUser.Email);
            }
            catch (DbUpdateException)
            {
                return new ApiResponse(500, "Error while saving data", null!);
            }
            return new ApiResponse(200, "Please check email to verify account!", null!);
        }

        public async Task<ApiResponse> ResendVerifyMail(Guid UserID)
        {

            var user = await _unitOfWork.UserRepository.GetByIdAsync(UserID);
            VerificationToken newToken = new VerificationToken();
            if (user == null)
            {
                return new ApiResponse(400, "Invalid user", null!);
            }
            newToken = await _jwtServices.GenerateToken(user, _verifyTokenConfig.VerifyExpiredHours);
            try
            {
                _emailServices.SendEmailVerify(newToken, user.Email);
            }
            catch (DbUpdateException)
            {
                return new ApiResponse(500, "Error while sending", null!);
            }
            return new ApiResponse(200, "Resend Success", null!);
        }


        // Reset password
        public async Task<ApiResponse> ResetPassword(Guid id, string token, string password)
        {

            var temp = _unitOfWork.VerificationRepository.DbSet.FirstOrDefault(x => x.Token == token);
            var user = _unitOfWork.UserRepository.GetByIdAsync(id).Result;

            if (user == null)
            {
                return new ApiResponse(400, "Invalid user", null!);
            }

            if (temp == null)
            {
                return new ApiResponse(400, "Invalid token", null!);
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            // need hash in frontend
            if (temp.ExpriredTime >= DateTime.Now && temp.Token == token)
            {
                _unitOfWork.UserRepository.Update(user);
                try
                {
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    return new ApiResponse(500, "Error while sending", null!);
                }
            }
            else
            {
                return new ApiResponse(401, "Link active is expired", null!);
            }
            return new ApiResponse(200, "Reset Password Success", null!);
        }

        public async Task<ApiResponse> ResendResetPasswordMail(Guid UserID)
        {

            var user = await _unitOfWork.UserRepository.GetByIdAsync(UserID);
            VerificationToken newToken = new VerificationToken();
            if (user == null)
            {
                return new ApiResponse(400, "Invalid user", null!);
            }
            newToken = await _jwtServices.GenerateToken(user, _verifyTokenConfig.VerifyExpiredHours);
            try
            {
                _emailServices.SendEmailForgotPassword(newToken, user.Email);
            }
            catch (DbUpdateException)
            {
                return new ApiResponse(500, "Error while sending", null!);
            }
            return new ApiResponse(200, "Resend Success", null!);
        }


        public async Task<ApiResponse> CheckVerifyEmail(Guid UserID, string token)
        {
            var temp = _unitOfWork.VerificationRepository.DbSet.FirstOrDefault(x => x.Token == token);
            var user = await _unitOfWork.UserRepository.GetByIdAsync(UserID);

            if (temp == null)
            {
                return new ApiResponse(400, "Invalid token", null!);
            }
            if (user == null)
            {
                return new ApiResponse(400, "Invalid user", null!);
            }

            user.Status = 1;
            user.StatusDate = DateTime.UtcNow;
            if (temp.ExpriredTime >= DateTime.Now)
            {
                _unitOfWork.UserRepository.Update(user);
                try
                {
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    return new ApiResponse(500, "Error while saving data", null!);
                }
            }
            else
            {
                return new ApiResponse(400, "Link active is expired", null!);
            }
            return new ApiResponse(200, "Verify success", null!);
        }


        private bool CheckEmailExixts(string email)
        {
            return _unitOfWork.UserRepository.DbSet.Any(e => e.Email == email);
        }
        private async Task<Profile> CreateProfile(Guid id)
        {
            var profile = new Profile();
            profile.UserId = id;
            _unitOfWork.ProfileRepository.Add(profile);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            return profile;
        }
    }
}
