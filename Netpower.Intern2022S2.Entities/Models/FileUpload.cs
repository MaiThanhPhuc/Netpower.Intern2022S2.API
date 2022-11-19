using Microsoft.AspNetCore.Http;
using Netpower.Intern2022S2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netpower.Intern2022S2.Entities.Models
{
    public class FileUpload
    {
        [Required(ErrorMessage = "UserID is required")]
        public Guid UserId { get; set; } = Guid.Empty!;
        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile file { get; set; }
    }
}
