using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Northwind.Model;
using Northwind.Repository.Api;
using Dapper;

namespace Northwind.Repository.Service
{
    public class ProductRepositoryDapper : IProductRepository
    {
        private IDapperContext _context;
        private string _sql;

        // melewatkan objek context via constructor
        public ProductRepositoryDapper(IDapperContext context)
        {
            _context = context;
        }

        private IEnumerable<Product> MappingRecordToObj(string sql, object param = null)
        {
            var product = _context.db.Query<Product, Category, Product>(_sql, (p, c) =>
            {
                p.CategoryID = c.CategoryID; p.Category = c;
                return p;
            }, param, splitOn: "CategoryID");

            return product;
        }

        public Product GetByID(int productId)
        {
            Product product = null;

            try
            {
                _sql = @"SELECT Products.ProductID, Products.ProductName, Products.QuantityPerUnit, 
                         Products.UnitPrice, Products.UnitsInStock,
                         Categories.CategoryID, Categories.CategoryName, Categories.Description
                         FROM Products INNER JOIN Categories ON Categories.CategoryId = Products.CategoryId 
                         WHERE Products.ProductID = @productId";

                product = MappingRecordToObj(_sql, new { productId }).SingleOrDefault();
            }
            catch
            {
            }

            return product;
        }

        public IList<Product> GetByCategory(int categoryId)
        {
            IList<Product> listOfProduct = new List<Product>();

            try
            {
                _sql = @"SELECT Products.ProductID, Products.ProductName, Products.QuantityPerUnit, 
                         Products.UnitPrice, Products.UnitsInStock,
                         Categories.CategoryID, Categories.CategoryName, Categories.Description
                         FROM Products INNER JOIN Categories ON Categories.CategoryId = Products.CategoryId 
                         WHERE Categories.CategoryID = @categoryId
                         ORDER BY Products.ProductName";

                listOfProduct = MappingRecordToObj(_sql, new { categoryId }).ToList();
            }
            catch
            {
            }

            return listOfProduct;
        }

        public IList<Product> GetByName(string productName)
        {
            IList<Product> listOfProduct = new List<Product>();

            try
            {
                _sql = @"SELECT Products.ProductID, Products.ProductName, Products.QuantityPerUnit, 
                         Products.UnitPrice, Products.UnitsInStock,
                         Categories.CategoryID, Categories.CategoryName, Categories.Description
                         FROM Products INNER JOIN Categories ON Categories.CategoryId = Products.CategoryId 
                         WHERE Products.ProductName LIKE @productName
                         ORDER BY Products.ProductName";

                productName = string.Format("%{0}%", productName);
                listOfProduct = MappingRecordToObj(_sql, new { productName }).ToList();
            }
            catch
            {
            }

            return listOfProduct;
        }

        public IList<Product> GetAll()
        {
            IList<Product> listOfProduct = new List<Product>();

            try
            {
                _sql = @"SELECT Products.ProductID, Products.ProductName, Products.QuantityPerUnit, 
                         Products.UnitPrice, Products.UnitsInStock,
                         Categories.CategoryID, Categories.CategoryName, Categories.Description
                         FROM Products INNER JOIN Categories ON Categories.CategoryId = Products.CategoryId 
                         ORDER BY Products.ProductName";

                listOfProduct = MappingRecordToObj(_sql).ToList();
            }
            catch
            {
            }

            return listOfProduct;
        }

        public int Save(Product obj)
        {
            var result = 0;

            try
            {
                _sql = @"INSERT INTO Products (ProductName, QuantityPerUnit, UnitPrice, UnitsInStock)
                         VALUES (@ProductName, @QuantityPerUnit, @UnitPrice, @UnitsInStock)";
                result = _context.db.Execute(_sql, obj);

                if (result > 0)
                    obj.ProductID = _context.GetLastId();
            }
            catch
            {
            }

            return result;
        }

        public int Update(Product obj)
        {
            var result = 0;

            try
            {
                _sql = @"UPDATE Products SET ProductName = @ProductName, QuantityPerUnit = @QuantityPerUnit, 
                         UnitPrice = @UnitPrice, UnitsInStock = @UnitsInStock
                         WHERE ProductID = @ProductID";
                result = _context.db.Execute(_sql, obj);
            }
            catch
            {
            }

            return result;
        }

        public int Delete(Product obj)
        {
            var result = 0;

            try
            {
                _sql = @"DELETE FROM Products
                         WHERE ProductID = @ProductID";
                result = _context.db.Execute(_sql, obj);
            }
            catch
            {
            }

            return result;
        }
        
    }
}
