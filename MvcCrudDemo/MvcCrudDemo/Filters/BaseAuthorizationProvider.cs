using MvcCrudDemo.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MvcCrudDemo.Filters
{
    public class BaseAuthorizationProvider
    {
    	protected readonly Dictionary<string, ActionFlags> Actions = new Dictionary<string, ActionFlags>();
		protected readonly HttpContextBase Context;
		protected readonly HttpVerbs Httpverb;
		protected readonly string Action;
		protected readonly string Controller;
        //private readonly string _id; // specific record id, if provided
		protected readonly Dictionary<HttpVerbs, ModelAction> VerbAction = new Dictionary<HttpVerbs, ModelAction> {
            { HttpVerbs.Post, ModelAction.Create },
            { HttpVerbs.Get, ModelAction.Read },
            { HttpVerbs.Put, ModelAction.Update },
            { HttpVerbs.Delete, ModelAction.Delete }
        };

    	public BaseAuthorizationProvider(HttpContextBase hcb)
        {
            Context = hcb;
            var rdv = ((MvcHandler)Context.CurrentHandler).RequestContext.RouteData.Values;
            Httpverb = (HttpVerbs)(Enum.Parse(typeof(HttpVerbs), Context.Request.GetHttpMethodOverride(), true)); // returns actual method if no override specified.
            // assumes specific Resources will be accessed by Id (see BaseResourceModel)
            // _id = rdv.ContainsKey("Id") ? rdv["Id"].ToString() : "";
            Controller = rdv["controller"].ToString();
            Action = rdv["action"].ToString();
            //var n = _context.User.Identity.Name;
            Init();
        }

        private void Init()
        {
            // initialize permissions from cached UserPermissions (test purposes only)...
            // NOTE, this must occur *BEFORE* any Controller actions.
            Context.Cache["UserPermissions"] = Context.Cache["UserPermissions"] ?? new UserPermissions();
            var up = (UserPermissions)Context.Cache["UserPermissions"];
            var af = new ActionFlags();
            af = up.PermitCreate ? af | ActionFlags.Create : af;
            af = up.PermitRead ? af | ActionFlags.Read : af;
            af = up.PermitUpdate ? af | ActionFlags.Update : af;
            af = up.PermitDelete ? af | ActionFlags.Delete : af;
            Allow(Controller, af);
        }

        // set individual flag:
        public void Allow(string controller, ModelAction a) {
            Actions[controller] = Actions.ContainsKey(controller) ? Actions[controller] | (ActionFlags)a : (ActionFlags)a;
        }

        // sets all flags at once:
        public void Allow(string controller, ActionFlags af)
        {
            Actions[controller] = af;
        }

        // implicit ModelAction, based on HTTP verb:
        public bool Can()
        {
            // ties access to new/edit forms to Create/Update privileges:
        	return Action.Equals("New") ? Can(ModelAction.Create)
        	       	: Action.Equals("Edit") ? Can(ModelAction.Update)
        	       	: Can(VerbAction[Httpverb]);
        }

        // explicit ModelAction:
        public bool Can(ModelAction ma)
        {
            return Actions.ContainsKey(Controller) && (Actions[Controller] & (ActionFlags)ma) > 0;
        }

        public void Disallow(string controller, ModelAction a) {
            if (Actions.ContainsKey(controller))
            {
                Actions[controller] = Actions[controller] ^ (ActionFlags)a;
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