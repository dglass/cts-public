using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrgNavWebApi.Models;
using OrgNavWebApi.Models.ViewModels;

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
			//var repo = new Models.AdoRepository<Models.ViewModels.PositionNodeVm>();
			var repo = new AdoRepository<PositionNode>();
			var rootVm = new PositionNodeVm(PositionNodeVm.TreesFromList(
				repo.GetFromProc("GetPositionTree", new Dictionary<string,object>() {
					{ "RootPosId", id },
					{ "AppUserId", 1 }
				}))[0]); // takes only first root node
			return rootVm;
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