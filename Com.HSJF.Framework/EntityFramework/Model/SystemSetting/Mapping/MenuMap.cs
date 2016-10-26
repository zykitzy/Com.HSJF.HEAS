using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.SystemSetting.Mapping
{
    public class MenuMap : EntityTypeConfiguration<Menu>
    {
        public MenuMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Table & Column Mappings
            this.ToTable("Menu", "sysset");
            this.Property(t => t.ID).HasColumnName("ID").HasMaxLength(128);
            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(128);
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.ParentID).HasColumnName("ParentID");
            this.Property(t => t.Icon).HasColumnName("Icon").HasMaxLength(255);

            this.HasOptional(t => t.ParentMenu).WithMany(i => i.ChildMenu).HasForeignKey(o => o.ParentID);
        }
    }
}