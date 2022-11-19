using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Netpower.Intern2022S2.DTOs;
using Netpower.Intern2022S2.Entities.Models;
using Netpower.Intern2022S2.Repositories;
using Netpower.Intern2022S2.Repositories.Interfaces;
using Netpower.Intern2022S2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Netpower.Intern2022S2.Services
{
    public class JwtServices : IJwtServices
    {
        private const int EXPIRED_TIME_JWT_DAYS = 1;
        
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;


        public JwtServices(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GetJwtAsync( Guid id)
        {
           
            var jwtKey = _configuration.GetValue<string>("JWT:SecretKey");

            var keyBytes = Encoding.ASCII.GetBytes(jwtKey);

            var tokenHandler = new JwtSecurityTokenHandler();

            var decriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),

                }),
                Expires = DateTime.Now.AddDays(EXPIRED_TIME_JWT_DAYS),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256),
            };

            var token = tokenHandler.CreateToken(decriptor);
            return await Task.FromResult(tokenHandler.WriteToken(token));
        }

        public async Task<VerificationToken> GenerateToken(User user, int time)
        {
            Guid token = Guid.NewGuid();
            VerificationToken newToken = new VerificationToken();
            newToken.UserId = user.UserId;
            newToken.Token = token.ToString();
            //newToken.ExpriredTime = DateTime.Now.AddHours(time);
            newToken.ExpriredTime = DateTime.Now.AddSeconds(20);
            _unitOfWork.VerificationRepository.Add(newToken);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            return newToken;
        }

    }
}
