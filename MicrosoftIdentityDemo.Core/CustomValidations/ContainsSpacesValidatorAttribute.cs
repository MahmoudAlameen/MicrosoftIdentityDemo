using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MicrosoftIdentityDemo.Core.CustomValidations
{
    public class ContainsSpacesValidatorAttribute : ValidationAttribute
    {
        string ErrorMessage { get; set; }
        public ContainsSpacesValidatorAttribute(string erroMessage)
        {
            ErrorMessage = erroMessage;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string v = (string)value;
                if (v.Contains(" "))
                    return new ValidationResult(ErrorMessage);

                return ValidationResult.Success;
            }
            return null;
        }
    }
}
