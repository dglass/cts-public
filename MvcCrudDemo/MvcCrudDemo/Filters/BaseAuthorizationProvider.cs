using MvcCrudDemo.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MvcCrudDemo.Filters
{
    public class BaseAuthorizationProvider
    {
    	private readonly Dictionary<string, ActionFlags> _actions = new Dictionary<string, ActionFlags>();
        private readonly HttpContextBase _context;
        private readonly HttpVerbs _httpverb;
        private readonly string _action;
        private readonly string _controller;
        //private readonly string _id; // specific record id, if provided
        private readonly Dictionary<HttpVerbs, ModelAction> _verbaction = new Dictionary<HttpVerbs, ModelAction> {
            { HttpVerbs.Post, ModelAction.Create },
            { HttpVerbs.Get, ModelAction.Read },
            { HttpVerbs.Put, ModelAction.Update },
            { HttpVerbs.Delete, ModelAction.Delete }
        };

    	public BaseAuthorizationProvider(HttpContextBase hcb)
        {
            _context = hcb;
            var rdv = ((MvcHandler)_context.CurrentHandler).RequestContext.RouteData.Values;
            _httpverb = (HttpVerbs)(Enum.Parse(typeof(HttpVerbs), _context.Request.GetHttpMethodOverride(), true)); // returns actual method if no override specified.
            // assumes specific Resources will be accessed by Id (see BaseResourceModel)
            // _id = rdv.ContainsKey("Id") ? rdv["Id"].ToString() : "";
            _controller = rdv["controller"].ToString();
            _action = rdv["action"].ToString();
            //var n = _context.User.Identity.Name;
            Init();
        }

        private void Init()
        {
            // initialize permissions from cached UserPermissions (test purposes only)...
            // NOTE, this must occur *BEFORE* any Controller actions.
            _context.Cache["UserPermissions"] = _context.Cache["UserPermissions"] ?? new UserPermissions();
            var up = (UserPermissions)_context.Cache["UserPermissions"];
            var af = new ActionFlags();
            af = up.PermitCreate ? af | ActionFlags.Create : af;
            af = up.PermitRead ? af | ActionFlags.Read : af;
            af = up.PermitUpdate ? af | ActionFlags.Update : af;
            af = up.PermitDelete ? af | ActionFlags.Delete : af;
            Allow(_controller, af);
        }

        // set individual flag:
        public void Allow(string controller, ModelAction a) {
            _actions[controller] = _actions.ContainsKey(controller) ? _actions[controller] | (ActionFlags)a : (ActionFlags)a;
        }

        // sets all flags at once:
        public void Allow(string controller, ActionFlags af)
        {
            _actions[controller] = af;
        }

        // implicit ModelAction, based on HTTP verb:
        public bool Can()
        {
            // ties access to new/edit forms to Create/Update privileges:
        	return _action.Equals("New") ? Can(ModelAction.Create)
        	       	: _action.Equals("Edit") ? Can(ModelAction.Update)
        	       	: Can(_verbaction[_httpverb]);
        }

        // explicit ModelAction:
        public bool Can(ModelAction ma)
        {
            return _actions.ContainsKey(_controller) && (_actions[_controller] & (ActionFlags)ma) > 0;
        }

        public void Disallow(string controller, ModelAction a) {
            if (_actions.ContainsKey(controller))
            {
                _actions[controller] = _actions[controller] ^ (ActionFlags)a;
            }
        }
    }

    // additional Permissions can be added as needed (e.g. based on controller action, impersonation, etc.),
    // but Can() logic will need to be modified if so.
    [Flags]
    public enum ActionFlags
    {
        Create = 1,
        Read = 2,
        Update = 4,
        Delete = 8
    }

    //// single-valued specific action:
    public enum ModelAction
    {
        Create = 1,
        Read = 2,
        Update = 4,
        Delete = 8
    }
}