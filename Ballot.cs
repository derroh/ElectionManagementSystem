//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElectionManagementSystem
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ballot
    {
        public int BallotId { get; set; }
        public string StudentId { get; set; }
        public string ElectionId { get; set; }
        public string PositionId { get; set; }
        public string CandidateId { get; set; }
        public Nullable<int> Vote { get; set; }
    }
}
