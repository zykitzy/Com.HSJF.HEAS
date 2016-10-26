using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.Others.Mapping
{
    public class RelationStateMap : EntityTypeConfiguration<RelationState>
    {
        public RelationStateMap()
        {
            this.HasKey(t=>t.ID);
            this.Property(t => t.CaseNum).HasMaxLength(128);
            this.Property(t=>t.RelationNumber).HasMaxLength(128);
            this.Property(t => t.SalesID).HasMaxLength(128);

            this.ToTable("RelationState","other");
            this.Property(t => t.CaseNum).HasColumnName("CaseNum");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Desc).HasColumnName("Desc");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.IsBinding).HasColumnName("IsBinding");
            this.Property(t => t.IsLock).HasColumnName("IsLock");
            this.Property(t => t.RelationNumber).HasColumnName("RelationNumber");
            this.Property(t => t.RelationType).HasColumnName("RelationType");
            this.Property(t => t.SalesID).HasColumnName("SalesID");
            this.Property(t => t.State).HasColumnName("State");
        }
    }
}
