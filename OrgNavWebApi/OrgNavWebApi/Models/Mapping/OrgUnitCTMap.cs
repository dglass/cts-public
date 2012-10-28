using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OrgNavWebApi.Models.Mapping
{
    public class OrgUnitCTMap : EntityTypeConfiguration<OrgUnitCT>
    {
        public OrgUnitCTMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Code)
                .HasMaxLength(5);

            this.Property(t => t.ShortName)
                .HasMaxLength(15);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.RbsCode)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("OrgUnitCTS");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.ShortName).HasColumnName("ShortName");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.RbsCode).HasColumnName("RbsCode");
            this.Property(t => t.HrmsOrgUnitId).HasColumnName("HrmsOrgUnitId");
        }
    }
}
