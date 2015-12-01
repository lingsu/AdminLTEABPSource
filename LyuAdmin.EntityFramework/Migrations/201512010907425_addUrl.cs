namespace LyuAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpTenants", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpTenants", "Url");
        }
    }
}
