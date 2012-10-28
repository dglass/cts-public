using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OrgNavWebApi.Models.Mapping
{
    public class AppUserMap : EntityTypeConfiguration<AppUser>
    {
        public AppUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.LanId)
                .HasMaxLength(255);

            this.Property(t => t.SidString)
                .HasMaxLength(255);

            this.Property(t => t.SidHexString)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("AppUser");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LanId).HasColumnName("LanId");
            this.Property(t => t.SidString).HasColumnName("SidString");
            this.Property(t => t.SidHexString).HasColumnName("SidHexString");
        }
    }
}
