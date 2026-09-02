using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchToStatusLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "StatusLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
                UPDATE l
                SET l.BranchId = i.BranchId
                FROM StatusLogs l
                INNER JOIN OrderItems i
                    ON i.OrderId = l.OrderId
                    AND i.OrderItemId = l.OrderItemId;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "StatusLogs");
        }
    }
}
