using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance_Manager_Backend_Tests.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class AddedCurrencyToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "primary_currency",
                table: "users",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "USD");

            migrationBuilder.AddColumn<string>(
                name: "secondary_currency_1",
                table: "users",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "secondary_currency_2",
                table: "users",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "primary_currency",
                table: "users");

            migrationBuilder.DropColumn(
                name: "secondary_currency_1",
                table: "users");

            migrationBuilder.DropColumn(
                name: "secondary_currency_2",
                table: "users");
        }
    }
}
