using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace LyuAdmin.EntityFramework.Repositories
{
    public abstract class LyuAdminRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<LyuAdminDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected LyuAdminRepositoryBase(IDbContextProvider<LyuAdminDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class LyuAdminRepositoryBase<TEntity> : LyuAdminRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected LyuAdminRepositoryBase(IDbContextProvider<LyuAdminDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
