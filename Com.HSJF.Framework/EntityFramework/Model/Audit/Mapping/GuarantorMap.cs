using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit.Mapping
{
    public class GuarantorMap : EntityTypeConfiguration<Guarantor>
    {
        public GuarantorMap()
        {
            this.HasKey(t => t.ID);
            this.Property(t => t.Name).HasMaxLength(128);
            this.Property(t => t.BaseAuditID).HasMaxLength(128);
            this.Property(t => t.ContactNumber).HasMaxLength(128);
            this.Property(t => t.GuarantType).HasMaxLength(128);
            this.Property(t => t.IdentityNumber).HasMaxLength(128);
            this.Property(t => t.IdentityType).HasMaxLength(128);
            this.Property(t => t.MarriedInfo).HasMaxLength(255);
            this.Property(t => t.RelationType).HasMaxLength(128);

            this.ToTable("Guarantor", "audit");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.ContactNumber).HasColumnName("ContactNumber");
            this.Property(t => t.GuarantType).HasColumnName("GuarantType");
            this.Property(t => t.IdentityNumber).HasColumnName("IdentityNumber");
            this.Property(t => t.IdentityType).HasColumnName("IdentityType");
            this.Property(t => t.MarriedInfo).HasColumnName("MarriedInfo");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.RelationType).HasColumnName("RelationType");
            this.Property(t => t.Sequence).HasColumnName("Sequence");


            this.HasRequired(t => t.BaseAudit).WithMany(t => t.Guarantors).HasForeignKey(t => t.BaseAuditID);
        }
    }
}
