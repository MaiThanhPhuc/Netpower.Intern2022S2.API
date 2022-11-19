using System;
using System.Collections.Generic;

namespace Netpower.Intern2022S2.Entities.Models
{
    public partial class User
    {
        public User()
        {
            VerificationTokens = new HashSet<VerificationToken>();
        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? Status { get; set; }
        public DateTimeOffset? StatusDate { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Profile? Profile { get; set; }
        public virtual ICollection<VerificationToken> VerificationTokens { get; set; }
    }
}
