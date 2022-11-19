using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netpower.Intern2022S2.Entities.Models
{
    public class SMTPConfigModel
    {
        public string? SenderAddress { get; set; }
        public string? SenderPassword { get; set; }
        public string? SenderDisplayName { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public bool IsBodyHTML { get; set; }

    }
}
