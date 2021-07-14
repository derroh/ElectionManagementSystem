using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectionManagementSystem.Areas.Admin.ViewModels
{
    public class StudentViewModel
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public YearOfStudy YearOfStudy { get; set; }
        public string Faculty { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}