using Microsoft.EntityFrameworkCore.Migrations;

namespace Reserva_B.Migrations
{
    public partial class PersonaDniUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Personas_Dni",
                table: "Personas",
                column: "Dni",
                unique: true,
                filter: "[Dni] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Personas_Dni",
                table: "Personas");
        }
    }
}
