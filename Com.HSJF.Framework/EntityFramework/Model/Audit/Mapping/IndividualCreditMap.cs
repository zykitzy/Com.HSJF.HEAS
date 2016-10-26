using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit.Mapping
{
    public class IndividualCreditMap : EntityTypeConfiguration<IndividualCredit>
    {
        public IndividualCreditMap()
        {
            this.HasKey(t => t.ID);
            this.Property(t => t.CreditCard).HasMaxLength(128);
            this.Property(t => t.BaseAuditID).HasMaxLength(128);
            this.Property(t => t.PersonID).HasMaxLength(128);


            this.ToTable("IndividualCredit", "audit");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.BaseAuditID).HasColumnName("BaseAuditID");
            this.Property(t => t.BankFlowFile).HasColumnName("BankFlow");
            this.Property(t => t.CreditCard).HasColumnName("CreditCard");
            this.Property(t => t.OtherCredit).HasColumnName("OtherCredit");
            this.Property(t => t.CreditInfo).HasColumnName("CreditInfo");
            this.Property(t => t.IndividualFile).HasColumnName("IndividualFile");
            this.Property(t => t.OverdueInfo).HasColumnName("OverdueInfo");
            this.Property(t => t.PersonID).HasColumnName("PersionID");
            this.Property(t => t.Sequence).HasColumnName("Sequence");


            this.HasRequired(t => t.BaseAudit).WithMany(t => t.IndividualCredits).HasForeignKey(t => t.BaseAuditID);
        }
    }
}
