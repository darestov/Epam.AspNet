using Epam.AspNet.Module1.Interfaces;
using Epam.AspNet.Module1.Models;
using Microsoft.EntityFrameworkCore;

namespace Epam.AspNet.Module1.DataAccess
{
    public class NorthwindContext: DbContext
    {
        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            :base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }


    }

    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        ISupplierRepository Suppliers { get; }

        int SaveChanges();
    }

    public class EfCoreUnitOfWork : IUnitOfWork
    {
        public EfCoreUnitOfWork(NorthwindContext context)
        {
            Context = context;
            Products = new ProductRepository(Context);
            Categories = new CategoryRepository(Context);
            Suppliers = new SupplierRepository(Context);
        }
        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get ; }
        public ISupplierRepository Suppliers { get; }
        private NorthwindContext Context { get; }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }
    }

}
