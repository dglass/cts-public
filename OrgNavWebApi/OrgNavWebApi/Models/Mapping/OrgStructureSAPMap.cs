using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OrgNavWebApi.Models.Mapping
{
    public class OrgStructureSAPMap : EntityTypeConfiguration<OrgStructureSAP>
    {
        public OrgStructureSAPMap()
        {
            // Primary Key
            this.HasKey(t => t.OrgUnitId);

            // Properties
            this.Property(t => t.OrgUnitId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("OrgStructureSAP");
            this.Property(t => t.OrgUnitId).HasColumnName("OrgUnitId");
            this.Property(t => t.ParentOrgUnitId).HasColumnName("ParentOrgUnitId");
        }
    }
}
