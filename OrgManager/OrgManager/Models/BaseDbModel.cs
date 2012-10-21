using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

namespace OrgManager.Models
{
	// TODO: subclass BaseResource...
    public class BaseDbModel
    {
        public static string ConnStr { get; set; }
        public int Id { get; set; }
        public BaseDbModel()
        {
            ConnStr = WebConfigurationManager.ConnectionStrings[0].ConnectionString;
        }
    }
}