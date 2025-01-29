using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearMind.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteAnotacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "Anotacoes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "Anotacoes");
        }
    }
}
