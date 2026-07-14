using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkerAuthentication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Workers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Workers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Workers");
        }
    }
}
