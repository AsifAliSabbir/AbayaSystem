using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFabricTrackingFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAbayaFabricBought",
                table: "OrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSheilaFabricBought",
                table: "OrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAbayaFabricBought",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "IsSheilaFabricBought",
                table: "OrderItems");
        }
    }
}
