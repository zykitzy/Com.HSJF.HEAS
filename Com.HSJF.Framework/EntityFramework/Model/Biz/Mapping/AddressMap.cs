using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Biz.Mapping
{
    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            this.HasKey(t => t.ID);

            this.Property(t => t.ID).IsRequired().HasMaxLength(128);
            this.Property(t => t.AddressType).HasMaxLength(128);
            this.Property(t => t.PersonID).IsRequired().HasMaxLength(128);

            this.ToTable("Address", "biz");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.AddressInfo).HasColumnName("AddressInfo");
            this.Property(t => t.AddressType).HasColumnName("AddressType");
            this.Property(t => t.IsDefault).HasColumnName("IsDefault");
            this.Property(t => t.Sequence).HasColumnName("Sequence");
            
            this.HasRequired(t => t.RelationPerson)
                .WithMany(t => t.Addresses).HasForeignKey(t => t.PersonID).WillCascadeOnDelete(true);
        }
    }
}
