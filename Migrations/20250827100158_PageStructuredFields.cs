using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GovSite.Migrations
{
    /// <inheritdoc />
    public partial class PageStructuredFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HtmlContent",
                table: "Pages",
                newName: "ThemePrimary");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "Pages",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "BlocksJson",
                table: "Pages",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HeroBgColor",
                table: "Pages",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HeroImageUrl",
                table: "Pages",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HeroSubtitle",
                table: "Pages",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HeroTextColor",
                table: "Pages",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HeroTitle",
                table: "Pages",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "BlocksJson",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "HeroBgColor",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "HeroImageUrl",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "HeroSubtitle",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "HeroTextColor",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "HeroTitle",
                table: "Pages");

            migrationBuilder.RenameColumn(
                name: "ThemePrimary",
                table: "Pages",
                newName: "HtmlContent");
        }
    }
}
