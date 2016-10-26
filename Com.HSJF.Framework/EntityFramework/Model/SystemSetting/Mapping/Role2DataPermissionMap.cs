using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.SystemSetting.Mapping
{
    public class Role2DataPermissionMap : EntityTypeConfiguration<Role2DataPermission>
    {
        public Role2DataPermissionMap()
        {
            this.HasKey(t => t.ID);
            this.Property(t => t.DataPermissionID).HasMaxLength(128);
            this.Property(t => t.RoleID).HasMaxLength(128);

            this.ToTable("Role2DataPermission", "sysset");
            this.Property(t => t.DataPermissionID).HasColumnName("DataPermissionID");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
        }
    }
}
