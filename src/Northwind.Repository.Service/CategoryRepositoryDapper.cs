using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Northwind.Model;
using Northwind.Repository.Api;
using Dapper;

namespace Northwind.Repository.Service
{
    public class CategoryRepositoryDapper : ICategoryRepository
    {
        private string _sql;

        public CategoryRepositoryDapper()
        {
        }

        public Category GetByID(int categoryId)
        {
            Category category = null;

            try
            {
                _sql = @"SELECT CategoryID, CategoryName, Description
                         FROM Categories
                         WHERE CategoryID = @categoryId";

                using (IDapperContext context = new DapperContext())
                {
                    category = context.db.Query<Category>(_sql, new { categoryId })
                                      .SingleOrDefault();
                }                
            }
            catch
            {
            }

            return category;
        }

        public IList<Category> GetByName(string categoryName)
        {
            IList<Category> listOfCategory = new List<Category>();

            try
            {
                _sql = @"SELECT CategoryID, CategoryName, Description 
                         FROM Categories
                         WHERE CategoryName LIKE @categoryName
                         ORDER BY CategoryName";
                
                categoryName = string.Format("%{0}%", categoryName);

                using (IDapperContext context = new DapperContext())
                {
                    listOfCategory = context.db.Query<Category>(_sql, new { categoryName })
                                            .ToList();
                }                
            }
            catch
            {
            }

            return listOfCategory;
        }

        public IList<Category> GetAll()
        {
            IList<Category> listOfCategory = new List<Category>();

            try
            {
                _sql = @"SELECT CategoryID, CategoryName, Description 
                         FROM Categories
                         ORDER BY CategoryName";

                using (IDapperContext context = new DapperContext())
                {
                    listOfCategory = context.db.Query<Category>(_sql)
                                            .ToList();
                }                
            }
            catch
            {
            }

            return listOfCategory;
        }        

        public int Save(Category obj)
        {
            var result = 0;

            try
            {
                _sql = @"INSERT INTO Categories (CategoryName, Description)
                         VALUES (@CategoryName, @Description)";

                using (IDapperContext context = new DapperContext())
                {
                    result = context.db.Execute(_sql, obj);

                    if (result > 0)
                        obj.CategoryID = context.GetLastId();
                }                                
            }
            catch
            {
            }

            return result;
        }

        public int Update(Category obj)
        {
            var result = 0;

            try
            {
                _sql = @"UPDATE Categories SET CategoryName = @CategoryName, Description = @Description
                         WHERE CategoryID = @CategoryID";

                using (IDapperContext context = new DapperContext())
                {
                    result = context.db.Execute(_sql, obj);
                }                
            }
            catch
            {
            }

            return result;
        }

        public int Delete(Category obj)
        {
            var result = 0;

            try
            {
                _sql = @"DELETE FROM Categories
                         WHERE CategoryID = @CategoryID";

                using (IDapperContext context = new DapperContext())
                {
                    result = context.db.Execute(_sql, obj);
                }                
            }
            catch
            {
            }

            return result;
        }        
    }
}
