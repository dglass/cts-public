using System;
using System.Collections.Generic;

namespace OrgNavWebApi.Models
{
    public class AppUser
    {
        public AppUser()
        {
            this.PositionNodeStates = new List<PositionNodeState>();
        }

        public int Id { get; set; }
        public string LanId { get; set; }
        public string SidString { get; set; }
        public string SidHexString { get; set; }
        public virtual ICollection<PositionNodeState> PositionNodeStates { get; set; }
    }
}
