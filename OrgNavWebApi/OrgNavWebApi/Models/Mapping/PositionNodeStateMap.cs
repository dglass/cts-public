using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OrgNavWebApi.Models.Mapping
{
    public class PositionNodeStateMap : EntityTypeConfiguration<PositionNodeState>
    {
        public PositionNodeStateMap()
        {
            // Primary Key
            this.HasKey(t => new { t.PositionNodeId, t.AppUserId });

            // Properties
            this.Property(t => t.PositionNodeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AppUserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("PositionNodeState");
            this.Property(t => t.PositionNodeId).HasColumnName("PositionNodeId");
            this.Property(t => t.AppUserId).HasColumnName("AppUserId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            // Relationships
            this.HasRequired(t => t.AppUser)
                .WithMany(t => t.PositionNodeStates)
                .HasForeignKey(d => d.AppUserId);

        }
    }
}
