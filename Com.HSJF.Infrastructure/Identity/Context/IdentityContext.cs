using Com.HSJF.Infrastructure.Identity.Model.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Identity.Context
{
    public class IdentityContext : DbContext
    {
        public IdentityContext() : base("name=IdentityContext")
        { }

        public DbSet<Com.HSJF.Infrastructure.Identity.Model.User> User { get; set; }
        public DbSet<Com.HSJF.Infrastructure.Identity.Model.Permission> Permission { get; set; }
        public DbSet<Com.HSJF.Infrastructure.Identity.Model.Role> Role { get; set; }
        public DbSet<Com.HSJF.Infrastructure.Identity.Model.UserRole> UserRole { get; set; }
        public DbSet<Com.HSJF.Infrastructure.Identity.Model.RolePermission> RolePermission { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PermissionMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new RolePermissionMap());
            modelBuilder.Configurations.Add(new UserRoleMap());
        }



    }
}
