using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit.Mapping
{
    public class RelationPersonAuditMap : EntityTypeConfiguration<RelationPersonAudit>
    {
        public RelationPersonAuditMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.AuditID)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.RelationType)
                .HasMaxLength(50);

            this.Property(t => t.BorrowerRelation)
                .HasMaxLength(50);

            this.Property(t => t.Name)
                .HasMaxLength(128);

            this.Property(t => t.IdentificationType)
                .HasMaxLength(50);

            this.Property(t => t.IdentificationNumber)
                .HasMaxLength(128);


            // Table & Column Mappings
            this.ToTable("RelationPersonAudit", "audit");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.AuditID).HasColumnName("AuditID");
            this.Property(t => t.RelationType).HasColumnName("RelationType");
            this.Property(t => t.BorrowerRelation).HasColumnName("BorrowerRelation");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.IdentificationType).HasColumnName("IdentificationType");
            this.Property(t => t.IdentificationNumber).HasColumnName("IdentificationNumber");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.IdentificationFile).HasColumnName("IdentificationFile");
            this.Property(t => t.MaritalStatus).HasColumnName("MaritalStatus");
            this.Property(t => t.MarryFile).HasColumnName("MarryFile");
            this.Property(t => t.SingleFile).HasColumnName("SingleFile");
            this.Property(t => t.BirthFile).HasColumnName("BirthFile");
            this.Property(t => t.AccountFile).HasColumnName("AccountFile");
            this.Property(t => t.SalaryPersonFile).HasColumnName("SalaryPersonFile");
            this.Property(t => t.SelfLicenseFile).HasColumnName("SelfLicenseFile");
            this.Property(t => t.SelfNonLicenseFile).HasColumnName("SelfNonLicenseFile");
            this.Property(t => t.SalaryDescription).HasColumnName("SalaryDescription");
            this.Property(t => t.IsCoBorrower).HasColumnName("IsCoBorrower");
            this.Property(t => t.Warranty).HasColumnName("Warranty");
            this.Property(t => t.IndividualFile).HasColumnName("IndividualFile");
            this.Property(t => t.BankFlowFile).HasColumnName("BankFlowFile");
            this.Property(t => t.OtherFile).HasColumnName("OtherFile");
            this.Property(t => t.Sequence).HasColumnName("Sequence");


            // Relationships
            this.HasRequired(t => t.BaseAudit)
                .WithMany(t => t.RelationPersonAudits)
                .HasForeignKey(d => d.AuditID);

        }
    }
}
