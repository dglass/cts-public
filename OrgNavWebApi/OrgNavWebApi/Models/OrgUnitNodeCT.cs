using System;
using System.Collections.Generic;

namespace OrgNavWebApi.Models
{
    public class OrgUnitNodeCT
    {
        public OrgUnitNodeCT()
        {
            this.OrgUnitNodeCTS1 = new List<OrgUnitNodeCT>();
        }

        public int Id { get; set; }
        public Nullable<int> ParentId { get; set; }
        public byte[] Ancestry { get; set; }
        public virtual OrgUnitCT OrgUnitCT { get; set; }
        public virtual ICollection<OrgUnitNodeCT> OrgUnitNodeCTS1 { get; set; }
        public virtual OrgUnitNodeCT OrgUnitNodeCT1 { get; set; }
    }
}
