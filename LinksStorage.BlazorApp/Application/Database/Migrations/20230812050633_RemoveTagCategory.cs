using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinksStorage.BlazorApp.Application.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTagCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_links_users_user_id",
                table: "links");

            migrationBuilder.DropForeignKey(
                name: "fk_tags_users_user_id",
                table: "tags");

            migrationBuilder.DropTable(
                name: "tag_tag_category");

            migrationBuilder.DropTable(
                name: "tag_categories");

            migrationBuilder.DropIndex(
                name: "ix_tags_user_id",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "tags");

            migrationBuilder.AddForeignKey(
                name: "fk_links_users_user_id",
                table: "links",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_links_users_user_id",
                table: "links");

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "tags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tag_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tag_categories", x => x.id);
                    table.ForeignKey(
                        name: "fk_tag_categories_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tag_tag_category",
                columns: table => new
                {
                    categories_id = table.Column<int>(type: "int", nullable: false),
                    tags_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tag_tag_category", x => new { x.categories_id, x.tags_id });
                    table.ForeignKey(
                        name: "fk_tag_tag_category_tag_categories_categories_id",
                        column: x => x.categories_id,
                        principalTable: "tag_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tag_tag_category_tags_tags_id",
                        column: x => x.tags_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tags_user_id",
                table: "tags",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_tag_categories_user_id",
                table: "tag_categories",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_tag_tag_category_tags_id",
                table: "tag_tag_category",
                column: "tags_id");

            migrationBuilder.AddForeignKey(
                name: "fk_links_users_user_id",
                table: "links",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_tags_users_user_id",
                table: "tags",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
