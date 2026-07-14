using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialBoutiqueSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOrdered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TypeOfOrder = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DepositPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    WorkerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedRoles = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.WorkerId);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelTextDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FabricName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsShopProvidingFabric = table.Column<bool>(type: "bit", nullable: false),
                    SelectedSheilaSize = table.Column<int>(type: "int", nullable: false),
                    IsReadyMadeAlteration = table.Column<bool>(type: "bit", nullable: false),
                    AlterationNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedWorkshopId = table.Column<int>(type: "int", nullable: false),
                    CutByWorkerId = table.Column<int>(type: "int", nullable: true),
                    StitchedByWorkerId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Workers_CutByWorkerId",
                        column: x => x.CutByWorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Workers_StitchedByWorkerId",
                        column: x => x.StitchedByWorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_CutByWorkerId",
                table: "OrderItems",
                column: "CutByWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_StitchedByWorkerId",
                table: "OrderItems",
                column: "StitchedByWorkerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Workers");
        }
    }
}
