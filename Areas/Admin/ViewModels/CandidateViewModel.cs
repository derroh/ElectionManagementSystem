using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectionManagementSystem.Areas.Admin.ViewModels
{
    public class CandidateViewModel
    {
        public string ElectionId { get; set; }
        public string StudentId { get; set; }
        public string PositionId { get; set; }
        public string CandidateId { get; set; }
    }
}