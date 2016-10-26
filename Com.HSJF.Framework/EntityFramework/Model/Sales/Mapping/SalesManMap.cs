using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Sales.Mapping
{
    public class SalesManMap : EntityTypeConfiguration<SalesMan>
    {
        public SalesManMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.SalesID)
                .HasMaxLength(128);

            this.Property(t => t.Post)
                .HasMaxLength(128);

            this.Property(t => t.GroupID)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("SalesMan","sysset");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.SalesID).HasColumnName("SalesID");
            this.Property(t => t.Post).HasColumnName("Post");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.GroupID).HasColumnName("GroupID");

            // Relationships
            this.HasRequired(t => t.SalesGroup)
                .WithMany(t => t.SalesMen)
                .HasForeignKey(d => d.GroupID);

        }
    }
}
