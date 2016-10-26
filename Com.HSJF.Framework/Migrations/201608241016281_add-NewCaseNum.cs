namespace Com.HSJF.Framework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNewCaseNum : DbMigration
    {
        public override void Up()
        {
            AddColumn("audit.BaseAudit", "NewCaseNum", c => c.String());
            AddColumn("biz.BaseCase", "NewCaseNum", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("biz.BaseCase", "NewCaseNum");
            DropColumn("audit.BaseAudit", "NewCaseNum");
        }
    }
}
