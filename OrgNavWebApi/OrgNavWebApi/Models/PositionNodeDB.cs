using System;
using System.Collections.Generic;

namespace OrgNavWebApi.Models
{
    public class PositionNodeDB
    {
        public PositionNodeDB()
        {
            this.PositionNode1 = new List<PositionNodeDB>();
        }

        public int Id { get; set; }
        public Nullable<int> SuperId { get; set; }
        public byte[] Ancestry { get; set; }
        public virtual ICollection<PositionNodeDB> PositionNode1 { get; set; }
        public virtual PositionNodeDB PositionNode2 { get; set; }
    }
}
