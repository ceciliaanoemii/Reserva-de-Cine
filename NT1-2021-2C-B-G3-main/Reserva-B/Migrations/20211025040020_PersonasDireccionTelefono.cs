using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reserva_B.Migrations
{
    public partial class PersonasDireccionTelefono : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientesDireccionTelefonosId",
                table: "Telefonos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClientesDireccionTelefonos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dni = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 60, nullable: false),
                    Apellido = table.Column<string>(maxLength: 60, nullable: false),
                    DireccionId = table.Column<int>(nullable: true),
                    FechaAlta = table.Column<DateTime>(nullable: false),
                    Password = table.Column<string>(maxLength: 15, nullable: false),
                    IdDireccion = table.Column<int>(nullable: false),
                    Calle = table.Column<string>(maxLength: 60, nullable: false),
                    CodPostal = table.Column<int>(nullable: false),
                    Numero = table.Column<int>(nullable: false),
                    Piso = table.Column<int>(nullable: true),
                    Departamento = table.Column<string>(maxLength: 3, nullable: true),
                    PersonaId = table.Column<int>(nullable: false),
                    TelefonoId = table.Column<int>(nullable: false),
                    CodArea = table.Column<int>(nullable: false),
                    NumeroTel = table.Column<int>(nullable: false),
                    Principal = table.Column<bool>(nullable: false),
                    Tipo = table.Column<int>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
