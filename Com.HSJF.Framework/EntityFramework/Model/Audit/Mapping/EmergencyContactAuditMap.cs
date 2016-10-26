using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit.Mapping
{
    public class EmergencyContactAuditMap : EntityTypeConfiguration<EmergencyContactAudit>
    {
        public EmergencyContactAuditMap()
        {
            this.HasKey(t => t.ID);
            this.Property(t => t.ID).IsRequired().HasMaxLength(128);
            this.Property(t => t.ContactNumber).HasMaxLength(128);
            this.Property(t => t.ContactType).HasMaxLength(128);
            this.Property(t => t.Name).HasMaxLength(128);
            this.Property(t => t.PersonID).HasMaxLength(128);

            this.ToTable("EmergencyContactAudit", "audit");
            this.Property(t => t.Name).HasColumnName("ID");
            this.Property(t => t.ContactNumber).HasColumnName("ContactNumber");
            this.Property(t => t.ContactType).HasColumnName("ContactType");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.Sequence).HasColumnName("Sequence");


            this.HasRequired(t => t.RelationPersonAudit).WithMany(t => t.EmergencyContactAudits).HasForeignKey(t => t.PersonID);
        }
    }
}
