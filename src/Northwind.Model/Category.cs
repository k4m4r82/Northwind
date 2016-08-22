using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.Model
{
    public class Category
    {
        public Category()
        {
            this.Products = new List<Product>();
        }

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
