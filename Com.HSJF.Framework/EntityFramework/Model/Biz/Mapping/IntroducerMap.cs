using System.Data.Entity.ModelConfiguration;

namespace Com.HSJF.Framework.EntityFramework.Model.Biz.Mapping
{
    public class IntroducerMap : EntityTypeConfiguration<Introducer>
    {
        public IntroducerMap()
        {
            this.HasKey(t => t.ID);
            this.Property(t => t.CaseID).HasMaxLength(128);

            this.ToTable("Introducer", "biz");
            this.Property(t => t.Account).HasColumnName("Account");
            this.Property(t => t.Contract).HasColumnName("Contract");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CaseID).HasColumnName("CaseID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.RebateAmmount).HasColumnName("RebateAmmount");
            this.Property(t => t.RebateRate).HasColumnName("RebateRate");
            this.Property(t => t.AccountBank).HasColumnName("AccountBank");
            this.Property(t => t.Sequence).HasColumnName("Sequence");
            
            this.HasRequired(t => t.BaseCase).WithMany(t => t.Introducers).HasForeignKey(t => t.CaseID);
        }
    }
}
