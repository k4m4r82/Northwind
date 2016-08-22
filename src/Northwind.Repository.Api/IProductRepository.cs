using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Northwind.Model;

namespace Northwind.Repository.Api
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Product GetByID(int productId);
        IList<Product> GetByCategory(int categoryId);
        IList<Product> GetByName(string productName);
    }
}
