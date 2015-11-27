using System.Data.Entity.Migrations;
using LyuAdmin.Migrations.SeedData;
using EntityFramework.DynamicFilters;

namespace LyuAdmin.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<LyuAdmin.EntityFramework.LyuAdminDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "LyuAdmin";
        }

        protected override void Seed(LyuAdmin.EntityFramework.LyuAdminDbContext context)
        {
            context.DisableAllFilters();
            new InitialDataBuilder(context).Build();
        }
    }
}
