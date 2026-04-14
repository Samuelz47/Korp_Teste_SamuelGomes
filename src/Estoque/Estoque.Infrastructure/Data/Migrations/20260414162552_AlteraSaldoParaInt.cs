using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estoque.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlteraSaldoParaInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Saldo",
                table: "Produtos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Saldo",
                table: "Produtos",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
