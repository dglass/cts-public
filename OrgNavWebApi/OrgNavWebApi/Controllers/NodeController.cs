using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrgNavWebApi.Controllers
{
    public class NodeController : ApiController
    {
        // GET api/node
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/node/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/node
        //public void Post([FromBody]string value)
        //public void Post([FromBody]string id)
        public void Post(string id)
        {
        }

        // PUT api/node/5
		//public void Put(int id, [FromBody]string action) 
		public void Put(int id, Node n) 
		{
		}

        // DELETE api/node/5
        public void Delete(int id)
        {
        }
    }

	// need this to auto-format request body in Web API.
	// Web API does not cache post request body like MVC.
	// see http://blogs.msdn.com/b/jmstall/archive/2012/04/16/how-webapi-does-parameter-binding.aspx
	public class Node
	{
		public string action { get; set; }
	}

}
