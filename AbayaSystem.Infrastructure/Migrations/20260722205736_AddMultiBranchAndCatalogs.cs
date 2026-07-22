using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiBranchAndCatalogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Workers_CutByWorkerId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Workers_StitchedByWorkerId",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_CutByWorkerId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_StitchedByWorkerId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "FabricName",
                table: "OrderItems",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "AssignedWorkshopId",
                table: "OrderItems",
                newName: "TargetBranchId");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Workers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDeliveryDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstimatedDeliveryDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsUrgent",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AssignedSupplierId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FabricId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FabricShopId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HandEmbroideredByWorkerId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HybridProcess",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                columns: new[] { "BranchId", "OrderId" });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsWorkshop = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "Fabrics",
                columns: table => new
                {
                    FabricId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FabricName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fabrics", x => x.FabricId);
                });

            migrationBuilder.CreateTable(
                name: "FabricShops",
                columns: table => new
                {
                    FabricShopId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FabricShopName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FabricShops", x => x.FabricShopId);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workers_BranchId",
                table: "Workers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_AssignedSupplierId",
                table: "OrderItems",
                column: "AssignedSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_BranchId_OrderId",
                table: "OrderItems",
                columns: new[] { "BranchId", "OrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_FabricId",
                table: "OrderItems",
                column: "FabricId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_FabricShopId",
                table: "OrderItems",
                column: "FabricShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_FabricShops_FabricShopId",
                table: "OrderItems",
                column: "FabricShopId",
                principalTable: "FabricShops",
                principalColumn: "FabricShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Fabrics_FabricId",
                table: "OrderItems",
                column: "FabricId",
                principalTable: "Fabrics",
                principalColumn: "FabricId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_BranchId_OrderId",
                table: "OrderItems",
                columns: new[] { "BranchId", "OrderId" },
                principalTable: "Orders",
                principalColumns: new[] { "BranchId", "OrderId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Suppliers_AssignedSupplierId",
                table: "OrderItems",
                column: "AssignedSupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Branches_BranchId",
                table: "Orders",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Branches_BranchId",
                table: "Workers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_FabricShops_FabricShopId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Fabrics_FabricId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_BranchId_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Suppliers_AssignedSupplierId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Branches_BranchId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Branches_BranchId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Fabrics");

            migrationBuilder.DropTable(
                name: "FabricShops");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_BranchId",
                table: "Workers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_AssignedSupplierId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_BranchId_OrderId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_FabricId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_FabricShopId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ActualDeliveryDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "EstimatedDeliveryDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsUrgent",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AssignedSupplierId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "FabricId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "FabricShopId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "HandEmbroideredByWorkerId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "HybridProcess",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "TargetBranchId",
                table: "OrderItems",
                newName: "AssignedWorkshopId");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "OrderItems",
                newName: "FabricName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "OrderId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Workers_CutByWorkerId",
                table: "OrderItems",
                column: "CutByWorkerId",
                principalTable: "Workers",
                principalColumn: "WorkerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Workers_StitchedByWorkerId",
                table: "OrderItems",
                column: "StitchedByWorkerId",
                principalTable: "Workers",
                principalColumn: "WorkerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
