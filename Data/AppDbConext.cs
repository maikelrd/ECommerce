using ECommerce.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public AppDbContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<User> Users { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        optionsBuilder.UseSqlServer(_config.GetConnectionString("ECommerce"));
    //    }

    //    protected override void OnModelCreating(ModelBuilder bldr)
    //    {


    //        bldr.Entity<Department>()
    //            .HasData(new
    //            {
    //                DepartmentId = 1,
    //                DepartmentName = "Electronic"
    //            },
    //            new
    //            {
    //                DepartmentId = 2,
    //                DepartmentName = "Computer"
    //            },
    //            new
    //            {
    //                DepartmentId = 3,
    //                DepartmentName = "Food & Grocery"
    //            },
    //            new
    //            {
    //                DepartmentId = 4,
    //                DepartmentName = "Books"
    //            },
    //            new
    //            {
    //                DepartmentId = 5,
    //                DepartmentName = "Movies, Music & Games"
    //            });

    //        bldr.Entity<Category>()
    //           .HasData(new
    //           {
    //               CategoryId = 1,
    //               CategoryName = "Televisors",
    //               DepartmentId = 1
    //           },
    //           new
    //           {
    //               CategoryId = 2,
    //               CategoryName = "Computer&accessories",
    //               DepartmentId = 2
    //           },
    //           new
    //           {
    //               CategoryId = 3,
    //               CategoryName = "Grocery",
    //               DepartmentId = 3
    //           },
    //            new
    //            {
    //                CategoryId = 4,
    //                CategoryName = "Video Games",
    //                DepartmentId = 1
    //            });

    //        bldr.Entity<Product>()
    //            .HasData(
    //            new
    //            {
    //                ProductId = 1,
    //                ProductName = "TV Samsung",
    //                ProductCode = "TV1234",
    //                ReleaseDate = "2020/10,/18",
    //                CategoryId = 1,
    //                UnitPrice = 245m,
    //                StockQty = 10,
    //                Description = "Tv Samsung 32'' ",
    //                StarRating = 4.3m,
    //                ImageUrl = "/assets/images/711qRHKMRTL._AC_UY218_.jpg",
    //               // ShoppingCartItemId = 0
    //            }, new
    //            {
    //                ProductId = 2,
    //                ProductName = "Laptop Dell",
    //                ProductCode = "laptopdell",
    //                ReleaseDate = "2021/10,/18",
    //                CategoryId = 2,
    //                UnitPrice = 831m,
    //                StockQty = 20,
    //                Description = "Laptop dell i7 ",
    //                StarRating = 4.5m,
    //                ImageUrl = "/assets/images/Dell-Laptop.png",
    //                //ShoppingCartItemId = 0
    //            },
    //            new
    //            {
    //                ProductId = 3,
    //                ProductName = "Apples bag",
    //                ProductCode = "apple012",
    //                ReleaseDate = "2022/10/18",
    //                CategoryId = 3,
    //                UnitPrice = 9m,
    //                StockQty = 2,
    //                Description = "Apples bag 5 pounds ",
    //                StarRating = 4.0m,
    //                ImageUrl = "/assets/images/71gcGpnE02L._AC_UL320_.jpg",
    //               // ShoppingCartItemId = 0
    //            },
    //            new
    //            {
    //                ProductId = 4,
    //                ProductName = "HP Deskjet 2755e",
    //                ProductCode = "hp2755e",
    //                ReleaseDate = "2021/24/4",
    //                CategoryId = 2,
    //                UnitPrice = 76.0m,
    //                StockQty = 4,
    //                Description = "Wireless Color All-in-One",
    //                StarRating = 4.1m,
    //                ImageUrl = "/assets/images/61UdeL7aO-L._AC_SL1500_.jpg",
    //              //  ShoppingCartItemId = 0
    //            },
    //              new
    //              {
    //                  ProductId = 5,
    //                  ProductName = "GT MEDIA V7S2X",
    //                  ProductCode = "saltel",
    //                  ReleaseDate = "2021/10/1",
    //                  CategoryId = 1,
    //                  UnitPrice = 39.0m,
    //                  StockQty = 7,
    //                  Description = "Decodificador Satelital for Astra 19.2E Galaxy 19 97W",
    //                  StarRating = 3.9m,
    //                  ImageUrl = "/assets/images/61BHMp1llML._AC_SL1500_.jpg",
    //                //  ShoppingCartItemId = 0
    //              }) ;
         

    //        //Users
    //        bldr.Entity<User>()
    //           .HasData(new
    //           {
    //               UserId = 1,
    //               FirstName = "maikel",
    //               LastName = "ramirez",
    //               Email = "maikelrd@gmail.com",
    //               Password = "maikel"
    //           });

    //        //ShoppingCartItem
    //        bldr.Entity<ShoppingCartItem>()
    //           .HasData(new
    //           {
    //               ShoppingCartItemId = 1,
    //                 UserId = 1,
    //                ProductId = 1,
    //                Amount =2
    //           }); ;

     
    //}
    }
}
