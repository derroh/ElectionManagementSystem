using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectionManagementSystem.Areas.Admin.ViewModels
{
    public class ElectionViewModel
    {
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ElectionId { get; set; }
    }
}