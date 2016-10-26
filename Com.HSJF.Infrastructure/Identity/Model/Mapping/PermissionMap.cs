using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Identity.Model.Mapping
{
    public class PermissionMap : EntityTypeConfiguration<Permission>
    {
        public PermissionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(128);


            // Table & Column Mappings
            this.ToTable("Permissions","identity");
            this.Property(t => t.Id).HasColumnName("Id");

            this.Property(t => t.Name).HasColumnName("Name").HasMaxLength(255);
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.State).HasColumnName("State");
        }
    }
}
