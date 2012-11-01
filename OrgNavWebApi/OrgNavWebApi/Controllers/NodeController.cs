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
    public class NodeController : ApiController
    {
        // GET api/node
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/node/5
		// (this is identical to PositionTreeController.Get(id))...
		public object Get(int id)
		{
			// first expand lazy target node:
			var success = AdoRepository<PositionNode>.Exec(
				"ExpandUserPositionNode",
				new Dictionary<string, object>() {
					{"NodeId", id},
					{"AppUserId", 1},
					{"ExpandAllSubNodes", false}
				}
			);

			// TODO: consider Repository as injectable controller dependency...
			//var repo = new Models.AdoRepository<Models.ViewModels.PositionNodeVm>();
			var repo = new AdoRepository<PositionNode>();
			var snl = repo.GetFromProc(
				"GetPositionTree",
				new Dictionary<string, object>() {
					{ "RootPosId", id },
					{ "AppUserId", 1 }
				});
			// TODO: modify TreesFromList to return viewmodels, not models.
			//var trees = PositionNodeVm.TreesFromList(snl.GetRange(1,snl.Count-1)); // omit self ([0])
			var trees = PositionNodeVm.VmTreesFromList(snl.GetRange(1,snl.Count-1)); // omit self ([0])
			//return PositionNodeVm.List(trees);
			return trees;
		}

        // POST api/node
        //public void Post([FromBody]string value)
        //public void Post([FromBody]string id)
        public void Post(string id)
        {
        }

        // PUT api/node/5
		//public void Put(int id, [FromBody]string action) 
		public void Put(int id, Node n) // NOTE, formatter that converts POST body to Node omits URL params! (id)
		{
			bool success;
			switch (n.action)
			{
				case "collapse" :
					// TODO: error handling, logging, etc.
					success =
					AdoRepository<PositionNode>.Exec("CollapseUserPositionNode",
						new Dictionary<string, object>()
						{
							{"NodeId", id},
							{"AppUserId", 1}, // TODO: replace with lookup
							{"IncludeSubNodes", false}
						}
					);
					return;
				case "expand" :
					success =
						AdoRepository<PositionNode>.Exec("ExpandUserPositionNode",
							new Dictionary<string,object>() {
								{"NodeId", id},
								{"AppUserId", 1},
								{"ExpandAllSubNodes", false}
							});
					return;
				default: return;
			}
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
		// *NOTE*, Web API doesn't set route id during model-mapping.  if needed, copy value from route param.
		public int id { get; set; }
		public string action { get; set; }
	}

}
