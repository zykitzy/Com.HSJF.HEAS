using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.Sales.Mapping
{
    public class DistrictMap : EntityTypeConfiguration<District>
    {
        public DistrictMap()
        {
            this.HasKey(t=>t.ID);
            this.Property(t => t.Name).IsRequired().HasMaxLength(255);
            this.Property(t => t.ShortNumber).IsRequired().HasMaxLength(18);

            this.ToTable("District","sysset");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ShortNumber).HasColumnName("ShortNumber");

        }
    }
}
