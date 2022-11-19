using Netpower.Intern2022S2.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netpower.Intern2022S2.Services.Interfaces
{
    public interface IUserServices
    {
        public List<UserDTO> Get();
        public Task<ApiResponse> GetById(Guid id);
        //public Task<ApiResponse> Post(UserDTO user);
        public Task<ApiResponse> PutUser(UserUpdateDTO userDTO);
        
    }
}
