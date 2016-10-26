using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit.Mapping
{
    public class EnterpriseCreditMap : EntityTypeConfiguration<EnterpriseCredit>
    {
        public EnterpriseCreditMap()
        {
            this.HasKey(t => t.ID);
            this.Property(t => t.CreditCard).HasMaxLength(128);
            this.Property(t => t.BaseAuditID).HasMaxLength(128);
            this.Property(t => t.EnterpriseID).HasMaxLength(128);

            this.ToTable("EnterpriseCredit", "audit");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.BaseAuditID).HasColumnName("BaseAuditID");
            this.Property(t => t.CreditCard).HasColumnName("CreditCard");
            this.Property(t => t.CreditInfo).HasColumnName("CreditInfo");
            this.Property(t => t.EnterpriseID).HasColumnName("EnterpriseID");
            this.Property(t => t.ShareholderDetails).HasColumnName("ShareholderDetails");
            this.Property(t => t.OtherDetailes).HasColumnName("OtherDetailes");
            this.Property(t => t.Sequence).HasColumnName("Sequence");


            this.HasRequired(t => t.BaseAudit).WithMany(t => t.EnterpriseCredits).HasForeignKey(t => t.BaseAuditID);
        }
    }
}
