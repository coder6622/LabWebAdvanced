using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TatBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class SubcriberCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SubscribedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UnsubscribedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UnsubscribedCausal = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FlagIsBlockSubByAdmin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AdminNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscribers");
        }
    }
}
