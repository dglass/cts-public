using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OrgNavWebApi.Models.Mapping
{
    public class TblCostCenterMap : EntityTypeConfiguration<TblCostCenter>
    {
        public TblCostCenterMap()
        {
            // Primary Key
            this.HasKey(t => new { t.FYr, t.Division, t.RespCntr, t.BsnCntr, t.CstCntr });

            // Properties
            this.Property(t => t.FYr)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            this.Property(t => t.Division)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.RespCntr)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.BsnCntr)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.CstCntr)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            this.Property(t => t.DscrptnNameShort)
                .HasMaxLength(20);

            this.Property(t => t.DscrptnNameLong)
                .HasMaxLength(40);

            this.Property(t => t.CCType)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.MgrLANId)
                .HasMaxLength(12);

            this.Property(t => t.BudgetContacts)
                .HasMaxLength(40);

            this.Property(t => t.Status)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.CreatorLANId)
                .HasMaxLength(12);

            this.Property(t => t.ModifierLANId)
                .HasMaxLength(12);

            // Table & Column Mappings
            this.ToTable("TblCostCenter");
            this.Property(t => t.FYr).HasColumnName("FYr");
            this.Property(t => t.Division).HasColumnName("Division");
            this.Property(t => t.RespCntr).HasColumnName("RespCntr");
            this.Property(t => t.BsnCntr).HasColumnName("BsnCntr");
            this.Property(t => t.CstCntr).HasColumnName("CstCntr");
            this.Property(t => t.DscrptnNameShort).HasColumnName("DscrptnNameShort");
            this.Property(t => t.DscrptnNameLong).HasColumnName("DscrptnNameLong");
            this.Property(t => t.CCType).HasColumnName("CCType");
            this.Property(t => t.MgrLANId).HasColumnName("MgrLANId");
            this.Property(t => t.PersonId).HasColumnName("PersonId");
            this.Property(t => t.BudgetContacts).HasColumnName("BudgetContacts");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CreatorLANId).HasColumnName("CreatorLANId");
            this.Property(t => t.CreatorDateTime).HasColumnName("CreatorDateTime");
            this.Property(t => t.ModifierLANId).HasColumnName("ModifierLANId");
            this.Property(t => t.ModifierDateTime).HasColumnName("ModifierDateTime");
        }
    }
}
