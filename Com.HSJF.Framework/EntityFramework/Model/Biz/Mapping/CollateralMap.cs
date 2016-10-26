using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Biz.Mapping
{
    public class CollateralMap : EntityTypeConfiguration<Collateral>
    {
        public CollateralMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.CaseID)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.CollateralType)
                .HasMaxLength(50);

            this.Property(t => t.HouseNumber)
                .HasMaxLength(128);

            this.Property(t => t.BuildingName)
                .HasMaxLength(128);

            this.Property(t => t.RightOwner)
                .HasMaxLength(128);
            this.Property(t => t.CompletionDate).HasMaxLength(4);

            // Table & Column Mappings
            this.ToTable("Collateral", "biz");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CaseID).HasColumnName("CaseID");
            this.Property(t => t.CollateralType).HasColumnName("CollateralType");
            this.Property(t => t.HouseNumber).HasColumnName("HouseNumber");
            this.Property(t => t.HouseFile).HasColumnName("HouseFile");
            this.Property(t => t.BuildingName).HasColumnName("BuildingName");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.RightOwner).HasColumnName("RightOwner");
            this.Property(t => t.HouseSize).HasColumnName("HouseSize");
            this.Property(t => t.Sequence).HasColumnName("Sequence");
            this.Property(t => t.CompletionDate).HasColumnName("CompletionDate");
            this.Property(t => t.LandType).HasColumnName("LandType");
            this.Property(t => t.HouseType).HasColumnName("HouseType");
            this.Property(t => t.TotalHeight).HasColumnName("TotalHeight");
            this.Property(t => t.IsLocked).HasColumnName("IsLocked");

            // Relationships
            this.HasRequired(t => t.BaseCase)
                .WithMany(t => t.Collaterals)
                .HasForeignKey(d => d.CaseID)
                .WillCascadeOnDelete(true);
        }
    }
}
