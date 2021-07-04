using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectionManagementSystem.CustomValidation
{
    public class IsPasswordStrong : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int validConditions = 0;
            string passWord = null;

            if (value is null)
                return false;
            else
                passWord = value.ToString();
                foreach (char c in passWord)
                {
                    if (c >= 'a' && c <= 'z')
                    {
                        validConditions++;
                        break;
                    }
                }
                foreach (char c in passWord)
                {
                    if (c >= 'A' && c <= 'Z')
                    {
                        validConditions++;
                        break;
                    }
                }
                if (validConditions == 0) return false;
                foreach (char c in passWord)
                {
                    if (c >= '0' && c <= '9')
                    {
                        validConditions++;
                        break;
                    }
                }
                if (validConditions == 1) return false;
                if (validConditions == 2)
                {
                    char[] special = { '@', '#', '$', '%', '^', '&', '+', '=' }; // or whatever    
                    if (passWord.IndexOfAny(special) == -1) return false;
                }
            return true;

        }
    }
    public class PasswordSameAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public PasswordSameAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (string)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (string)property.GetValue(validationContext.ObjectInstance);

            if (currentValue == comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}