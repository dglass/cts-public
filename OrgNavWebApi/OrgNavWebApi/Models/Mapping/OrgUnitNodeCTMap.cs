using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OrgNavWebApi.Models.Mapping
{
    public class OrgUnitNodeCTMap : EntityTypeConfiguration<OrgUnitNodeCT>
    {
        public OrgUnitNodeCTMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Ancestry)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("OrgUnitNodeCTS");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.Ancestry).HasColumnName("Ancestry");

            // Relationships
            this.HasRequired(t => t.OrgUnitCT)
                .WithOptional(t => t.OrgUnitNodeCT);
            this.HasOptional(t => t.OrgUnitNodeCT1)
                .WithMany(t => t.OrgUnitNodeCTS1)
                .HasForeignKey(d => d.ParentId);

        }
    }
}
