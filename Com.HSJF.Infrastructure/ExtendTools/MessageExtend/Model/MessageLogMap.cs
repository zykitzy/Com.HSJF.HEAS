using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.ExtendTools.MessageExtend.Model
{
    public class MessageLogMap : EntityTypeConfiguration<MessageLog>
    {
        public MessageLogMap()
        {
            this.HasKey(t => t.ID);
            this.Property(t => t.Subject).HasMaxLength(100);
            this.Property(t => t.SYSCode).HasMaxLength(50);

            this.ToTable("MessageLog");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.Recipient).HasColumnName("Recipient");
            this.Property(t => t.Sender).HasColumnName("Sender");
            this.Property(t => t.SendTime).HasColumnName("SendTime");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.SYSCode).HasColumnName("SYSCode");
        }
    }
}
