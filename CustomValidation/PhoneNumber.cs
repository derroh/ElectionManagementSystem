using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.CustomValidation
{
    public class IsPhoneNumberValid : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is null)
                return false;
            else
                return RegexFunctions.IsValidPhoneNumber(value.ToString());           
               
        }
    }
    public class IsPhoneNumberAvailable : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is null)
                return false;
            else
                return AppFunctions.IsPhoneAvailable(value.ToString());

        }
    }
}