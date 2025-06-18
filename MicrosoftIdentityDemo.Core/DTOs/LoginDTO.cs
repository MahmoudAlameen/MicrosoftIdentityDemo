using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftIdentityDemo.Core.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress(ErrorMessage ="email format is invalid")]
        public string Email { get; set; }
        public string Password { get; set; }    
    }
}
