using ElectionManagementSystem.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ElectionManagementSystem.Models
{
    public class RegisterUser
    {
        public int idUser { get; set; }

        [Required(ErrorMessage = "Please enter first name")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Please enter last name")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Please enter email")]
        [IsValidEmailAddress(ErrorMessage = "Please enter a valid email")]
        [IsEmailAddressAvailable(ErrorMessage = "An account with the email provided already exists")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Please enter a password")]
      //  [IsPasswordStrong(ErrorMessage = "Please provide a strong password")]
        public string Password { get; set; }



        // [Required(ErrorMessage = "Please confirm your password")]
        // [IsPasswordStrong(ErrorMessage = "Kindly provide a strong password")]
        // [PasswordSame("Password", ErrorMessage = "Your passwords do not match")]
        [Compare("Password", ErrorMessage = "Password and confirm password does not match")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Please enter your phone number")]
        [IsPhoneNumberValid(ErrorMessage = "Please enter a valid phone number")]
        [IsPhoneNumberAvailable(ErrorMessage = "An account with the phone number provided already exists")]
        public string Phone { get; set; }


        [Required(ErrorMessage = "Please enter your student Id")]
        [IsStudentIdAvailable(ErrorMessage = "An account with the student Id provided already exists")]
        public string StudentId { get; set; }


        public string Role { get; set; }

        [Required(ErrorMessage = "You must agree to the T & Cs")]
        public bool IsAcceptedTC { get; set; }
    }
}