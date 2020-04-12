using Epam.AspNet.Module1.Models;
using System.Collections.Generic;

namespace Epam.AspNet.Module1.Interfaces
{
    public interface ISupplierRepository
    {
        IEnumerable<Supplier> ListSuppliers();

    }
}
