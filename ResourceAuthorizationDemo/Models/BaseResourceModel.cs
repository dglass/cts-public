using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceAuthorizationDemo.Models
{
    public class BaseResourceModel
    {
        public BaseResourceModel()
        {
            Context = HttpContext.Current;
        }

        public static List<BaseResourceModel> ListResources()
        {
            // TODO: determine available resources to list for current user.
            return new List<BaseResourceModel> {
                new BaseResourceModel() { Id = "R001" },
                new BaseResourceModel() { Id = "R002" },
                new BaseResourceModel() { Id = "R003" }
            };
        }

        // alternate constructor takes HttpContextBase from controller to extract Authorization info
/*        public BaseResourceModel(HttpContextBase ctx)
        {
            Context = ctx;
        }
*/

        // Context can be explicitly set as property...(or make read-only?)
        //public HttpContextBase Context { get; set; }
        public HttpContext Context { get; set; } // *NOTE*, Controller gets HttpContextBase...
        public string Id { get; set; } // TODO: should Id be string or int?  thinking string.
    }
}