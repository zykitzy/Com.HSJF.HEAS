namespace Com.HSJF.Framework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_NewCaseNum_Length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("audit.BaseAudit", "NewCaseNum", c => c.String(maxLength: 20));
            AlterColumn("biz.BaseCase", "NewCaseNum", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("biz.BaseCase", "NewCaseNum", c => c.String());
            AlterColumn("audit.BaseAudit", "NewCaseNum", c => c.String());
        }
    }
}
