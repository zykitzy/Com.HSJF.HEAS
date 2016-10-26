using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.SystemSetting.Mapping
{
    public class Role2MenuMap : EntityTypeConfiguration<Role2Menu>
    {
        public Role2MenuMap()
        {
            this.HasKey(t => t.ID);
            this.Property(t => t.DataPermissionID).HasMaxLength(128);
            this.Property(t => t.RoleID).HasMaxLength(128);

            this.ToTable("Role2Menu","sysset");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.DataPermissionID).HasColumnName("DataPermissionID");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
        }
    }
}
