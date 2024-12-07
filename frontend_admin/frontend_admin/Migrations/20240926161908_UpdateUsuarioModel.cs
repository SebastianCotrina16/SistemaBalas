using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace frontend_admin.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsuarioModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "examenesconfiguracion",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numerodisparos = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_examenesconfiguracion", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "salastiro",
                columns: table => new
                {
                    idsala = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    disponible = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salastiro", x => x.idsala);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    idusuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    correo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    clave = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    restablecer = table.Column<bool>(type: "boolean", nullable: false),
                    confirmado = table.Column<bool>(type: "boolean", nullable: false),
                    token = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    dni = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.idusuario);
                    table.ForeignKey(
                        name: "FK_usuarios_users_dni",
                        column: x => x.dni,
                        principalTable: "users",
                        principalColumn: "dni",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "impactosbala",
                columns: table => new
                {
                    idimpacto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idusuario = table.Column<int>(type: "integer", nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ubicacion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    precision = table.Column<float>(type: "real", nullable: false),
                    rutaimagen = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_impactosbala", x => x.idimpacto);
                    table.ForeignKey(
                        name: "FK_impactosbala_usuarios_idusuario",
                        column: x => x.idusuario,
                        principalTable: "usuarios",
                        principalColumn: "idusuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reportes",
                columns: table => new
                {
                    idreporte = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idusuario = table.Column<int>(type: "integer", nullable: false),
                    fechareporte = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    totalimpactos = table.Column<int>(type: "integer", nullable: false),
                    promedioprecision = table.Column<float>(type: "real", nullable: false),
                    detalles = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reportes", x => x.idreporte);
                    table.ForeignKey(
                        name: "FK_reportes_usuarios_idusuario",
                        column: x => x.idusuario,
                        principalTable: "usuarios",
                        principalColumn: "idusuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservas",
                columns: table => new
                {
                    idreserva = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idusuario = table.Column<int>(type: "integer", nullable: false),
                    idsala = table.Column<int>(type: "integer", nullable: false),
                    fechareserva = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservas", x => x.idreserva);
                    table.ForeignKey(
                        name: "FK_reservas_salastiro_idsala",
                        column: x => x.idsala,
                        principalTable: "salastiro",
                        principalColumn: "idsala",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservas_usuarios_idusuario",
                        column: x => x.idusuario,
                        principalTable: "usuarios",
                        principalColumn: "idusuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "practicas",
                columns: table => new
                {
                    idpractica = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idusuario = table.Column<int>(type: "integer", nullable: false),
                    idreserva = table.Column<int>(type: "integer", nullable: false),
                    fechapractica = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    totaldisparos = table.Column<int>(type: "integer", nullable: false),
                    precisionpromedio = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_practicas", x => x.idpractica);
                    table.ForeignKey(
                        name: "FK_practicas_reservas_idreserva",
                        column: x => x.idreserva,
                        principalTable: "reservas",
                        principalColumn: "idreserva",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_practicas_usuarios_idusuario",
                        column: x => x.idusuario,
                        principalTable: "usuarios",
                        principalColumn: "idusuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detallespracticas",
                columns: table => new
                {
                    iddetallepractica = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idpractica = table.Column<int>(type: "integer", nullable: false),
                    disparonumero = table.Column<int>(type: "integer", nullable: false),
                    precision = table.Column<float>(type: "real", nullable: false),
                    ubicacion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    rutaimagen = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detallespracticas", x => x.iddetallepractica);
                    table.ForeignKey(
                        name: "FK_detallespracticas_practicas_idpractica",
                        column: x => x.idpractica,
                        principalTable: "practicas",
                        principalColumn: "idpractica",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_detallespracticas_idpractica",
                table: "detallespracticas",
                column: "idpractica");

            migrationBuilder.CreateIndex(
                name: "IX_impactosbala_idusuario",
                table: "impactosbala",
                column: "idusuario");

            migrationBuilder.CreateIndex(
                name: "IX_practicas_idreserva",
                table: "practicas",
                column: "idreserva");

            migrationBuilder.CreateIndex(
                name: "IX_practicas_idusuario",
                table: "practicas",
                column: "idusuario");

            migrationBuilder.CreateIndex(
                name: "IX_reportes_idusuario",
                table: "reportes",
                column: "idusuario");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_idsala",
                table: "reservas",
                column: "idsala");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_idusuario",
                table: "reservas",
                column: "idusuario");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_dni",
                table: "usuarios",
                column: "dni");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "detallespracticas");

            migrationBuilder.DropTable(
                name: "examenesconfiguracion");

            migrationBuilder.DropTable(
                name: "impactosbala");

            migrationBuilder.DropTable(
                name: "reportes");

            migrationBuilder.DropTable(
                name: "practicas");

            migrationBuilder.DropTable(
                name: "reservas");

            migrationBuilder.DropTable(
                name: "salastiro");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
