using System.Data.Common;
using Abp.Zero.EntityFramework;
using LyuAdmin.Authorization.Roles;
using LyuAdmin.MultiTenancy;
using LyuAdmin.Users;

namespace LyuAdmin.EntityFramework
{
    public class LyuAdminDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public LyuAdminDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in LyuAdminDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of LyuAdminDbContext since ABP automatically handles it.
         */
        public LyuAdminDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public LyuAdminDbContext(DbConnection connection)
            : base(connection, true)
        {

        }
    }
}
