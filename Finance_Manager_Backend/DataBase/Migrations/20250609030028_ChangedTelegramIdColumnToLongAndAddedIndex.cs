using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance_Manager_Backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTelegramIdColumnToLongAndAddedIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "telegram_id",
                table: "users",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_telegram_id",
                table: "users",
                column: "telegram_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_telegram_id",
                table: "users");

            migrationBuilder.AlterColumn<string>(
                name: "telegram_id",
                table: "users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
