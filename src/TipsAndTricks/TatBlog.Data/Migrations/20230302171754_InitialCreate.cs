using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TatBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostMap_Posts_PostsId",
                table: "PostMap");

            migrationBuilder.DropForeignKey(
                name: "FK_PostMap_Tags_TagsId",
                table: "PostMap");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostMap",
                table: "PostMap");

            migrationBuilder.RenameTable(
                name: "PostMap",
                newName: "PostTags");

            migrationBuilder.RenameIndex(
                name: "IX_PostMap_TagsId",
                table: "PostTags",
                newName: "IX_PostTags_TagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTags",
                table: "PostTags",
                columns: new[] { "PostsId", "TagsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Posts_PostsId",
                table: "PostTags",
                column: "PostsId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Tags_TagsId",
                table: "PostTags",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Posts_PostsId",
                table: "PostTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Tags_TagsId",
                table: "PostTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTags",
                table: "PostTags");

            migrationBuilder.RenameTable(
                name: "PostTags",
                newName: "PostMap");

            migrationBuilder.RenameIndex(
                name: "IX_PostTags_TagsId",
                table: "PostMap",
                newName: "IX_PostMap_TagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostMap",
                table: "PostMap",
                columns: new[] { "PostsId", "TagsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PostMap_Posts_PostsId",
                table: "PostMap",
                column: "PostsId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostMap_Tags_TagsId",
                table: "PostMap",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
