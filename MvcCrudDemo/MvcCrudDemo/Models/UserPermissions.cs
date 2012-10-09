namespace MvcCrudDemo.Models
{
    public class UserPermissions : CachedResource
    {
        public UserPermissions()
        {
            // default to read + update since we need update to update UserPermissions themselves.
            // TODO: revert this to PermitRead only, but apply authorize attribute to separate CachedResource subclass.
            PermitRead = true;
//            PermitCreate = PermitUpdate = PermitDelete = false;
            PermitCreate = PermitUpdate = PermitDelete = true;
        }
        // just for demoing effect of different permission settings...
        public bool PermitRead { get; set; }
        public bool PermitUpdate { get; set; }
        public bool PermitDelete { get; set; }
        public bool PermitCreate { get; set; }
    }
}