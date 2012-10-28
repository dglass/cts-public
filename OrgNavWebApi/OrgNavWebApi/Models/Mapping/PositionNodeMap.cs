using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OrgNavWebApi.Models.Mapping
{
    public class PositionNodeMap : EntityTypeConfiguration<PositionNodeDB>
    {
        public PositionNodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Ancestry)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PositionNode");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SuperId).HasColumnName("SuperId");
            this.Property(t => t.Ancestry).HasColumnName("Ancestry");

            // Relationships
            this.HasOptional(t => t.PositionNode2)
                .WithMany(t => t.PositionNode1)
                .HasForeignKey(d => d.SuperId);

        }
    }
}
