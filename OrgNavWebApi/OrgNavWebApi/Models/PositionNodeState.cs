using System;
using System.Collections.Generic;

namespace OrgNavWebApi.Models
{
    public class PositionNodeState
    {
        public int PositionNodeId { get; set; }
        public int AppUserId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
