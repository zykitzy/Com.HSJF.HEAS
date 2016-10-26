using System.Data.Entity;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.EntityFramework.Model.Audit.Mapping;
using Com.HSJF.Framework.EntityFramework.Model.Biz;
using Com.HSJF.Framework.EntityFramework.Model.Biz.Mapping;
using Com.HSJF.Framework.EntityFramework.Model.Lending;
using Com.HSJF.Framework.EntityFramework.Model.Lending.Mapping;
using Com.HSJF.Framework.EntityFramework.Model.Mortgage;
using Com.HSJF.Framework.EntityFramework.Model.Mortgage.Mapping;
using Com.HSJF.Framework.EntityFramework.Model.Others;
using Com.HSJF.Framework.EntityFramework.Model.Others.Mapping;
using Com.HSJF.Framework.EntityFramework.Model.Sales;
using Com.HSJF.Framework.EntityFramework.Model.Sales.Mapping;
using Com.HSJF.Framework.EntityFramework.Model.SystemSetting;
using Com.HSJF.Framework.EntityFramework.Model.SystemSetting.Mapping;

namespace Com.HSJF.Framework.Models
{
    public partial class HEASContext : DbContext
    {
        static HEASContext()
        {
            Database.SetInitializer<HEASContext>(null);
        }

        public HEASContext()
            : base("Name=HEASContext")
        {
        }
        //系统
        public DbSet<Menu> Menus { get; set; }
        public DbSet<User2Menu> User2Menu { get; set; }
        //       public DbSet<Menu2Permission> Menu2Permission { get; set; }
        public DbSet<SalesGroup> SalesGroups { get; set; }
        public DbSet<SalesMan> SalesMen { get; set; }
        public DbSet<Dictionary> Dictionary { get; set; }
        public DbSet<DataPermission> DataPermission { get; set; }
        public DbSet<RelationState> RelationState { get; set; }
        public DbSet<Role2Menu> Role2Menu { get; set; }
        public DbSet<Role2DataPermission> Role2DataPermission { get; set; }
        //进件
        public DbSet<BaseCase> BaseCases { get; set; }
        public DbSet<Collateral> Collaterals { get; set; }
        public DbSet<RelationEnterprise> RelationEnterprises { get; set; }
        public DbSet<RelationPerson> RelationPersons { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public DbSet<Introducer> Introducers { get; set; }
        //审核
        public DbSet<BaseAudit> BaseAudits { get; set; }
        public DbSet<EnforcementPerson> EnforcementPersons { get; set; }
        public DbSet<EnterpriseCredit> EnterpriseCredits { get; set; }
        public DbSet<EstimateSource> EstimateSources { get; set; }
        public DbSet<Guarantor> Guarantors { get; set; }
        public DbSet<HouseDetail> HouseDetails { get; set; }
        public DbSet<IndividualCredit> IndividualCredits { get; set; }
        public DbSet<IndustryCommerceTax> IndustryCommerceTaxs { get; set; }
        public DbSet<CollateralAudit> CollateralAudit { get; set; }
        public DbSet<RelationEnterpriseAudit> RelationEnterpriseAudit { get; set; }
        public DbSet<RelationPersonAudit> RelationPersonAudit { get; set; }
        public DbSet<ContactAudit> ContactAudit { get; set; }
        public DbSet<AddressAudit> AddresseAudit { get; set; }
        public DbSet<EmergencyContactAudit> EmergencyContactAudit { get; set; }
        public DbSet<IntroducerAudit> IntroducerAudit { get; set; }
        //公证抵押
        public DbSet<PublicMortgage> PublicMortgage { get; set; }

        //放款
        public DbSet<Lending> Lending { get; set; }

        public DbSet<MigT> MigT { get; set; }

        public DbSet<Menu2Role> Menu2Role { get; set; }//角色菜单

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MenuMap());
            //       modelBuilder.Configurations.Add(new Menu2PermissionMap());
            modelBuilder.Configurations.Add(new SalesGroupMap());
            modelBuilder.Configurations.Add(new SalesManMap());
            modelBuilder.Configurations.Add(new DictionaryMap());
            modelBuilder.Configurations.Add(new DistrictMap());
            modelBuilder.Configurations.Add(new User2MenuMap());
            modelBuilder.Configurations.Add(new DataPermissionMap());
            modelBuilder.Configurations.Add(new Role2MenuMap());
            modelBuilder.Configurations.Add(new Role2DataPermissionMap());

            modelBuilder.Configurations.Add(new BaseCaseMap());
            modelBuilder.Configurations.Add(new CollateralMap());
            modelBuilder.Configurations.Add(new RelationPersonMap());
            modelBuilder.Configurations.Add(new RelationEnterpriseMap());
            modelBuilder.Configurations.Add(new ContactMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new EmergencyContactMap());
            modelBuilder.Configurations.Add(new IntroducerMap());

            modelBuilder.Configurations.Add(new BaseAuditMap());
            modelBuilder.Configurations.Add(new EnforcementPersonMap());
            modelBuilder.Configurations.Add(new EnterpriseCreditMap());
            modelBuilder.Configurations.Add(new EstimateSourceMap());
            modelBuilder.Configurations.Add(new GuarantorMap());
            modelBuilder.Configurations.Add(new HouseDetailMap());
            modelBuilder.Configurations.Add(new IndividualCreditMap());
            modelBuilder.Configurations.Add(new IndustryCommerceTaxMap());
            modelBuilder.Configurations.Add(new CollateralAuditMap());
            modelBuilder.Configurations.Add(new RelationPersonAuditMap());
            modelBuilder.Configurations.Add(new RelationEnterpriseAuditMap());
            modelBuilder.Configurations.Add(new ContactAuditMap());
            modelBuilder.Configurations.Add(new AddressAuditMap());
            modelBuilder.Configurations.Add(new EmergencyContactAuditMap());
            modelBuilder.Configurations.Add(new IntroducerAuditMap());

            modelBuilder.Configurations.Add(new PublicMortgageMap());
            modelBuilder.Configurations.Add(new LendMap());

            modelBuilder.Configurations.Add(new RelationStateMap());


            modelBuilder.Configurations.Add(new MigTMapping());

            modelBuilder.Configurations.Add(new Menu2RoleMap());//角色菜单
        }
    }
}
