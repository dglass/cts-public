using OrgNavWebApi.Models;
using OrgNavWebApi.Models.ViewModels;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Http;

namespace OrgNavWebApi.Controllers
{
    public class NodeController : ApiController
    {
		private IRepository _repo = new AdoRepository("OrganizationNavigatorContext");

		[ActionName("NodeOp")]
		public object GetRoot(int id)
		{
			var rootVm = PositionNodeVm.VmTreesFromList(
				_repo.GetFromProc<PositionNode>("GetPositionTree",
					parameters: new
					{
						RootPosId = id,
						AppUserId = AppUserId()
					})
				);
			return rootVm;
		}

        // GET api/node/5/subnodes
		public object GetLazyExpand(int id)
		{
			// first expand lazy target node:
			_repo.Exec(	
				"ExpandUserPositionNode",
				parameters: new
				{
					NodeId = id,
					AppUserId = AppUserId(),
					ExpandAllSubNodes = false
				});

			// TODO: consider Repository as injectable controller dependency...
			var snl = _repo.GetFromProc<PositionNode>(
				"GetPositionTree",
				parameters: new {
					RootPosId = id,
					AppUserId = AppUserId()
				});
			var trees = PositionNodeVm.VmTreesFromList(snl.GetRange(1,snl.Count-1)); // omit self ([0])
			return trees;
		}

        // POST api/node
		[ActionName("NodeOp")]
        public void Post(string id)
        {
        }

        // PUT api/node/5
		[ActionName("NodeOp")]
		public void Put(int id, Node n) // NOTE, formatter that converts POST body to Node omits URL params! (id)
		{
			switch (n.action)
			{
				case "collapse" :
					// TODO: error handling, logging, etc.
					_repo.Exec("CollapseUserPositionNode",
						parameters: new {
							NodeId = id,
							AppUserId = AppUserId(),
							IncludeSubNodes = false
						}
					);
					return;
				case "expand" :
					_repo.Exec("ExpandUserPositionNode",
						parameters: new {
							NodeId = id,
							AppUserId = AppUserId(),
							ExpandAllSubNodes = false
						}
					);
					return;
				default: return;
			}
		}

        // DELETE api/node/5
		[ActionName("NodeOp")]
        public void Delete(int id)
        {
        }

		private int AppUserId()
		{
			// this will fail unless Windows Auth is enabled, Anonymous disabled:
			var wi = (WindowsIdentity)HttpContext.Current.User.Identity;
			var sid = wi.User;
			var binsid = new byte[sid.BinaryLength];
			sid.GetBinaryForm(binsid, 0);
			// TODO: Write-through Caching Repository per-user
			return _repo.GetSingleFromProc<AppUser>("GetUserId",
				parameters: new {
				LanId = wi.Name,
				SidString = sid.Value,
				SidHexString = BitConverter.ToString(binsid,0)
			}).Id;
		}
    }

	// need this to auto-format request body in Web API.
	// Web API does not cache post request body like MVC.
	// see http://blogs.msdn.com/b/jmstall/archive/2012/04/16/how-webapi-does-parameter-binding.aspx
	public class Node
	{
		// *NOTE*, Web API doesn't set map route-based params (e.g. id) on model
		// during model-mapping.  if needed, copy value into such model properties.
		public int id { get; set; }
		public string action { get; set; }
	}
}