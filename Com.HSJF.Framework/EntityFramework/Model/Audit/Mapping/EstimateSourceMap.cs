using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit.Mapping
{
    public class EstimateSourceMap : EntityTypeConfiguration<EstimateSource>
    {
        public EstimateSourceMap()
        {
            this.HasKey(t => t.ID);
            this.Property(t => t.RushEstimate).HasPrecision(18, 6);

            this.ToTable("EstimateSource", "audit");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ContactNumber).HasColumnName("ContactNumber");
            this.Property(t => t.EstimateInstitutions).HasColumnName("EstimateInstitutions");
            this.Property(t => t.InformationProvider).HasColumnName("InformationProvider");
            this.Property(t => t.RushEstimate).HasColumnName("RushEstimate");
            this.Property(t => t.Sequence).HasColumnName("Sequence");
            this.Property(t => t.CertificateFile).HasColumnName("CertificateFile");
            
            this.HasRequired(t => t.HouseDetail).WithMany(t => t.EstimateSources).HasForeignKey(t=>t.HouseDetailID);
        }

    }
}
