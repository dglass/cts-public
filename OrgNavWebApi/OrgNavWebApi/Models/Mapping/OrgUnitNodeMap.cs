using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OrgNavWebApi.Models.Mapping
{
    public class OrgUnitNodeMap : EntityTypeConfiguration<OrgUnitNode>
    {
        public OrgUnitNodeMap()
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
            this.ToTable("OrgUnitNode");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.Ancestry).HasColumnName("Ancestry");

            // Relationships
            this.HasOptional(t => t.OrgUnitNode2)
                .WithMany(t => t.OrgUnitNode1)
                .HasForeignKey(d => d.ParentId);

        }
    }
}
