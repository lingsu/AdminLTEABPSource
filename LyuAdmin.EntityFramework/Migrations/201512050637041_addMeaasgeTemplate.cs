namespace LyuAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMeaasgeTemplate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        BccEmailAddresses = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        AttachedDownloadId = c.Int(nullable: false),
                        EmailAccountId = c.Int(nullable: false),
                        LimitedToStores = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MessageTemplates");
        }
    }
}
