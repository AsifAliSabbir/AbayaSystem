using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AbayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LengthAbayaFront = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LengthAbayaBack = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LengthSleeve = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WidthArmHole = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WidthSleeveOpening = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WidthShoulder = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WidthBody = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WidthBottom = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ButtonType = table.Column<int>(type: "int", nullable: false),
                    NumberOfButtons = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

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
                name: "StatusLogs",
                columns: table => new
                {
                    StatusLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    PreviousState = table.Column<int>(type: "int", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false),
                    PreviousWorkerId = table.Column<int>(type: "int", nullable: true),
                    CurrentWorkerId = table.Column<int>(type: "int", nullable: true),
                    TimeOfEvent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusLogs", x => x.StatusLogId);
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

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    WorkerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedRoles = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.WorkerId);
                    table.ForeignKey(
                        name: "FK_Workers_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DepositPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceDue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => new { x.BranchId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_Orders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelTextDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FabricShopId = table.Column<int>(type: "int", nullable: true),
                    FabricId = table.Column<int>(type: "int", nullable: true),
                    ColorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedSheilaSize = table.Column<int>(type: "int", nullable: false),
                    IsReadyMadeAlteration = table.Column<bool>(type: "bit", nullable: false),
                    AlterationNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetBranchId = table.Column<int>(type: "int", nullable: false),
                    AssignedSupplierId = table.Column<int>(type: "int", nullable: true),
                    CutByWorkerId = table.Column<int>(type: "int", nullable: true),
                    StitchedByWorkerId = table.Column<int>(type: "int", nullable: true),
                    HandEmbroideredByWorkerId = table.Column<int>(type: "int", nullable: true),
                    TypeOfOrder = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsAbayaFabricBought = table.Column<bool>(type: "bit", nullable: false),
                    IsSheilaFabricBought = table.Column<bool>(type: "bit", nullable: false),
                    ActualDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    ExternalWorkerId = table.Column<int>(type: "int", nullable: true),
                    BuyFabricForExternal = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HandEmbRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    rawFabricEmb = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => new { x.BranchId, x.OrderId, x.OrderItemId });
                    table.ForeignKey(
                        name: "FK_OrderItems_ExternalWorkers_ExternalWorkerId",
                        column: x => x.ExternalWorkerId,
                        principalTable: "ExternalWorkers",
                        principalColumn: "ExternalWorkerId");
                    table.ForeignKey(
                        name: "FK_OrderItems_FabricShops_FabricShopId",
                        column: x => x.FabricShopId,
                        principalTable: "FabricShops",
                        principalColumn: "FabricShopId");
                    table.ForeignKey(
                        name: "FK_OrderItems_Fabrics_FabricId",
                        column: x => x.FabricId,
                        principalTable: "Fabrics",
                        principalColumn: "FabricId");
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_BranchId_OrderId",
                        columns: x => new { x.BranchId, x.OrderId },
                        principalTable: "Orders",
                        principalColumns: new[] { "BranchId", "OrderId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Suppliers_AssignedSupplierId",
                        column: x => x.AssignedSupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateTable(
                name: "ExternalVendorJobs",
                columns: table => new
                {
                    ExternalVendorJobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExternalWorkerId = table.Column<int>(type: "int", nullable: false),
                    Stage = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DispatchedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReturnedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DispatchNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ReturnNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DispatchedByWorkerId = table.Column<int>(type: "int", nullable: true),
                    ReceivedByWorkerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalVendorJobs", x => x.ExternalVendorJobId);
                    table.ForeignKey(
                        name: "FK_ExternalVendorJobs_ExternalWorkers_ExternalWorkerId",
                        column: x => x.ExternalWorkerId,
                        principalTable: "ExternalWorkers",
                        principalColumn: "ExternalWorkerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalVendorJobs_OrderItems_BranchId_OrderId_OrderItemId",
                        columns: x => new { x.BranchId, x.OrderId, x.OrderItemId },
                        principalTable: "OrderItems",
                        principalColumns: new[] { "BranchId", "OrderId", "OrderItemId" },
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_ExternalVendorJobs_BranchId_OrderId_OrderItemId",
                table: "ExternalVendorJobs",
                columns: new[] { "BranchId", "OrderId", "OrderItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalVendorJobs_ExternalWorkerId",
                table: "ExternalVendorJobs",
                column: "ExternalWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_AssignedSupplierId",
                table: "OrderItems",
                column: "AssignedSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ExternalWorkerId",
                table: "OrderItems",
                column: "ExternalWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_FabricId",
                table: "OrderItems",
                column: "FabricId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_FabricShopId",
                table: "OrderItems",
                column: "FabricShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_BranchId",
                table: "Workers",
                column: "BranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalVendorJobs");

            migrationBuilder.DropTable(
                name: "StatusLogs");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ExternalWorkers");

            migrationBuilder.DropTable(
                name: "FabricShops");

            migrationBuilder.DropTable(
                name: "Fabrics");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
