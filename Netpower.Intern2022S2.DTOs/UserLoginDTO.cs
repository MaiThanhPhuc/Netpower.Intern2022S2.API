using Netpower.Intern2022S2.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netpower.Intern2022S2.DTOs
{
    public class UserLoginDTO
    {
        public UserLoginDTO()
        {
        }
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;

    }
}
