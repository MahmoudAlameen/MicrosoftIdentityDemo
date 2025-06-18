using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftIdentityDemo.Core.CustomValidations
{
    public class ShouldContainsUpperCaseValidatorAttribute : ValidationAttribute
    {
        string? ErrorMessage { get; set; }
        public ShouldContainsUpperCaseValidatorAttribute(string? erroMessage = null)
        {
            ErrorMessage = erroMessage;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string input && !string.IsNullOrEmpty(input))
            {
                bool containsUppercase = input.Any(char.IsUpper);

                if (!containsUppercase)
                {
                    return new ValidationResult(string.IsNullOrEmpty(ErrorMessage) ?
                        "The field must contain at least one uppercase character." : ErrorMessage);
                }
            }

            return ValidationResult.Success;

        }
    }
}
