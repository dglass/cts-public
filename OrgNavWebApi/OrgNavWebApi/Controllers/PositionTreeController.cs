using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrgNavWebApi.Controllers
{
    public class PositionTreeController : ApiController
    {
        // GET api/positiontree
		// (multiple roots (parentless nodes))
        public IEnumerable<object> Get()
        {
            return new object[] { "value1", "value2" };
        }

        // GET api/positiontree/5
        //public Models.ViewModels.PositionNodeVm Get(int id)
        public object Get(int id)
        {
			// TODO: consider Repository as injectable controller dependency...
			var repo = new Models.AdoRepository<Models.ViewModels.PositionNodeVm>();
			return repo.Get(id);
        }

		// CUD ops should post to api/positionnode (not tree)
		//// POST api/positiontree
		//public void Post([FromBody]string value)
		//{
		//}

		//// PUT api/positiontree/5
		//public void Put(int id, [FromBody]string value)
		//{
		//}

		//// DELETE api/positiontree/5
		//public void Delete(int id)
		//{
		//}
    }
}