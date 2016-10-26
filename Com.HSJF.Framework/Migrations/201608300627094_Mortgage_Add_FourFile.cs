namespace Com.HSJF.Framework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mortgage_Add_FourFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("after.PublicMortgage", "FourFile", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("after.PublicMortgage", "FourFile");
        }
    }
}
