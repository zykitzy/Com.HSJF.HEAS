using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit.Mapping
{
    public class BaseAuditMap : EntityTypeConfiguration<BaseAudit>
    {
        public BaseAuditMap()
        {
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

            this.Property(t => t.CreateUser)
                .HasMaxLength(128);
            this.Property(t => t.CaseStatus).HasMaxLength(64);

            this.Property(t => t.Term)
                .HasMaxLength(128);

            this.Property(t => t.AuditTerm)
                .HasMaxLength(128);

            this.Property(t => t.Partner)
                .HasMaxLength(128);
            this.Property(t => t.CaseMode)
                .HasMaxLength(128);

            this.Property(t => t.LoanAmount).HasPrecision(18, 6);
            this.Property(t => t.AuditAmount).HasPrecision(18, 6);
            this.Property(t => t.AnnualRate).HasPrecision(8, 6);
            this.Property(t => t.PlatformCharge).HasPrecision(18, 6);
            this.Property(t => t.ComprehensiveRate).HasPrecision(8, 6);
            this.Property(t => t.AuditRate).HasPrecision(8, 6);
            this.Property(t => t.MonthlyInterest).HasPrecision(18, 6);
            this.Property(t => t.ServiceCharge).HasPrecision(18, 6);
            this.Property(t => t.ServiceChargeRate).HasPrecision(8, 6);
            this.Property(t => t.EarnestMoney).HasPrecision(18, 6);
            this.Property(t => t.OutboundCost).HasPrecision(18, 6);
            this.Property(t => t.DebitEvaluationCost).HasPrecision(18, 6);
            this.Property(t => t.DebitGuaranteeCost).HasPrecision(18, 6);
            this.Property(t => t.DebitInsuranceCost).HasPrecision(18, 6);
            this.Property(t => t.DebitNotarizationCost).HasPrecision(18, 6);
            this.Property(t => t.DebitOtherCost).HasPrecision(18, 6);
            this.Property(t => t.LevyAssetsSurveyCost).HasPrecision(18, 6);
            this.Property(t => t.LevyCreditReportCost).HasPrecision(18, 6);
            this.Property(t => t.LevyNotarizationCost).HasPrecision(18, 6);
            this.Property(t => t.LevyOtherCost).HasPrecision(18, 6);
            this.Property(t => t.ThirdPartyAuditAmount).HasPrecision(18, 6);
            this.Property(t => t.ThirdPartyAuditRate).HasPrecision(8, 6);
            this.Property(t => t.CustEarnestMoney).HasPrecision(18, 6);

            this.Property(t => t.BankCard)
               .HasMaxLength(20);

            this.Property(t => t.OpeningBank)
               .HasMaxLength(256);

            this.Property(t => t.OpeningSite)
               .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("BaseAudit", "audit");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CaseNum).HasColumnName("CaseNum");
            this.Property(t => t.CaseType).HasColumnName("CaseType");
            this.Property(t => t.SalesID).HasColumnName("SalesID");
            this.Property(t => t.SalesGroupID).HasColumnName("SalesGroupID");
            this.Property(t => t.DistrictID).HasColumnName("DistrictID");
            this.Property(t => t.BorrowerName).HasColumnName("BorrowerName");
            this.Property(t => t.LoanAmount).HasColumnName("LoanAmount");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.IsNeedReport).HasColumnName("IsNeedReport");
            this.Property(t => t.CaseStatus).HasColumnName("CaseStatus");
            this.Property(t => t.FaceReportFile).HasColumnName("FaceReportFile");
            this.Property(t => t.FieldReportFile).HasColumnName("FieldReportFile");
            this.Property(t => t.LoanDetailReportFile).HasColumnName("LoanDetailReportFile");
            this.Property(t => t.Term).HasColumnName("Term");
            this.Property(t => t.AuditTerm).HasColumnName("AuditTerm");
            this.Property(t => t.Partner).HasColumnName("Partner");
            this.Property(t => t.BankCard).HasColumnName("BankCard");
            this.Property(t => t.OpeningBank).HasColumnName("OpeningBank");
            this.Property(t => t.OpeningSite).HasColumnName("OpeningSite");
            this.Property(t => t.AuditAmount).HasColumnName("AuditAmount");
            this.Property(t => t.AnnualRate).HasColumnName("AnnualRate");
            this.Property(t => t.PlatformCharge).HasColumnName("PlatformCharge");
            this.Property(t => t.ComprehensiveRate).HasColumnName("ComprehensiveRate");
            this.Property(t => t.MortgageOrder).HasColumnName("MortgageOrder");
            this.Property(t => t.CaseDetail).HasColumnName("CaseDetail");
            this.Property(t => t.AuditRate).HasColumnName("AuditRate");
            this.Property(t => t.AuditComment).HasColumnName("AuditComment");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.RejectType).HasColumnName("RejectType");
            this.Property(t => t.Deposit).HasColumnName("Deposit");
            this.Property(t => t.DepositDate).HasColumnName("DepositDate");
            this.Property(t => t.ServiceCharge).HasColumnName("ServiceCharge");
            this.Property(t => t.ServiceChargeRate).HasColumnName("ServiceChargeRate");
            this.Property(t => t.IsActivitieRate).HasColumnName("IsActivitieRate");
            this.Property(t => t.LenderName).HasColumnName("LenderName");
            this.Property(t => t.EarnestMoney).HasColumnName("EarnestMoney");
            this.Property(t => t.OutboundCost).HasColumnName("OutboundCost");
            this.Property(t => t.DebitNotarizationCost).HasColumnName("DebitNotarizationCost");
            this.Property(t => t.DebitEvaluationCost).HasColumnName("DebitEvaluationCost");
            this.Property(t => t.DebitGuaranteeCost).HasColumnName("DebitGuaranteeCost");
            this.Property(t => t.DebitInsuranceCost).HasColumnName("DebitInsuranceCost");
            this.Property(t => t.DebitOtherCost).HasColumnName("DebitOtherCost");
            this.Property(t => t.LevyNotarizationCost).HasColumnName("LevyNotarizationCost");
            this.Property(t => t.LevyAssetsSurveyCost).HasColumnName("LevyAssetsSurveyCost");
            this.Property(t => t.LevyCreditReportCost).HasColumnName("LevyCreditReportCost");
            this.Property(t => t.LevyOtherCost).HasColumnName("LevyOtherCost");
            this.Property(t => t.CaseMode).HasColumnName("CaseMode");
            this.Property(t => t.MonthlyInterest).HasColumnName("MonthlyInterest");
            this.Property(t => t.ThirdParty).HasColumnName("ThirdParty");
            this.Property(t => t.LendingDate).HasColumnName("LendingDate");
            this.Property(t => t.PaymentDate).HasColumnName("PaymentDate");
            this.Property(t => t.ActualInterest).HasColumnName("ActualInterest");
            this.Property(t => t.AdvanceInterest).HasColumnName("AdvanceInterest");
            this.Property(t => t.NewCaseNum).HasColumnName("NewCaseNum").HasMaxLength(20);

            this.Property(t => t.PaymentFactor).HasColumnName("PaymentFactor");
            this.Property(t => t.Purpose).HasColumnName("Purpose");
            this.Property(t => t.LendingTerm).HasColumnName("LendingTerm");
            this.Property(t => t.ContractFileInfo).HasColumnName("ContractFileInfo");
            this.Property(t => t.RejectReason).HasColumnName("RejectReason");
            this.Property(t => t.LoanProposedFile).HasColumnName("LoanProposedFile");
            this.Property(t => t.CustEarnestMoney).HasColumnName("CustEarnestMoney");
        }
    }
}
