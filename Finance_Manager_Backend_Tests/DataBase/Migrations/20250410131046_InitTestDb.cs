using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance_Manager_Tests.Migrations
{
    /// <inheritdoc />
    public partial class InitTestDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_income = table.Column<bool>(type: "bit", nullable: false),
                    icon_path = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    background_color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    parent_category_fk = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_categories_categories_parent_category_fk",
                        column: x => x.parent_category_fk,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    salt = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    balance = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "savings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    goal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    current_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    user_fk = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_savings", x => x.id);
                    table.ForeignKey(
                        name: "FK_savings_users_user_fk",
                        column: x => x.user_fk,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    category_fk = table.Column<int>(type: "int", nullable: false),
                    inner_category_fk = table.Column<int>(type: "int", nullable: true),
                    user_fk = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_transactions_categories_category_fk",
                        column: x => x.category_fk,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transactions_categories_inner_category_fk",
                        column: x => x.inner_category_fk,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transactions_users_user_fk",
                        column: x => x.user_fk,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_categories_parent_category_fk",
                table: "categories",
                column: "parent_category_fk");

            migrationBuilder.CreateIndex(
                name: "IX_savings_user_fk",
                table: "savings",
                column: "user_fk");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_category_fk",
                table: "transactions",
                column: "category_fk");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_inner_category_fk",
                table: "transactions",
                column: "inner_category_fk");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_user_fk",
                table: "transactions",
                column: "user_fk");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "savings");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
