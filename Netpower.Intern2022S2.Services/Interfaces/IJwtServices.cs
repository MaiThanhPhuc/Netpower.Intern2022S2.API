using Netpower.Intern2022S2.DTOs;
using Netpower.Intern2022S2.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Netpower.Intern2022S2.Services.Interfaces
{
    public interface IJwtServices
    {
        public Task<string> GetJwtAsync(Guid id);
        public Task<VerificationToken> GenerateToken(User user, int time);

    }
}
