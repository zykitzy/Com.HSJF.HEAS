using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.File.FileInterface.Mapping
{
    public class FileDescriptionMap : EntityTypeConfiguration<FileDescription>
    {
        public FileDescriptionMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("FileDescriptions");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.FileCreateTime).HasColumnName("FileCreateTime");
            this.Property(t => t.FileData).HasColumnName("FileData");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.FileSaveName).HasColumnName("FileSaveName");
            this.Property(t => t.FileState).HasColumnName("FileState");
            this.Property(t => t.FileType).HasColumnName("FileType");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.LinkID).HasColumnName("LinkID");
            this.Property(t => t.LinkKey).HasColumnName("LinkKey");
            this.Property(t => t.FileStorageID).HasColumnName("FileStorageID");

        }
    }
}
