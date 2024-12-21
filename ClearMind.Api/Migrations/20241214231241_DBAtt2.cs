using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearMind.Migrations
{
    /// <inheritdoc />
    public partial class DBAtt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contextoEmocao",
                table: "Emocao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "contextoEmocao",
                table: "Emocao",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }
    }
}
