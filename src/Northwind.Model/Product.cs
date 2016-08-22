using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.Model
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string QuantityPerUnit { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<short> UnitsInStock { get; set; }

        public Nullable<int> CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }
}
