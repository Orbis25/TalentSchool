using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentSchool.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StaticContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Modules");

            migrationBuilder.AddColumn<int>(
                name: "CurrentContentIndex",
                table: "Progresses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StaticContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaticContents_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaticContents_ModuleId",
                table: "StaticContents",
                column: "ModuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaticContents");

            migrationBuilder.DropColumn(
                name: "CurrentContentIndex",
                table: "Progresses");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Modules",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
