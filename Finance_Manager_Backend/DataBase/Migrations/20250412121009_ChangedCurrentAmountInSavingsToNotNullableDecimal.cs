using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance_Manager_Backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangedCurrentAmountInSavingsToNotNullableDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "current_amount",
                table: "savings",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "current_amount",
                table: "savings",
                type: "decimal(10,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldDefaultValue: 0m);
        }
    }
}
