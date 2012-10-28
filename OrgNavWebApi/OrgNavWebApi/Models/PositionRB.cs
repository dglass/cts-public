using System;
using System.Collections.Generic;

namespace OrgNavWebApi.Models
{
    public class PositionRB
    {
        public int Id { get; set; }
        public Nullable<int> OrgUnitId { get; set; }
        public string RbsCode { get; set; }
        public virtual OrgUnit OrgUnit { get; set; }
    }
}
