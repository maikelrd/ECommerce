using ECommerce.Data.Entities;
using System.Threading.Tasks;

namespace ECommerce.Data
{
    public interface IAppRepository
    {
        //General
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        //Category
        Task<Category[]> GetAllCategoriesAsync(bool includeProducts = false);
        Task<Category> GetCategoryById(int id, bool includeProducts = false);
        Task<Category> GetCategoryByName(string name, bool includeProducts = false);

        //Department
        Task<Department[]> GetAllDepartmentsAsync(bool includeCategories = false);

        Task<Department> GetDepartmentById(int id, bool includeCategories = false);
        Task<Department> GetDepartmentByName(string name, bool includeCategories = false);
        Task<Product[]> GetAllProductsAsync();
   
        //Product
        Task<Product> GetProductById(int id);
        Task<Product> GetProductByName(string name);
        Task<bool> SaveChangesAsync();
    }
}