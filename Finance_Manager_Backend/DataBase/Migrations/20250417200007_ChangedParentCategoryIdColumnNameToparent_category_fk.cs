using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance_Manager_Backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangedParentCategoryIdColumnNameToparent_category_fk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_categories_parent_category_fk", 
                table: "categories");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_categories_parent_category_fk",
                table: "categories",
                column: "parent_category_fk",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_categories_parent_category_fk",
                table: "categories");

            migrationBuilder.RenameColumn(
                name: "parent_category_fk",
                table: "categories",
                newName: "ParentCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_categories_parent_category_fk",
                table: "categories",
                newName: "IX_categories_ParentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_categories_ParentCategoryId",
                table: "categories",
                column: "ParentCategoryId",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
