using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netpower.Intern2022S2.Entities.Models;


namespace Netpower.Intern2022S2.DTOs
{
    public class ProfileDTO
    {
        public ProfileDTO()
        {
        }

        public ProfileDTO(Profile profile)
        {
            Sex = profile.Sex;
            Birthday = profile.Birthday;
            Nationality = profile.Nationality;
            Address = profile.Address;
            PhoneNumber = profile.PhoneNumber;
            Image = profile.Image;
            UserId = profile.UserId;
        }
        public Guid UserId { get; set; } = Guid.Empty!;
        public bool? Sex { get; set; }
        public DateTimeOffset? Birthday { get; set; }
        public string? Nationality { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public byte[]? Image { get; set; }

        public Profile ProfileDTOToProfile(Profile profile)
        {
            profile.PhoneNumber = PhoneNumber;
            profile.Birthday = Birthday;
            profile.Nationality = Nationality;
            profile.Address = Address;
            profile.Sex = Sex;

            return profile;
        }
    }
}
