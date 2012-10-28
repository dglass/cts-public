using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OrgNavWebApi.Models.Mapping
{
    public class PositionRBMap : EntityTypeConfiguration<PositionRB>
    {
        public PositionRBMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RbsCode)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("PositionRBS");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OrgUnitId).HasColumnName("OrgUnitId");
            this.Property(t => t.RbsCode).HasColumnName("RbsCode");

            // Relationships
            this.HasOptional(t => t.OrgUnit)
                .WithMany(t => t.PositionRBS)
                .HasForeignKey(d => d.OrgUnitId);

        }
    }
}
