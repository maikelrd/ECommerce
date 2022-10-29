using ECommerce.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Data
{
    public interface IAppRepository
    {
        //General
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        //Category
        Task<Category[]> GetAllCategoriesAsync(bool includeProducts = false);
        Task<Category> GetCategoryById(int id, bool includeProducts = false);
        Task<Category> GetCategoryByName(string name, bool includeProducts = false);

        Task<Product[]> GetAllProductsByCategoryAsync(int id);

        //Department
        Task<Department[]> GetAllDepartmentsAsync(bool includeCategories = false);

        Task<Department> GetDepartmentById(int id, bool includeCategories = false);
        Task<Department> GetDepartmentByName(string name, bool includeCategories = false);


        //Product
        Task<Product[]> GetAllProductsAsync();
        Task<Product> GetProductById(int id);
        Task<Product> GetProductByName(string name);
        Task<Product[]> GetProductsByPage(int page);
        int GetCountProducts();
        int GetCountProductsFilter(string textFilter);
        Task<Product[]> GetProductsByCategoryAndPage(int categoryId, int page);
        int GetCountProductsByCategory(int categoryId);
        Task<Product[]> GetProductsFilter(string filterBy);

        //filter
        int GetCountProductsByCategoryFilter(int categoryId, string filterText);
        Task<Product[]> GetProductsByCategoryFilter(int categoryId,string filterBy);

        //User
      //  Task<User[]> GetAllUserAsync();
       // Task<User> GetUserByIdAsync(int id);
        Task<UsersEcommerce> GetUserAspNetByIdAsync(string id);
      //  Task<User> GetUserByNameAsync(string name);
        Task<UsersEcommerce> GetUserAspNetByNameAsync(string name);
      //  Task<User> GetUserByEmailAsync(string email);
        Task<UsersEcommerce> GetUserAspNetByEmailAsync(string email);

        //ShoppingCart old
        Task<ShoppingCartItem[]> GetAllCartsAsync();        
        //Task<IActionResult> ClearCart(int id);
        Task<ShoppingCartItem> GetCartByIdAsync(int id);
         Task<ShoppingCartItem[]> GetCartsByUserAsync(int userId);
        //Task<IActionResult> AddToCart(ShoppingCartItem shoppingCartItem);
        //Task<IActionResult> RemoveFromCart
        //Task ClearCart(string email);
        public void ClearCart(int userId);
        // Task<decimal> GetShoppingCartTotal(User user);
        decimal GetShoppingCartTotal(int id);

        ShoppingCartItem GetCartByProduct(Product product);
        //ShoppingCartItem GetCartById(int id);
        ShoppingCartItem GetCartByNotProduct(Product product, int userId);

        //shoppingCart New
        Task<ShoppingCart[]> GetAllShoppingCartsAsync();
        ShoppingCart GetShoppingCartByUserAsync(string userId);
        Task<ShoppingCart> GetShoppingCartByIdAsync(int id);
        Task<ProductShoppingCart> GetProductShoppingCartAsyn(int shoppingCartId, int productId);
        Task<ProductShoppingCart[]> GetProductShoppingCartByShoppingCartIdAsyn(int shoppingCartId);

        //ProductShoppingCart
        Task<ProductShoppingCart[]> GetAllProductShoppingCartAsync();
        Task<ProductShoppingCart> GetProductShoppingCartByProductShoppingCartIdAsyn(int productShoppingCartId);
        void ClearProductShoppingCartCart(int shoppingCartId);

    }
}