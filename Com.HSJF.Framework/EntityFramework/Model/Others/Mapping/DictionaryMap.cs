using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.Others.Mapping
{
    public class DictionaryMap : EntityTypeConfiguration<Dictionary>
    {
        public DictionaryMap()
        {
            // Primary Key
            this.HasKey(t => t.Path);

            // Properties
            this.Property(t => t.Path)
                .IsRequired();

            this.Property(t => t.Key)
                .HasMaxLength(128);

            this.Property(t => t.Text)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.ParentKey)
                .HasMaxLength(255);


            // Table & Column Mappings
            this.ToTable("Dictionary","other");
            this.Property(t => t.Path).HasColumnName("Path");
            this.Property(t => t.Key).HasColumnName("Key");
            this.Property(t => t.Text).HasColumnName("Text");
            this.Property(t => t.ParentKey).HasColumnName("ParentKey");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.Desc).HasColumnName("Desc");

        }
    }
}
