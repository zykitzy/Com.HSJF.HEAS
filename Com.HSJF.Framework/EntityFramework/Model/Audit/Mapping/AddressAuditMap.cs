using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit.Mapping
{
    public class AddressAuditMap : EntityTypeConfiguration<AddressAudit>
    {
        public AddressAuditMap()
        {
            this.HasKey(t => t.ID);

            this.Property(t => t.ID).IsRequired().HasMaxLength(128);
            this.Property(t => t.AddressType).HasMaxLength(128);
            this.Property(t => t.PersonID).HasMaxLength(128);

            this.ToTable("AddressAudit", "audit");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.AddressInfo).HasColumnName("AddressInfo");
            this.Property(t => t.AddressType).HasColumnName("AddressType");
            this.Property(t => t.IsDefault).HasColumnName("IsDefault");
            this.Property(t => t.Sequence).HasColumnName("Sequence");
            this.HasRequired(t => t.RelationPersonAudit).WithMany(t => t.AddressAudits).HasForeignKey(t => t.PersonID);
        }
    }
}
