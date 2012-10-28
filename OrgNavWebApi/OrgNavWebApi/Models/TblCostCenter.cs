using System;
using System.Collections.Generic;

namespace OrgNavWebApi.Models
{
    public class TblCostCenter
    {
        public string FYr { get; set; }
        public string Division { get; set; }
        public string RespCntr { get; set; }
        public string BsnCntr { get; set; }
        public string CstCntr { get; set; }
        public string DscrptnNameShort { get; set; }
        public string DscrptnNameLong { get; set; }
        public string CCType { get; set; }
        public string MgrLANId { get; set; }
        public Nullable<int> PersonId { get; set; }
        public string BudgetContacts { get; set; }
        public string Status { get; set; }
        public string CreatorLANId { get; set; }
        public Nullable<System.DateTime> CreatorDateTime { get; set; }
        public string ModifierLANId { get; set; }
        public Nullable<System.DateTime> ModifierDateTime { get; set; }
    }
}
