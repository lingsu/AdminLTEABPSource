namespace LyuAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addHosts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpTenants", "Hosts", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpTenants", "Hosts");
        }
    }
}
