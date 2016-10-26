namespace Com.HSJF.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileDescriptions",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        FileState = c.Int(nullable: false),
                        LinkID = c.Guid(nullable: false),
                        LinkKey = c.String(),
                        FileName = c.String(),
                        FileSaveName = c.String(),
                        FileType = c.String(),
                        FileCreateTime = c.DateTime(nullable: false),
                        FileData = c.Binary(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FileDescriptions");
        }
    }
}
