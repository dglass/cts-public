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
        public List<NodeVm> Get(int id)
        {
			var units = _repo.GetFromProc<Node>("GetOrgUnitTreeCTS", new { });
			return NodeVm.VmTreesFromList(units);
        }

        // POST api/orgunit
        public void Post([FromBody]string value)
        {
        }

        // PUT api/orgunit/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/orgunit/5
        public void Delete(int id)
        {
        }
    }
}