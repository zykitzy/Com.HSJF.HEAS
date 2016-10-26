using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Biz.Mapping
{
    public class ContactMap : EntityTypeConfiguration<Contact>
    {
        public ContactMap()
        {
            this.HasKey(t => t.ID);
            this.Property(t => t.ContactType).HasMaxLength(128);
            this.Property(t => t.ContactNumber).HasMaxLength(128);
            this.Property(t => t.PersonID).IsRequired().HasMaxLength(128);

            this.ToTable("Contact", "biz");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ContactNumber).HasColumnName("ContactNumber");
            this.Property(t => t.ContactType).HasColumnName("ContactType");
            this.Property(t => t.IsDefault).HasColumnName("IsDefault");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.Sequence).HasColumnName("Sequence");
            

            this.HasRequired(t => t.RelationPerson).WithMany(t => t.Contacts).HasForeignKey(t => t.PersonID).WillCascadeOnDelete(true);
        }
    }
}
