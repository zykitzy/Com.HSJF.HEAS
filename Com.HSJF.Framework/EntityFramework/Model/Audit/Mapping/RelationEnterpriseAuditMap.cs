using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit.Mapping
{
    public class RelationEnterpriseAuditMap : EntityTypeConfiguration<RelationEnterpriseAudit>
    {
        public RelationEnterpriseAuditMap()
        {


            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.PersonID)
                .HasMaxLength(128);

            this.Property(t => t.EnterpriseName)
                .HasMaxLength(128);

            this.Property(t => t.RegisterNumber)
                .HasMaxLength(128);

            this.Property(t => t.LegalPerson)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("RelationEnterpriseAudit", "audit");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.EnterpriseDes).HasColumnName("EnterpriseDes");
            this.Property(t => t.EnterpriseName).HasColumnName("EnterpriseName");
            this.Property(t => t.RegisterNumber).HasColumnName("RegisterNumber");
            this.Property(t => t.LegalPerson).HasColumnName("LegalPerson");
            this.Property(t => t.ShareholderDetails).HasColumnName("ShareholderDetails");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.RegisteredCapital).HasColumnName("RegisteredCapital");
            this.Property(t => t.MainBusiness).HasColumnName("MainBusiness");
            this.Property(t => t.IndividualFile).HasColumnName("IndividualFile");
            this.Property(t => t.BankFlowFile).HasColumnName("BankFlowFile");
            this.Property(t => t.Sequence).HasColumnName("Sequence");


            // Relationships
            this.HasOptional(t => t.RelationPersonAudit)
                .WithMany(t => t.RelationEnterpriseAudits)
                .HasForeignKey(d => d.PersonID);
        }
    }
}
