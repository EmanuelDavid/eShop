using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogMicroS.DL
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity GetAllFromBase();
        void InsertFromBase(TEntity entity);
    }
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationContext _dbContext;
        private DbSet<TEntity> entities;

        public BaseRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
            entities = dbContext.Set<TEntity>();
        }

        public TEntity GetAllFromBase()
        {
            return entities.FirstOrDefault();
        }

        public void InsertFromBase(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
        }
    }

    public abstract class BaseEntity
    {
        public long Id { get; set; }
    }


    public interface ISiteRepository : IBaseRepository<Site>
    {
        void GetBiggestBuilding();
    }

    public class SiteRepository : BaseRepository<Site>, ISiteRepository
    {
        public SiteRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }

        public void GetBiggestBuilding()
        {
            throw new NotImplementedException();
        }
    }

    public class Site: BaseEntity
    {
        public string Name { get; set; }

    }
}
