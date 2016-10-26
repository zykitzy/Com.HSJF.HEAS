using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Sales.Mapping
{
    public class SalesGroupMap : EntityTypeConfiguration<SalesGroup>
    {
        public SalesGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Name)
                .HasMaxLength(128);

            this.Property(t => t.Company)
                .HasMaxLength(255);

            this.Property(t => t.CompanyCode)
                .HasMaxLength(128);

            this.Property(t => t.DistrictID)
               .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("SalesGroup", "sysset");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Company).HasColumnName("Company");
            this.Property(t => t.CompanyCode).HasColumnName("CompanyCode");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.DistrictID).HasColumnName("DistrictID");

            this.HasRequired(t => t.District).WithMany(t => t.SalesGroup).HasForeignKey(t => t.DistrictID);
        }
    }
}
