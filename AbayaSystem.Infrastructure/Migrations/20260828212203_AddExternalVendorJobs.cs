using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalVendorJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalVendorJobs",
                columns: table => new
                {
                    ExternalVendorJobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_ExternalVendorJobs_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "OrderItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalVendorJobs_ExternalWorkerId",
                table: "ExternalVendorJobs",
                column: "ExternalWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalVendorJobs_OrderItemId",
                table: "ExternalVendorJobs",
                column: "OrderItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalVendorJobs");
        }
    }
}
