using System;
using System.Collections.Generic;
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
}