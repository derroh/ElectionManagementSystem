using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectionManagementSystem.Areas.Admin.ViewModels
{
    public class ElectionPositionListViewModel
    {
        public string ElectionId { get; set; }
        public string Name { get; set; }
        public string PositionId { get; set; }
        public string Sequence { get; set; }
    }
}