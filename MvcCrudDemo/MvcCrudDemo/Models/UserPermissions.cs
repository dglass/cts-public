namespace MvcCrudDemo.Models
{
    public class UserPermissions
    {
        public UserPermissions()
        {
            // default to read-only:
            PermitRead = true;
            PermitUpdate = PermitDelete = PermitCreate = false;
        }
        // just for demoing effect of different permission settings...
        public bool PermitRead { get; set; }
        public bool PermitUpdate { get; set; }
        public bool PermitDelete { get; set; }
        public bool PermitCreate { get; set; }
    }
}