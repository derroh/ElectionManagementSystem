using ElectionManagementSystem.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectionManagementSystem.Models
{
    public class PasswordResetViewModel
    {

        [Required(ErrorMessage = "Please enter email")]
        [IsValidEmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }
    }
}