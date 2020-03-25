using Epam.AspNet.Module1.Interfaces;
using Epam.AspNet.Module1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.AspNet.Module1.DataAccess
{
    public class CategoryRepository: ICategoryRepository
    {
        public CategoryRepository(NorthwindContext context)
        {
            Context = context;
        }

        public NorthwindContext Context { get; }

        public IEnumerable<Category> ListCategories()
        {
            return Context.Categories.ToList();
        }
    }
}
