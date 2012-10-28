using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using OrgNavWebApi.Models.Mapping;

namespace OrgNavWebApi.Models
{
    public class OrganizationNavigatorContext : DbContext
    {
        static OrganizationNavigatorContext()
        {
            Database.SetInitializer<OrganizationNavigatorContext>(null);
        }

		public OrganizationNavigatorContext()
			: base("Name=OrganizationNavigatorContext")
		{
		}

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<OrgStructureSAP> OrgStructureSAPs { get; set; }
        public DbSet<OrgUnit> OrgUnits { get; set; }
        public DbSet<OrgUnitCT> OrgUnitCTS { get; set; }
        public DbSet<OrgUnitNode> OrgUnitNodes { get; set; }
        public DbSet<OrgUnitNodeCT> OrgUnitNodeCTS { get; set; }
        public DbSet<PositionNodeDB> PositionNodes { get; set; }
        public DbSet<PositionNodeState> PositionNodeStates { get; set; }
        public DbSet<PositionRB> PositionRBS { get; set; }
        public DbSet<TblCostCenter> TblCostCenters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AppUserMap());
            modelBuilder.Configurations.Add(new OrgStructureSAPMap());
            modelBuilder.Configurations.Add(new OrgUnitMap());
            modelBuilder.Configurations.Add(new OrgUnitCTMap());
            modelBuilder.Configurations.Add(new OrgUnitNodeMap());
            modelBuilder.Configurations.Add(new OrgUnitNodeCTMap());
            modelBuilder.Configurations.Add(new PositionNodeMap());
            modelBuilder.Configurations.Add(new PositionNodeStateMap());
            modelBuilder.Configurations.Add(new PositionRBMap());
            modelBuilder.Configurations.Add(new TblCostCenterMap());
        }
    }
}
