using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinksStorage.BlazorApp.Application.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveExplicitNavigationToLinksFromTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_link_tag_links_links_id",
                table: "link_tag");

            migrationBuilder.RenameColumn(
                name: "links_id",
                table: "link_tag",
                newName: "link_id");

            migrationBuilder.AddForeignKey(
                name: "fk_link_tag_links_link_id",
                table: "link_tag",
                column: "link_id",
                principalTable: "links",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_link_tag_links_link_id",
                table: "link_tag");

            migrationBuilder.RenameColumn(
                name: "link_id",
                table: "link_tag",
                newName: "links_id");

            migrationBuilder.AddForeignKey(
                name: "fk_link_tag_links_links_id",
                table: "link_tag",
                column: "links_id",
                principalTable: "links",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
