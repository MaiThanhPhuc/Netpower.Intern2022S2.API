using Netpower.Intern2022S2.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netpower.Intern2022S2.DTOs
{
    public class UserDTO
    {
        public Guid UserId { get; set; } = Guid.Empty!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;

        public UserDTO(User user)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
        }

        public UserDTO()
        {
        }

        public User UserDTOtoUser(User user)
        {
            user.FirstName = this.FirstName;
            user.LastName = this.LastName;
            user.Email = this.Email;
            return user;
        }
    }

}
