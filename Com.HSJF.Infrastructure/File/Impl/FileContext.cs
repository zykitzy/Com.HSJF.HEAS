using Com.HSJF.Infrastructure.File.FileInterface;
using Com.HSJF.Infrastructure.File.FileInterface.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.File
{
    public class FileContext : DbContext
    {

        public FileContext() : base("name=FileContext")
        { }

        public DbSet<FileDescription> FileDescription { get; set; }
        public DbSet<FileStorage> FileStorage { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FileDescriptionMap());
            modelBuilder.Configurations.Add(new FileStorageMap());
        }
    }
}
