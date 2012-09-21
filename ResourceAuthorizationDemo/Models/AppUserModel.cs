using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceAuthorizationDemo.Models
{
    public class AppUserModel : BaseResourceModel
    {
        public bool CanRead(BaseResourceModel brm)
        {
            // dummy logic here...
            return true;
        }
    }
}