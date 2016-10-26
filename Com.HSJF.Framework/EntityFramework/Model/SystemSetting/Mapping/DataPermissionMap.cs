using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.SystemSetting.Mapping
{
    public class DataPermissionMap : EntityTypeConfiguration<DataPermission>
    {
        public DataPermissionMap()
        {
            this.HasKey(t => t.ID);

            this.Property(t => t.RoleID).IsRequired().HasMaxLength(128);

            this.ToTable("DataPermission", "sysset");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.DataPermissionID).HasColumnName("DataPermissionID");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
        }
    }
}
