using Com.HSJF.Infrastructure.ExtendTools.MessageExtend.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.ExtendTools.MessageExtend
{
    public class MessageContext : DbContext
    {
        public MessageContext() : base("name=MessageContext")
        {
        }
        public DbSet<MessageLog> MessageLog { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MessageLogMap());
        }
    }
}
