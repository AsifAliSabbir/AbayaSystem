using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSheilaReturns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReturnOfSheilaTranID",
                table: "SheilaTrans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SheilaTrans_ReturnOfSheilaTranID",
                table: "SheilaTrans",
                column: "ReturnOfSheilaTranID");

            migrationBuilder.AddForeignKey(
                name: "FK_SheilaTrans_SheilaTrans_ReturnOfSheilaTranID",
                table: "SheilaTrans",
                column: "ReturnOfSheilaTranID",
                principalTable: "SheilaTrans",
                principalColumn: "SheilaTranID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SheilaTrans_SheilaTrans_ReturnOfSheilaTranID",
                table: "SheilaTrans");

            migrationBuilder.DropIndex(
                name: "IX_SheilaTrans_ReturnOfSheilaTranID",
                table: "SheilaTrans");

            migrationBuilder.DropColumn(
                name: "ReturnOfSheilaTranID",
                table: "SheilaTrans");
        }
    }
}
