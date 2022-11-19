using System;
using System.Collections.Generic;

namespace Netpower.Intern2022S2.Entities.Models
{
    public partial class Profile
    {
        public Guid UserId { get; set; }
        public bool? Sex { get; set; }
        public DateTimeOffset? Birthday { get; set; }
        public string? Nationality { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public byte[]? Image { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
