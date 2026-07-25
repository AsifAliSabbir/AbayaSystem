using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AbayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalWorkersAndMultiItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOrdered",
                table: "Orders",
                newName: "OrderDate");

            migrationBuilder.AddColumn<bool>(
                name: "BuyFabricForExternal",
                table: "OrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExternalWorkerId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderBranchId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderId1",
                table: "OrderItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExternalWorkers",
                columns: table => new
                {
                    ExternalWorkerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupportedType = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalWorkers", x => x.ExternalWorkerId);
                });

            migrationBuilder.InsertData(
                table: "ExternalWorkers",
                columns: new[] { "ExternalWorkerId", "IsActive", "Name", "Phone", "SupportedType" },
                values: new object[,]
                {
                    { 1, true, "Rubel", "+971500000001", 2 },
                    { 2, true, "Saiful", "+971500000002", 2 },
                    { 3, true, "Alim Emb", "+971500000003", 1 },
                    { 4, true, "Computer Emb1", "+971500000003", 1 },
                    { 5, true, "Computer Emb2", "+971500000003", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ExternalWorkerId",
                table: "OrderItems",
                column: "ExternalWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderBranchId_OrderId1",
                table: "OrderItems",
                columns: new[] { "OrderBranchId", "OrderId1" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ExternalWorkers_ExternalWorkerId",
                table: "OrderItems",
                column: "ExternalWorkerId",
                principalTable: "ExternalWorkers",
                principalColumn: "ExternalWorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderBranchId_OrderId1",
                table: "OrderItems",
                columns: new[] { "OrderBranchId", "OrderId1" },
                principalTable: "Orders",
                principalColumns: new[] { "BranchId", "OrderId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ExternalWorkers_ExternalWorkerId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderBranchId_OrderId1",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "ExternalWorkers");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ExternalWorkerId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderBranchId_OrderId1",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "BuyFabricForExternal",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ExternalWorkerId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderBranchId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "Orders",
                newName: "DateOrdered");
        }
    }
}
