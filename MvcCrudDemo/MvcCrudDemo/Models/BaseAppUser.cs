using MvcCrudDemo.Filters;

namespace MvcCrudDemo.Models
{
    public class BaseAppUser
    {
        private BaseAuthorizationProvider _bap; // potential dependency for DI

        public BaseAppUser(BaseAuthorizationProvider bap)
        {
            _bap = bap;
        }

        public bool Can()
        {
            return _bap.Can();
        }

    }

}