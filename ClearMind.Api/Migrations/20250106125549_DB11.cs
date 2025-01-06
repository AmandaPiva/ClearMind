using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearMind.Migrations
{
    /// <inheritdoc />
    public partial class DB11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NomeEmocao",
                table: "Emocao",
                type: "character varying(600)",
                maxLength: 600,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "Anotacao",
                table: "Anotacoes",
                type: "character varying(600)",
                maxLength: 600,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Anotacao",
                table: "Anotacoes");

            migrationBuilder.AlterColumn<string>(
                name: "NomeEmocao",
                table: "Emocao",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(600)",
                oldMaxLength: 600);
        }
    }
}
