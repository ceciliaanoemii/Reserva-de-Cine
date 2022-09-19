using Microsoft.EntityFrameworkCore.Migrations;

namespace Reserva_B.Migrations
{
    public partial class AddPeliculaFoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "Peliculas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Peliculas");
        }
    }
}
