using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.Lending.Mapping
{
    public class LendMap : EntityTypeConfiguration<Lending>
    {
        public LendMap()
        {
            this.HasKey(t => t.ID);

            this.Property(t => t.ID).HasMaxLength(128);
            this.Property(t => t.ContractAmount).HasPrecision(18, 6);

            this.ToTable("Lending", "after");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.LendFile).HasColumnName("LendFile");
            this.Property(t => t.LendTime).HasColumnName("LendTime");

            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.Borrower).HasColumnName("Borrower");
            this.Property(t => t.BorrowNumber).HasColumnName("BorrowNumber");
            this.Property(t => t.ContactNumber).HasColumnName("ContactNumber");
            this.Property(t => t.ContractAmount).HasColumnName("ContractAmount");
            this.Property(t => t.OpeningBank).HasColumnName("OpeningBank");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.PaymentDay).HasColumnName("PaymentDay");

            this.HasRequired(t => t.BaseAudit).WithRequiredDependent(t => t.Lending);
        }
    }
}
