using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReadymadeSheila : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SheilaShops",
                columns: table => new
                {
                    SheilaShopID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SheilaShopName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheilaShops", x => x.SheilaShopID);
                    table.ForeignKey(
                        name: "FK_SheilaShops_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SheilaTrans",
                columns: table => new
                {
                    SheilaTranID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SheilaShopID = table.Column<int>(type: "int", nullable: false),
                    OrderID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheilaTrans", x => x.SheilaTranID);
                    table.ForeignKey(
                        name: "FK_SheilaTrans_SheilaShops_SheilaShopID",
                        column: x => x.SheilaShopID,
                        principalTable: "SheilaShops",
                        principalColumn: "SheilaShopID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SheilaShops_BranchID",
                table: "SheilaShops",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_SheilaTrans_SheilaShopID",
                table: "SheilaTrans",
                column: "SheilaShopID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SheilaTrans");

            migrationBuilder.DropTable(
                name: "SheilaShops");
        }
    }
}
