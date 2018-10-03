using CatalogMicroS.DL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogMicroS.DL
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {

        }

        public DbSet<CatalogItemEntity> CatalogItems { get; set; }
    }
}
