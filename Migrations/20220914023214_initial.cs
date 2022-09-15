using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_Categories_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQty = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StarRating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartItems",
                columns: table => new
                {
                    ShoppingCartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItems", x => x.ShoppingCartItemId);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "DepartmentName" },
                values: new object[,]
                {
                    { 1, "Electronic" },
                    { 2, "Computer" },
                    { 3, "Food & Grocery" },
                    { 4, "Books" },
                    { 5, "Movies, Music & Games" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "FirstName", "LastName", "Password" },
                values: new object[] { 1, "maikelrd@gmail.com", "maikel", "ramirez", "maikel" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "DepartmentId" },
                values: new object[,]
                {
                    { 1, "Televisors", 1 },
                    { 4, "Video Games", 1 },
                    { 2, "Computer&accessories", 2 },
                    { 3, "Grocery", 3 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "Description", "ImageUrl", "ProductCode", "ProductName", "ReleaseDate", "StarRating", "StockQty", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, "Tv Samsung 32'' ", "/assets/images/711qRHKMRTL._AC_UY218_.jpg", "TV1234", "TV Samsung", "2020/10,/18", 4.3m, 10, 245m },
                    { 5, 1, "Decodificador Satelital for Astra 19.2E Galaxy 19 97W", "/assets/images/61BHMp1llML._AC_SL1500_.jpg", "saltel", "GT MEDIA V7S2X", "2021/10/1", 3.9m, 7, 39.0m },
                    { 2, 2, "Laptop dell i7 ", "/assets/images/Dell-Laptop.png", "laptopdell", "Laptop Dell", "2021/10,/18", 4.5m, 20, 831m },
                    { 4, 2, "Wireless Color All-in-One", "/assets/images/61UdeL7aO-L._AC_SL1500_.jpg", "hp2755e", "HP Deskjet 2755e", "2021/24/4", 4.1m, 4, 76.0m },
                    { 3, 3, "Apples bag 5 pounds ", "/assets/images/71gcGpnE02L._AC_UL320_.jpg", "apple012", "Apples bag", "2022/10/18", 4.0m, 2, 9m }
                });

            migrationBuilder.InsertData(
                table: "ShoppingCartItems",
                columns: new[] { "ShoppingCartItemId", "Amount", "ProductId", "UserId" },
                values: new object[] { 1, 2, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DepartmentId",
                table: "Categories",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ProductId",
                table: "ShoppingCartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_UserId",
                table: "ShoppingCartItems",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingCartItems");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
