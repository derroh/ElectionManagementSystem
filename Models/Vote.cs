using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectionManagementSystem.Models
{
    public class Vote
    {
        public string ElectionID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Positions { get; set; }
        public PositionsOnBallot[] PositionsOnBallot { get; set; }
    }

    public class PositionsOnBallot
    {
        public string PositionId { get; set; }
        public string PositionName { get; set; }
        public Candidates[] Candidates { get; set; }
    }

    public class Candidates
    {
        public string CandidateID { get; set; }
        public string CandidateName { get; set; }
        public string CandidateStudentId { get; set; }
        public string ElectralPositionId { get; set; }
    }
}


