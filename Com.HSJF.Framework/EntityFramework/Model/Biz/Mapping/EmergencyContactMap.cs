using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Biz.Mapping
{
    public class EmergencyContactMap : EntityTypeConfiguration<EmergencyContact>
    {
        public EmergencyContactMap()
        {
            this.HasKey(t => t.ID);
            this.Property(t => t.ID).IsRequired().HasMaxLength(128);
            this.Property(t => t.ContactNumber).HasMaxLength(128);
            this.Property(t => t.ContactType).HasMaxLength(128);
            this.Property(t => t.Name).HasMaxLength(128);
            this.Property(t => t.PersonID).HasMaxLength(128);

            this.ToTable("EmergencyContact", "biz");
            this.Property(t => t.Name).HasColumnName("ID");
            this.Property(t => t.ContactNumber).HasColumnName("ContactNumber");
            this.Property(t => t.ContactType).HasColumnName("ContactType");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.Sequence).HasColumnName("Sequence");
            

            this.HasRequired(t => t.RelationPerson).WithMany(t => t.EmergencyContacts).HasForeignKey(t => t.PersonID).WillCascadeOnDelete(true);


        }
    }
}
