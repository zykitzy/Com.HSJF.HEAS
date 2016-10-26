using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Identity.Model.Mapping
{
    public class RolePermissionMap :EntityTypeConfiguration<RolePermission>
    {
        public RolePermissionMap()
        {
            this.HasKey(t=>t.ID);

            this.Property(t => t.PermissionID).IsRequired().HasMaxLength(128);
            this.Property(t => t.RoleID).IsRequired().HasMaxLength(128);

            this.ToTable("Role2Permission", "identity");
            this.Property(t => t.PermissionID).HasColumnName("PermissionID");
            this.Property(t => t.RoleID).HasColumnName("RoleID");

            this.HasRequired(t => t.Role).WithMany(t => t.RolePermission).HasForeignKey(t => t.RoleID);
            this.HasRequired(t => t.Permission).WithMany(t => t.RolePermission).HasForeignKey(t => t.PermissionID);
        }
    }
}
