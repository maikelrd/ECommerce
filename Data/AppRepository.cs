using ECommerce.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data
{
    public class AppRepository : IAppRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AppRepository> _logger;
        public AppRepository(AppDbContext context, ILogger<AppRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        //General
        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding an object of type {entity.GetType()} to the context");
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Removing an object of type {entity.GetType()} to the context");
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempting to sabe the changes in the context");
            //Only return success if at least one row was changed    
            return await _context.SaveChangesAsync() > 0;
        }

        //Departments
        public async Task<Department[]> GetAllDepartmentsAsync(bool includeCategories = false)
        {
            _logger.LogInformation($"Getting all Departments");
            IQueryable<Department> query = _context.Departments;

            if (includeCategories)
            {
                query = query.Include(c => c.Categories);
            }

            query = query.OrderBy(d => d.DepartmentName);
            return await query.ToArrayAsync();
        }

        public async Task<Department> GetDepartmentById(int id, bool includeCategories = false)
        {
            _logger.LogInformation($"Getting Department for id = {id}");

            IQueryable<Department> query = _context.Departments.Where(d => d.DepartmentId == id);
            if (includeCategories)
            {
                query = query.Include(c => c.Categories);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Department> GetDepartmentByName(string name, bool includeCategories = false)
        {
            _logger.LogInformation($"Getting Department for name = {name}");

            IQueryable<Department> query = _context.Departments.Where(d => d.DepartmentName == name);
            if (includeCategories)
            {
                query = query.Include(c => c.Categories);
            }

            return await query.FirstOrDefaultAsync();
        }

        //Categories
        public async Task<Category[]> GetAllCategoriesAsync(bool includeProducts = false)
        {
            _logger.LogInformation($"Getting all Categories");
            IQueryable<Category> query = _context.Categories;

            if (includeProducts)
            {
                query = query.Include(c => c.Products);
            }

            query = query.OrderByDescending(c => c.CategoryName);
            return await query.ToArrayAsync();
        }

        public async Task<Category> GetCategoryById(int id, bool includeProducts = false)
        {
            _logger.LogInformation($"Getting Category for id = {id}");

            IQueryable<Category> query = _context.Categories.Where(c => c.CategoryId == id);
            if (includeProducts)
            {
                query = query.Include(c => c.Products);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Category> GetCategoryByName(string name, bool includeProducts = false)
        {
            _logger.LogInformation($"Getting Category for name = {name}");

            IQueryable<Category> query = _context.Categories.Where(c => c.CategoryName == name);
            if (includeProducts)
            {
                query = query.Include(c => c.Products);
            }

            return await query.FirstOrDefaultAsync();
        }

        //Products
        public async Task<Product[]> GetAllProductsAsync()
        {
            _logger.LogInformation($"Getting all Products");
            IQueryable<Product> query = _context.Products;


            query = query.OrderByDescending(p => p.ProductName);
            return await query.ToArrayAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            _logger.LogInformation($"Getting Product for id = {id}");

            IQueryable<Product> query = _context.Products.Where(p => p.ProductId == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByName(string name)
        {
            _logger.LogInformation($"Getting Product for name = {name}");

            IQueryable<Product> query = _context.Products.Where(p => p.ProductName == name);

            return await query.FirstOrDefaultAsync();
        }

        //Users
        public async Task<User[]> GetAllUserAsync()
        {
            _logger.LogInformation($"Getting all Users");
            IQueryable<User> query = _context.Users.OrderByDescending(u => u.FirstName);
            return await query.ToArrayAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            _logger.LogInformation($"Getting a User for {id}");
            IQueryable<User> query = _context.Users.Where(u => u.UserId == id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            _logger.LogInformation($"Getting a User for {name}");
            IQueryable<User> query = _context.Users.Where(u => u.FirstName == name);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            _logger.LogInformation($"Getting a User for {email}");
            IQueryable<User> query = _context.Users.Where(u => u.Email == email);
            return await query.FirstOrDefaultAsync();
        }

        //ShoppingCarts
        public async Task<ShoppingCartItem[]> GetAllCartsAsync()
        {
            _logger.LogInformation($"Getting all ShoppingCarts");

            IQueryable<ShoppingCartItem> query = _context.ShoppingCartItems.
                Include(p => p.Product);
            return await query.ToArrayAsync();
        }

        public async Task<ShoppingCartItem[]> GetCartsByUserAsync(int userId)
        {
            _logger.LogInformation($"Getting a Shopping Cart for user:  {userId}");


            IQueryable<ShoppingCartItem> query = _context.ShoppingCartItems.Where(c => c.UserId == userId)
                                                                           .Include(p => p.Product);

            return await query.ToArrayAsync();
        }

        public   Task<ShoppingCartItem> GetCartByIdAsync(int id)
        {
            _logger.LogInformation($"Getting a Shopping Cart for Id:  {id}");
            IQueryable<ShoppingCartItem> query = _context.ShoppingCartItems.Where(c => c.ShoppingCartItemId == id)
                                                                           .Include(p => p.Product);
            return  query.FirstOrDefaultAsync();
        }

        //public ShoppingCartItem GetCartById(int id)
        //{
        //    _logger.LogInformation($"Getting a Shopping Cart for Id:  {id}");
        //    IQueryable<ShoppingCartItem> query = _context.ShoppingCartItems.Where(c => c.ShoppingCartItemId == id)
        //                                                                   .Include(p => p.ProductId);
        //    return query.FirstOrDefault();
        //}

        public ShoppingCartItem GetCartByProduct(Product product)
        {
            var query = _context.ShoppingCartItems.Where(p => p.ProductId == product.ProductId).
                Include(p =>p.Product);
            return  query.FirstOrDefault();
        }
        public ShoppingCartItem GetCartByNotProduct(Product product, int userId)
        {
            var query = _context.ShoppingCartItems.Where(s => s.ProductId == product.ProductId && s.UserId == userId);
              
            return query.FirstOrDefault();
        }
        //public async Task<IActionResult> AddToCart(Product product)
        //{
        //    _logger.LogInformation($"Adding a product to the shopping cart");
        //    var query = _context.ShoppingCartItems.Where(s => s.Product.ProductId == product.ProductId)
        //    return await query.SingleOrDefaultAsync();
        //}
        public void ClearCart(int userId)
        {

            var cartItems = _context.ShoppingCartItems.Where(cart => cart.UserId == userId);
            _context.RemoveRange(cartItems);
            // _context.SaveChanges();

        }

        public decimal GetShoppingCartTotal(int userId)
        {
        //    var total = _context.ShoppingCartItems.Where(c => c.UserId == userId)
        //                    .Select(c => c.Product.UnitPrice * c.Amount).Sum();
        //    return total;
        return 0;

        }
    }
}
