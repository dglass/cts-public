using System;
using System.Collections.Generic;

namespace OrgNavWebApi.Models
{
    public class OrgUnitNode
    {
        public OrgUnitNode()
        {
            this.OrgUnitNode1 = new List<OrgUnitNode>();
        }

        public int Id { get; set; }
        public Nullable<int> ParentId { get; set; }
        public byte[] Ancestry { get; set; }
        public virtual ICollection<OrgUnitNode> OrgUnitNode1 { get; set; }
        public virtual OrgUnitNode OrgUnitNode2 { get; set; }
    }
}
