using System;
using System.Collections.Generic;

namespace Netpower.Intern2022S2.Entities.Models
{
    public partial class VerificationToken
    {
        public Guid TokenId { get; set; }
        public Guid UserId { get; set; }
        public string? Token { get; set; }
        public DateTimeOffset? ExpriredTime { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
