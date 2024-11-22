using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearMind.Migrations
{
    /// <inheritdoc />
    public partial class DbInit4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NomeEmocao",
                table: "Emocao",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "decisao",
                table: "Emocao",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "decisao",
                table: "Emocao");

            migrationBuilder.AlterColumn<string>(
                name: "NomeEmocao",
                table: "Emocao",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);
        }
    }
}
