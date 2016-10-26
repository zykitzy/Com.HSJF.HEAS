using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.Mortgage.Mapping
{
    public class PublicMortgageMap : EntityTypeConfiguration<PublicMortgage>
    {
        public PublicMortgageMap()
        {
            this.HasKey(t => t.ID);

            this.Property(t => t.ID).HasMaxLength(128);
            this.Property(t => t.ContractAmount).HasPrecision(18, 6);


            this.ToTable("PublicMortgage", "after");
          
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ContractFile).HasColumnName("ContractFile");
            this.Property(t => t.NoteFile).HasColumnName("NoteFile");
            this.Property(t => t.OtherFile).HasColumnName("OtherFile");
            this.Property(t => t.ReceiptFile).HasColumnName("ReceiptFile");
            this.Property(t => t.ContractAmount).HasColumnName("ContractAmount");
            this.Property(t => t.ContractDate).HasColumnName("ContractDate");
            this.Property(t => t.ContractFile).HasColumnName("ContractFile");
            this.Property(t => t.ContractNo).HasColumnName("ContractNo");
            this.Property(t => t.ContractPerson).HasColumnName("ContractPerson");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UndertakingFile).HasColumnName("Undertaking");
            this.Property(t => t.RepaymentAttorneyFile).HasColumnName("RepaymentAttorney");
            this.Property(t => t.ContactConfirmFile).HasColumnName("ContactConfirm");
            this.Property(t => t.PowerAttorneyFile).HasColumnName("PowerAttorney");
            this.Property(t => t.CollectionFile).HasColumnName("CollectionFile");
            this.Property(t => t.LenderName).HasColumnName("LenderName");
            this.HasRequired(t => t.BaseAudit)
                .WithRequiredDependent(t => t.PublicMortgage);
        }
    }
}
