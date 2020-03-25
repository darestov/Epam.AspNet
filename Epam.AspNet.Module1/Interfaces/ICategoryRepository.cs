using Epam.AspNet.Module1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.AspNet.Module1.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> ListCategories();
    }
}
