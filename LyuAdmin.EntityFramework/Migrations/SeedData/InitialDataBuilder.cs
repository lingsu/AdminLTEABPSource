using LyuAdmin.EntityFramework;

namespace LyuAdmin.Migrations.SeedData
{
    public class InitialDataBuilder
    {
        private readonly LyuAdminDbContext _context;

        public InitialDataBuilder(LyuAdminDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            new DefaultTenantRoleAndUserBuilder(_context).Build();
        }
    }
}
