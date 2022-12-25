using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeliculaBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class Peliculas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "peliculas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    resumen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    trailer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    enCines = table.Column<bool>(type: "bit", nullable: false),
                    FechaLanzamiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    poster = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_peliculas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "peliculasActores",
                columns: table => new
                {
                    peliculaId = table.Column<int>(type: "int", nullable: false),
                    actorId = table.Column<int>(type: "int", nullable: false),
                    personaje = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_peliculasActores", x => new { x.actorId, x.peliculaId });
                    table.ForeignKey(
                        name: "FK_peliculasActores_Actores_actorId",
                        column: x => x.actorId,
                        principalTable: "Actores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_peliculasActores_peliculas_peliculaId",
                        column: x => x.peliculaId,
                        principalTable: "peliculas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "peliculasCines",
                columns: table => new
                {
                    peliculaId = table.Column<int>(type: "int", nullable: false),
                    cineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_peliculasCines", x => new { x.cineId, x.peliculaId });
                    table.ForeignKey(
                        name: "FK_peliculasCines_Cines_cineId",
                        column: x => x.cineId,
                        principalTable: "Cines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_peliculasCines_peliculas_peliculaId",
                        column: x => x.peliculaId,
                        principalTable: "peliculas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "peliculasGeneros",
                columns: table => new
                {
                    peliculaId = table.Column<int>(type: "int", nullable: false),
                    generoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_peliculasGeneros", x => new { x.generoId, x.peliculaId });
                    table.ForeignKey(
                        name: "FK_peliculasGeneros_Generos_generoId",
                        column: x => x.generoId,
                        principalTable: "Generos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_peliculasGeneros_peliculas_peliculaId",
                        column: x => x.peliculaId,
                        principalTable: "peliculas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_peliculasActores_peliculaId",
                table: "peliculasActores",
                column: "peliculaId");

            migrationBuilder.CreateIndex(
                name: "IX_peliculasCines_peliculaId",
                table: "peliculasCines",
                column: "peliculaId");

            migrationBuilder.CreateIndex(
                name: "IX_peliculasGeneros_peliculaId",
                table: "peliculasGeneros",
                column: "peliculaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "peliculasActores");

            migrationBuilder.DropTable(
                name: "peliculasCines");

            migrationBuilder.DropTable(
                name: "peliculasGeneros");

            migrationBuilder.DropTable(
                name: "peliculas");
        }
    }
}
