using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit.Mapping
{
    public class ContactAuditMap : EntityTypeConfiguration<ContactAudit>
    {
        public ContactAuditMap()
        {
            this.HasKey(t => t.ID);
            this.Property(t => t.ContactType).HasMaxLength(128);
            this.Property(t => t.ContactNumber).HasMaxLength(128);
            this.Property(t => t.PersonID).HasMaxLength(128);

            this.ToTable("ContactAudit", "audit");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ContactNumber).HasColumnName("ContactNumber");
            this.Property(t => t.ContactType).HasColumnName("ContactType");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.IsDefault).HasColumnName("IsDefault");
            this.Property(t => t.Sequence).HasColumnName("Sequence");

            this.HasRequired(t => t.RelationPersonAudit).WithMany(t => t.ContactAudits).HasForeignKey(t => t.PersonID);
        }
    }
}
