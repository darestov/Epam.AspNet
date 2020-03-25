using Epam.AspNet.Module1.Interfaces;
using Epam.AspNet.Module1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.AspNet.Module1.DataAccess
{
    public class ProductRepository: IProductRepository
    {
        public ProductRepository(NorthwindContext context)
        {
            Context = context;
        }

        public NorthwindContext Context { get; }

        public void Add(Product product)
        {
            Context.Add(product);
        }

        public Product GetProduct(int id)
        {
            return Context.Products.Find(id);
        }

        public IEnumerable<Product> ListProductsWithCategoriesAndSuppliers(int max = 0)
        {
            IQueryable<Product> query = Context.Products.Include(x => x.Supplier).Include(x => x.Category);
            if (max != 0)
                query = query.Take(max);
            return query.ToList();
        }
    }
}
