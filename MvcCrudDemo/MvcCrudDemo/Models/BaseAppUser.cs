using MvcCrudDemo.Filters;

namespace MvcCrudDemo.Models
{
    public class BaseAppUser
    {
        protected BaseAuthorizationProvider Bap; // potential dependency for DI

        public BaseAppUser(BaseAuthorizationProvider bap)
        {
            Bap = bap;
        }

        public bool Can()
        {
            return Bap.Can();
        }

    }

}