namespace Com.HSJF.Framework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_thirdparty_properties : DbMigration
    {
        public override void Up()
        {
            AddColumn("audit.BaseAudit", "ThirdPartyAuditAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("audit.BaseAudit", "ThirdPartyAuditTerm", c => c.String());
            AddColumn("audit.BaseAudit", "ThirdPartyAuditRate", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("audit.BaseAudit", "ThirdPartyAuditRate");
            DropColumn("audit.BaseAudit", "ThirdPartyAuditTerm");
            DropColumn("audit.BaseAudit", "ThirdPartyAuditAmount");
        }
    }
}
