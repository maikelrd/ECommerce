﻿// <auto-generated />
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ECommerce.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ECommerce.Data.Entities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.HasKey("CategoryId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            CategoryName = "Televisors",
                            DepartmentId = 1
                        },
                        new
                        {
                            CategoryId = 2,
                            CategoryName = "Computer&accessories",
                            DepartmentId = 2
                        },
                        new
                        {
                            CategoryId = 3,
                            CategoryName = "Grocery",
                            DepartmentId = 3
                        });
                });

            modelBuilder.Entity("ECommerce.Data.Entities.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DepartmentName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DepartmentId");

                    b.ToTable("Departments");

                    b.HasData(
                        new
                        {
                            DepartmentId = 1,
                            DepartmentName = "Electronic"
                        },
                        new
                        {
                            DepartmentId = 2,
                            DepartmentName = "Computer"
                        },
                        new
                        {
                            DepartmentId = 3,
                            DepartmentName = "Food & Grocery"
                        },
                        new
                        {
                            DepartmentId = 4,
                            DepartmentName = "Books"
                        },
                        new
                        {
                            DepartmentId = 5,
                            DepartmentName = "Movies, Music & Games"
                        });
                });

            modelBuilder.Entity("ECommerce.Data.Entities.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReleaseDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("StarRating")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("StockQty")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            CategoryId = 1,
                            Description = "Tv Samsung 32'' ",
                            ImageUrl = "",
                            ProductCode = "TV1234",
                            ProductName = "TV Samsung",
                            ReleaseDate = "2020/10,/18",
                            StarRating = 4.3m,
                            StockQty = 10,
                            UnitPrice = 245m
                        },
                        new
                        {
                            ProductId = 2,
                            CategoryId = 2,
                            Description = "Laptop dell i7 ",
                            ImageUrl = "",
                            ProductCode = "laptopdell",
                            ProductName = "Laptop Dell",
                            ReleaseDate = "2021/10,/18",
                            StarRating = 4.5m,
                            StockQty = 20,
                            UnitPrice = 831m
                        },
                        new
                        {
                            ProductId = 3,
                            CategoryId = 3,
                            Description = "Apples bag 5 pounds ",
                            ImageUrl = "",
                            ProductCode = "apple012",
                            ProductName = "Apples bag",
                            ReleaseDate = "2022/10/18",
                            StarRating = 4.0m,
                            StockQty = 2,
                            UnitPrice = 9m
                        },
                        new
                        {
                            ProductId = 4,
                            CategoryId = 2,
                            Description = "Wireless Color All-in-One",
                            ImageUrl = "",
                            ProductCode = "hp2755e",
                            ProductName = "HP Deskjet 2755e",
                            ReleaseDate = "2021/24/4",
                            StarRating = 4.1m,
                            StockQty = 4,
                            UnitPrice = 76.0m
                        });
                });

            modelBuilder.Entity("ECommerce.Data.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Password = "Maikel",
                            UserName = "Maikel"
                        });
                });

            modelBuilder.Entity("ECommerce.Data.Entities.Category", b =>
                {
                    b.HasOne("ECommerce.Data.Entities.Department", null)
                        .WithMany("Categories")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ECommerce.Data.Entities.Product", b =>
                {
                    b.HasOne("ECommerce.Data.Entities.Category", null)
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ECommerce.Data.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("ECommerce.Data.Entities.Department", b =>
                {
                    b.Navigation("Categories");
                });
#pragma warning restore 612, 618
        }
    }
}
