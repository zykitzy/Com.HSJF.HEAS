using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.Others.Mapping
{
    public class MigTMapping : EntityTypeConfiguration<MigT>
    {
        public MigTMapping()
        {
            this.HasKey(t => t.ID);
            this.ToTable("MigT");
        }
    }
}
