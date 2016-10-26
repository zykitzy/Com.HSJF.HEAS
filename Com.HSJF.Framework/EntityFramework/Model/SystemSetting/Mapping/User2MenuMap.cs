using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Com.HSJF.Framework.EntityFramework.Model.SystemSetting;

namespace Com.HSJF.Framework.EntityFramework.Model.SystemSetting.Mapping
{
    public class User2MenuMap : EntityTypeConfiguration<User2Menu>
    {
        public User2MenuMap()
        {
            this.HasKey(t => t.ID);

            this.ToTable("User2Menu","sysset");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.MenuID).HasColumnName("MenuID");



        }
        
    }
}
