using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Identity.Model.Mapping
{
    public class UserRoleMap : EntityTypeConfiguration<UserRole>
    {
        public UserRoleMap()
        {
            this.HasKey(t => t.ID);
            this.Property(t => t.RoleID).IsRequired().HasMaxLength(128);
            this.Property(t => t.UserID).IsRequired().HasMaxLength(128);

            this.ToTable("User2Role", "identity");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.UserID).HasColumnName("UserID");

            this.HasRequired(t => t.User).WithMany(t => t.UserRole).HasForeignKey(t => t.UserID);
            this.HasRequired(t => t.Role).WithMany(t => t.UserRole).HasForeignKey(t => t.RoleID);
        }
    }
}
