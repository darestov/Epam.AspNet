using Epam.AspNet.Module1.Interfaces;
using Epam.AspNet.Module1.Models;
using System.Collections.Generic;
using System.Linq;

namespace Epam.AspNet.Module1.DataAccess
{
    public class SupplierRepository : ISupplierRepository
    {
        public SupplierRepository(NorthwindContext context)
        {
            Context = context;
        }

        public NorthwindContext Context { get; }

        public IEnumerable<Supplier> ListSuppliers()
        {
            return Context.Suppliers.ToList();
        }
    }
}
