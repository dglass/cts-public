using System;
using System.Collections.Generic;

namespace OrgNavWebApi.Models
{
    public class OrgStructureSAP
    {
        public int OrgUnitId { get; set; }
        public int ParentOrgUnitId { get; set; }
    }
}
