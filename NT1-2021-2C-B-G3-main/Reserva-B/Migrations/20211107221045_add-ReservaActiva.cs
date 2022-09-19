using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reserva_B.Migrations
{
    public partial class addReservaActiva : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Telefonos_ClientesDireccionTelefonos_ClientesDireccionTelefonosId",
                table: "Telefonos");

            migrationBuilder.DropTable(
                name: "ClientesDireccionTelefonos");

            migrationBuilder.DropIndex(
                name: "IX_Telefonos_ClientesDireccionTelefonosId",
                table: "Telefonos");

            migrationBuilder.DropColumn(
                name: "ClientesDireccionTelefonosId",
                table: "Telefonos");

            migrationBuilder.AddColumn<bool>(
                name: "ReservaActiva",
                table: "Reservas",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservaActiva",
                table: "Reservas");

            migrationBuilder.AddColumn<int>(
                name: "ClientesDireccionTelefonosId",
                table: "Telefonos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClientesDireccionTelefonos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Apellido = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Calle = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    CodArea = table.Column<int>(type: "int", nullable: false),
                    CodPostal = table.Column<int>(type: "int", nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    DireccionId = table.Column<int>(type: "int", nullable: true),
                    Dni = table.Column<int>(type: "int", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdDireccion = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    NumeroTel = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    PersonaId = table.Column<int>(type: "int", nullable: false),
                    Piso = table.Column<int>(type: "int", nullable: true),
                    Principal = table.Column<bool>(type: "bit", nullable: false),
                    TelefonoId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientesDireccionTelefonos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientesDireccionTelefonos_Direcciones_DireccionId",
                        column: x => x.DireccionId,
                        principalTable: "Direcciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientesDireccionTelefonos_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Telefonos_ClientesDireccionTelefonosId",
                table: "Telefonos",
                column: "ClientesDireccionTelefonosId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientesDireccionTelefonos_DireccionId",
                table: "ClientesDireccionTelefonos",
                column: "DireccionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientesDireccionTelefonos_PersonaId",
                table: "ClientesDireccionTelefonos",
                column: "PersonaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Telefonos_ClientesDireccionTelefonos_ClientesDireccionTelefonosId",
                table: "Telefonos",
                column: "ClientesDireccionTelefonosId",
                principalTable: "ClientesDireccionTelefonos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
