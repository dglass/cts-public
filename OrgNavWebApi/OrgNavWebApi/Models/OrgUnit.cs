using System;
using System.Collections.Generic;

namespace OrgNavWebApi.Models
{
    public class OrgUnit
    {
        public OrgUnit()
        {
            this.PositionRBS = new List<PositionRB>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string RBSCode { get; set; }
        public virtual ICollection<PositionRB> PositionRBS { get; set; }
    }
}
