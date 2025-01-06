using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearMind.Migrations
{
    /// <inheritdoc />
    public partial class DB12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anotacoes_Emocao_emocaoId",
                table: "Anotacoes");

            migrationBuilder.DropIndex(
                name: "IX_Anotacoes_emocaoId",
                table: "Anotacoes");

            migrationBuilder.DropColumn(
                name: "emocaoId",
                table: "Anotacoes");

            migrationBuilder.CreateIndex(
                name: "IX_Anotacoes_IdEmocao",
                table: "Anotacoes",
                column: "IdEmocao");

            migrationBuilder.AddForeignKey(
                name: "FK_Anotacoes_Emocao_IdEmocao",
                table: "Anotacoes",
                column: "IdEmocao",
                principalTable: "Emocao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anotacoes_Emocao_IdEmocao",
                table: "Anotacoes");

            migrationBuilder.DropIndex(
                name: "IX_Anotacoes_IdEmocao",
                table: "Anotacoes");

            migrationBuilder.AddColumn<int>(
                name: "emocaoId",
                table: "Anotacoes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Anotacoes_emocaoId",
                table: "Anotacoes",
                column: "emocaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Anotacoes_Emocao_emocaoId",
                table: "Anotacoes",
                column: "emocaoId",
                principalTable: "Emocao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
