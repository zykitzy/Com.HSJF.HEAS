namespace Com.HSJF.Framework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_ShortCode_for_SalesGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("sysset.SalesGroup", "ShortCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("sysset.SalesGroup", "ShortCode");
        }
    }
}
