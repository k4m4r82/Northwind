using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Northwind.Model;

namespace Northwind.Repository.Api
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Category GetByID(int categoryId);
        IList<Category> GetByName(string categoryName);
    }
}
