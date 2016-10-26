using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.SystemSetting.Mapping
{
    public class Menu2RoleMap : EntityTypeConfiguration<Menu2Role>
    {
        public Menu2RoleMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("Menu2Role", "sysset");

            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.MenuID).HasColumnName("MenuID");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
        }
    }
}