using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ElectionManagementSystem.Models
{
    public class ElectionCandidateView
    {
        public ElectionCandidate electionCandidate { get; set; }
        public Student student { get; set; }
        public ElectionPosition electionPosition { get; set; }
    }
   
}