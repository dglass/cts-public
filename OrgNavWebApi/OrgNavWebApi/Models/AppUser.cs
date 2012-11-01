using System;
using System.Collections.Generic;

namespace OrgNavWebApi.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string LanId { get; set; }
        public string SidString { get; set; }
        public string SidHexString { get; set; }
    }
}
