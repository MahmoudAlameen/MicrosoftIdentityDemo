using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftIdentityDemo.Core.CustomValidations
{
    public class ShouldContainsLowerCaseValidatorAttribute : ValidationAttribute
    {
        string? ErrorMessage { get; set; }
        public ShouldContainsLowerCaseValidatorAttribute(string? erroMessage = null)
        {
            ErrorMessage = erroMessage;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string input && !string.IsNullOrEmpty(input))
            {
                bool containsUppercase = input.Any(char.IsLower);

                if (!containsUppercase)
                {
                    return new ValidationResult(string.IsNullOrEmpty(ErrorMessage) ?
                        "The field must contain at least one lowercase character." : ErrorMessage);
                }
            }

            return ValidationResult.Success;

        }
    }
}
