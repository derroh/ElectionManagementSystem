using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectionManagementSystem.Areas.Admin.ViewModels
{
    public class ElectionPositionViewModel
    {
        public string ElectionId { get; set; }
        public string Name { get; set; }
        public string PositionId { get; set; }
        public int Sequence { get; set; }
    }
}