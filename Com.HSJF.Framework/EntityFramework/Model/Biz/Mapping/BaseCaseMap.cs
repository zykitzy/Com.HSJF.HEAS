using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.Biz.Mapping
{
    public class BaseCaseMap : EntityTypeConfiguration<BaseCase>
    {
        public BaseCaseMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.CaseNum)
                .HasMaxLength(128);

            this.Property(t => t.CaseType)
                .HasMaxLength(50);

            this.Property(t => t.SalesID)
                .HasMaxLength(128);

            this.Property(t => t.SalesGroupID)
                .HasMaxLength(128);

            this.Property(t => t.DistrictID)
                .HasMaxLength(128);

            this.Property(t => t.BorrowerName)
                .HasMaxLength(128);

            this.Property(t => t.Term)
                .HasMaxLength(128);

            this.Property(t => t.Partner)
                .HasMaxLength(128);
            this.Property(t => t.CreateUser)
                .HasMaxLength(128);
            this.Property(t => t.BankCard)
                .HasMaxLength(20);
            this.Property(t => t.OpeningBank)
               .HasMaxLength(256);
            this.Property(t => t.OpeningSite)
               .HasMaxLength(256);

            this.Property(t => t.AnnualRate).HasPrecision(8, 6);
            this.Property(t => t.LoanAmount).HasPrecision(18, 6);
            this.Property(t => t.ServiceCharge).HasPrecision(18, 6);
            this.Property(t => t.ServiceChargeRate).HasPrecision(8, 6);

            // Table & Column Mappings
            this.ToTable("BaseCase", "biz");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CaseNum).HasColumnName("CaseNum");
            this.Property(t => t.CaseType).HasColumnName("CaseType");
            this.Property(t => t.SalesID).HasColumnName("SalesID");
            this.Property(t => t.SalesGroupID).HasColumnName("SalesGroupID");
            this.Property(t => t.DistrictID).HasColumnName("DistrictID");
            this.Property(t => t.BorrowerName).HasColumnName("BorrowerName");
            this.Property(t => t.LoanAmount).HasColumnName("LoanAmount");
            this.Property(t => t.Term).HasColumnName("Term");
            this.Property(t => t.Partner).HasColumnName("Partner");
            this.Property(t => t.BankCard).HasColumnName("BankCard");
            this.Property(t => t.OpeningBank).HasColumnName("OpeningBank");
            this.Property(t => t.OpeningSite).HasColumnName("OpeningSite");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Deposit).HasColumnName("Deposit");
            this.Property(t => t.DepositDate).HasColumnName("DepositDate");
            this.Property(t => t.ServiceCharge).HasColumnName("ServiceCharge");
            this.Property(t => t.ServiceChargeRate).HasColumnName("ServiceChargeRate");
            this.Property(t => t.IsActivitieRate).HasColumnName("IsActivitieRate");

            this.Property(t => t.PaymentFactor).HasColumnName("PaymentFactor");
            this.Property(t => t.Purpose).HasColumnName("Purpose");

            this.Property(t => t.NewCaseNum).HasColumnName("NewCaseNum").HasMaxLength(20);
        }
    }
}
