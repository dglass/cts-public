using System;
using System.Collections.Generic;

namespace OrgNavWebApi.Models
{
    public class OrgUnitCT
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string RbsCode { get; set; }
        public Nullable<int> HrmsOrgUnitId { get; set; }
        public virtual OrgUnitNodeCT OrgUnitNodeCT { get; set; }
    }
}
