using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectionManagementSystem.CustomValidation
{
    public class IsStudentIdAvailable : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is null)
                return false;
            else
                return AppFunctions.IsStudentAvailable(value.ToString());

        }
    }
}