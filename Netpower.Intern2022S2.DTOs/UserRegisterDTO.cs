using Netpower.Intern2022S2.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netpower.Intern2022S2.DTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Last Name is required")]

        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "Email is required")]

        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;

        public UserRegisterDTO()
        {
        }


    }
}
