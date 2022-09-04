using ECommerce.Data.Entities;
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

        public async Task<Department[]> GetAllDepartmentsAsync(bool includeCategories = false)
        {
            _logger.LogInformation($"Getting all Departments");
            IQueryable<Department> query = _context.Departments;

            if (includeCategories)
            {
                query=query.Include(c => c.Categories);
            }

            query = query.OrderByDescending(d => d.DepartmentName);
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

        public async Task<Category[]> GetAllCategoriesAsync(bool includeProducts = false)
        {
            _logger.LogInformation($"Getting all Categories");
            IQueryable<Category> query = _context.Categories;

            if (includeProducts)
            {
               query= query.Include(c => c.Products);
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
    }
}
