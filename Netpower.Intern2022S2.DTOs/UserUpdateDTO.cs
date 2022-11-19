using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netpower.Intern2022S2.DTOs
{
    public class UserUpdateDTO
    {
        public UserUpdateDTO()
        {
        }

        public UserUpdateDTO(Guid userId, string firstName, string lastName, string password)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
