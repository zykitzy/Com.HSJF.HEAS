using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit.Mapping
{
    public class HouseDetailMap : EntityTypeConfiguration<HouseDetail>
    {
        public HouseDetailMap()
        {
            this.HasKey(t => t.ID);
            this.Property(t => t.AssessedValue).HasPrecision(18, 6);
            this.Property(t => t.TotalHeight).HasPrecision(18, 6);
            this.Property(t => t.Accout).HasMaxLength(128);
            this.Property(t => t.CollateralID).HasMaxLength(128);
            this.Property(t => t.CompletionDate).HasMaxLength(4);

            this.ToTable("HouseDetail", "audit");

            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Accout).HasColumnName("Accout");
            this.Property(t => t.AssessedValue).HasColumnName("AssessedValue");
            this.Property(t => t.BaseAuditID).HasColumnName("BaseAuditID");
            this.Property(t => t.Collateral).HasColumnName("Collateral");
            this.Property(t => t.CompletionDate).HasColumnName("CompletionDate");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.HouseType).HasColumnName("HouseType");
            this.Property(t => t.LandType).HasColumnName("LandType");
            this.Property(t => t.LimitInfo).HasColumnName("LimitInfo");
            this.Property(t => t.RepairSituation).HasColumnName("RepairSituation");
            this.Property(t => t.ServiceCondition).HasColumnName("ServiceCondition");
            this.Property(t => t.TotalHeight).HasColumnName("TotalHeight");
            this.Property(t => t.CollateralID).HasColumnName("CollateralID");
            this.Property(t => t.HousePhotoFile).HasColumnName("HousePhotoFile");
            this.Property(t => t.HouseReportFile).HasColumnName("HouseReportFile");
            this.Property(t => t.Sequence).HasColumnName("Sequence");

            this.Property(t => t.RealHigh).HasColumnName("RealHigh");
            this.Property(t => t.IsDamage).HasColumnName("IsDamage");
            this.Property(t => t.RealResident).HasColumnName("RealResident");
            this.Property(t => t.WaterPaymentCheck).HasColumnName("WaterPaymentCheck");
            this.Property(t => t.TaxPaymentCheck).HasColumnName("TaxPaymentCheck");
            this.Property(t => t.Man2Wei1).HasColumnName("Man2Wei1");
            this.Property(t => t.SpecialResident).HasColumnName("SpecialResident");
            this.Property(t => t.OtherDescription).HasColumnName("OtherDescription");
            this.Property(t => t.SchoolInfo).HasColumnName("SchoolInfo");
            this.Property(t => t.HospitalInfo).HasColumnName("HospitalInfo");
            this.Property(t => t.TrafficInfo).HasColumnName("TrafficInfo");
            this.Property(t => t.Supermarket).HasColumnName("Supermarket");
            this.Property(t => t.Recreation).HasColumnName("Recreation");
            this.Property(t => t.NegativeSite).HasColumnName("NegativeSite");
            this.Property(t => t.VillagePhotoFile).HasColumnName("VillagePhotoFile");
            this.Property(t => t.MainGatePhotoFile).HasColumnName("MainGatePhotoFile");
            this.Property(t => t.ParlourPhotoFile).HasColumnName("ParlourPhotoFile");
            this.Property(t => t.BedroomPhotoFile).HasColumnName("BedroomPhotoFile");
            this.Property(t => t.KitchenRoomPhotoFile).HasColumnName("KitchenRoomPhotoFile");
            this.Property(t => t.ToiletPhotoFile).HasColumnName("ToiletPhotoFile");

            this.HasRequired(t => t.BaseAudit).WithMany(t => t.HouseDetails).HasForeignKey(t => t.BaseAuditID);
        }
    }
}
