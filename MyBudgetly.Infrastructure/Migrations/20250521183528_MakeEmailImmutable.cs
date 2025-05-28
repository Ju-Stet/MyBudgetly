using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBudgetly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeEmailImmutable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackupEmail",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackupEmail",
                table: "Users");
        }
    }
}
