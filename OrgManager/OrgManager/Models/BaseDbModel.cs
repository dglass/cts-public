using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

namespace OrgManager.Models
{
    public class BaseDbModel
    {
        public string ConnStr { get; set; }
        public int Id { get; set; }
        public BaseDbModel()
        {
            ConnStr = WebConfigurationManager.ConnectionStrings[0].ConnectionString;
        }
    }
}