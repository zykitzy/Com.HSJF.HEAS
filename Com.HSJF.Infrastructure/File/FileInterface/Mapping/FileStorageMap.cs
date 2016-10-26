using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.File.FileInterface.Mapping
{
    public class FileStorageMap : EntityTypeConfiguration<FileStorage>
    {
        public FileStorageMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("FileStorages");
            this.Property(t => t.FileData).HasColumnName("FileData");
            this.Property(t => t.FileUrl).HasColumnName("FileUrl");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.MD5).HasColumnName("MD5");
            this.Property(t => t.StoreType).HasColumnName("StoreType");
        }
    }
}
