﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance_Manager_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedTelegramIdColumnToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "telegram_id",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "telegram_id",
                table: "users");
        }
    }
}
