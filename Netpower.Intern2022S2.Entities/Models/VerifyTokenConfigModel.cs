using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netpower.Intern2022S2.Entities.Models
{
    public class VerifyTokenConfigModel
    {
        public int VerifyExpiredHours { get; set; }
        public int ForgotExpiredHours { get; set; }
        public string? UrlDirect { get; set; }
    }
}
