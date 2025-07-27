﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodTracker.Migrations
{
    /// <inheritdoc />
    public partial class tustedPersonsName_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrustedPersonsName",
                table: "AspNetUsers",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrustedPersonsName",
                table: "AspNetUsers");
        }
    }
}
