using Epam.AspNet.Module1.Models;
using Microsoft.EntityFrameworkCore;

namespace Epam.AspNet.Module1.DataAccess
{
    public class NorthwindContext: DbContext, IUnitOfWork
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
        DbSet<Product> Products { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Supplier> Suppliers { get; set; }

        int SaveChanges();
    }
   
}
