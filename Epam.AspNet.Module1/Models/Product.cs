using Epam.AspNet.Module1.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace Epam.AspNet.Module1.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        [Required]
        [NobodyLikesBroccoli]
        public string ProductName { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public short UnitsInStock{ get; set; }
        public short UnitsOnOrder { get; set; }
        public short ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        public int CategoryID { get; set; }
        public int SupplierID { get; set; }

        public Category Category { get; set; }
        public Supplier Supplier { get; set; }
    }
}
