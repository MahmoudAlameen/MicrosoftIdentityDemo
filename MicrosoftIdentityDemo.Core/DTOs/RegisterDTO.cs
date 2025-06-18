using MicrosoftIdentityDemo.Core.CustomValidations;
using MicrosoftIdentityDemo.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftIdentityDemo.Core.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="Full Name of the user is required")]
        public string FullName { get; set; }
        public string FullNameAR { get; set; } = string.Empty;
        [Required(ErrorMessage ="email is required")]
        [EmailAddress(ErrorMessage = "email format is invalid")]
        public string Email { get; set; }
        [Phone(ErrorMessage ="phone format is invalid")]
        public string PhoneNumber { get; set; }

        public Role Role { get; set; }
        [Required(ErrorMessage ="password is required")]
        [MinLength(6, ErrorMessage ="minmum number of characters allowed is 6 characters")]
        [MaxLength(20, ErrorMessage ="maximum number of characters allowed is 20 characters")]
        [ShouldContainsLowerCaseValidator]
        [ShouldContainsUpperCaseValidator]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password), ErrorMessage ="password and confirm password not matched ")]
        public string ConfirmPassword { get; set; }
    }
}
