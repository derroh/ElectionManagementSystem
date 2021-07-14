using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectionManagementSystem
{
    enum ApprovalStatus
    {        
        Created = 0,
        Open = 1,
        Canceled = 2,
        Approved = 3,
        Rejected = 4,
    }
    public enum ElectionStatus
    {
        Created = 0,
        Open = 1,
        Closed = 2,
        Archived = 3
    }
    public enum YearOfStudy
    {
        [Display(Name = "First Year")]
        First = 1,
        [Display(Name = "Second Year")]
        Second = 2,
        [Display(Name = "Third Year")]
        Third = 3,
        [Display(Name = "Fourth Year")]
        Fourth = 4
    }
}