using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrgManagerWebApi.Models;
using OrgManagerWebApi.Models.ViewModels;

namespace OrgManagerWebApi.Controllers
{
    public class OrgUnitController : ApiController
    {
		private IRepository _repo = new AdoRepository("OrganizationNavigatorContext");

        // GET api/orgunit
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/orgunit/5
		[ActionName("NodeOp")]
        public List<NodeVm> Get(int id)
        {
			var units = _repo.GetFromProc<Node>("GetOrgUnitTreeCTS", new { });
			return NodeVm.VmTreesFromList(units);
        }

		[ActionName("GetDetail")]
		public Node GetDetail(int id)
		{
			var n = _repo.GetSingleFromProc<Node>("GetOrgUnitDetail", parameters: new
			{
				Id = id
			});
			return n;
		}

        // POST api/orgunit
        //public void Post([FromBody]string value)
        //public NodeVm Post(int id, [FromBody]string value)
		[ActionName("NodeOp")]
        public NodeVm Post(int id) // id=id of Parent node.  if passing POST args, need receiving Model.
        {
			var n =	_repo.GetSingleFromProc<Node>("AddOrgUnitCTS", parameters: new
				{
					ParentId = id
				});
			var vm = new NodeVm(n);
			// TODO: parameter testing, Exception on mismatched params.
			return vm;
        }

        // PUT api/orgunit/5
		[ActionName("NodeOp")]
		public HttpResponseMessage Put(int id, Node updated)
        {
			switch (updated.UpdateAction)
			{
				case "move":
					if (!
						_repo.Exec("MoveOrgUnitCTS", parameters: new {
							Id = id,
							ParentId = updated.ParentId,
							SibIndex = updated.SibIndex
						}))
					{
						return new HttpResponseMessage(HttpStatusCode.InternalServerError)
						{
							ReasonPhrase = "node move failed."
						};
					}
					break;
				case "rename":
					if (!
						_repo.Exec("RenameOrgUnitCTS", parameters: new
						{
							Id = id,
							Name = updated.Name
						}))
					{
						return new HttpResponseMessage(HttpStatusCode.InternalServerError)
						{
							ReasonPhrase = "unit rename failed."
						};
					}
					break;
				case "update":
					if (!
						_repo.Exec("UpdateOrgUnitCTS", parameters: new
						{
							Id = id,
							Code = updated.Code,
							ShortName = updated.ShortName
						}))
					{
						return new HttpResponseMessage(HttpStatusCode.InternalServerError)
						{
							ReasonPhrase = "unit update failed."
						};
					}
					break;
				default: break;
			}
			return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // DELETE api/orgunit/5
		// returning HRM instead of void, per:
		// http://www.asp.net/web-api/overview/creating-web-apis/creating-a-web-api-that-supports-crud-operations
		// TODO: pre-check for delete authorization and leaf status before attempting delete.
		// (this is also where lock checks should occur when branch locking is enabled).
		[ActionName("NodeOp")]
		public HttpResponseMessage Delete(int id)
        {
			if (_repo.Exec("DeleteOrgUnitCTS", parameters: new { Id = id }))
			{
				return new HttpResponseMessage(HttpStatusCode.OK);
			}
			else {
				// TODO: more robust exception logging / handling in SmartProc.
				// *NOTE*, this could actually be blocked at client; normal use would never reach server.
				return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed)
				{
					ReasonPhrase = "deleting non-leaf nodes is currently disabled."
				};
			}
        }
    }
}