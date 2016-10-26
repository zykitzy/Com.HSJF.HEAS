namespace Com.HSJF.Framework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAnnualRate : DbMigration
    {
        public override void Up()
        {
            AddColumn("biz.BaseCase", "AnnualRate", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("biz.BaseCase", "AnnualRate");
        }
    }
}
