using Com.HSJF.Framework.EntityFramework.Model.SystemSetting;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.SystemSetting.Mapping
{
    public class Menu2PermissionMap  : EntityTypeConfiguration<Menu2Permission>
    {
        public Menu2PermissionMap()
        {
            this.HasKey(t=>t.ID);

            this.ToTable("Menu2PermissionMap");
            this.Property(t => t.ID).HasColumnName("ID").HasMaxLength(128).IsRequired();
            this.Property(t => t.MenuID).HasColumnName("MenuID").HasMaxLength(128).IsRequired();
            this.Property(t => t.PermissionID).HasColumnName("PermissionID").HasMaxLength(128).IsRequired();


        }
    }
}
