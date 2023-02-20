using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_Api_Examples.Migrations
{
    public partial class upisAkGodinu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UpisAkGodine",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    datum_ZimskiOvjera = table.Column<DateTime>(type: "datetime2", nullable: true),
                    datum_ZimskiUpis = table.Column<DateTime>(type: "datetime2", nullable: true),
                    godinaStudija = table.Column<int>(type: "int", nullable: false),
                    studentId = table.Column<int>(type: "int", nullable: false),
                    akademskaGodinaId = table.Column<int>(type: "int", nullable: false),
                    cijenaSkolarine = table.Column<float>(type: "real", nullable: true),
                    evidentiraoKorisnikId = table.Column<int>(type: "int", nullable: false),
                    obnovaGodine = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpisAkGodine", x => x.id);
                    table.ForeignKey(
                        name: "FK_UpisAkGodine_AkademskaGodina_akademskaGodinaId",
                        column: x => x.akademskaGodinaId,
                        principalTable: "AkademskaGodina",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UpisAkGodine_KorisnickiNalog_evidentiraoKorisnikId",
                        column: x => x.evidentiraoKorisnikId,
                        principalTable: "KorisnickiNalog",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UpisAkGodine_Student_studentId",
                        column: x => x.studentId,
                        principalTable: "Student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UpisAkGodine_akademskaGodinaId",
                table: "UpisAkGodine",
                column: "akademskaGodinaId");

            migrationBuilder.CreateIndex(
                name: "IX_UpisAkGodine_evidentiraoKorisnikId",
                table: "UpisAkGodine",
                column: "evidentiraoKorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_UpisAkGodine_studentId",
                table: "UpisAkGodine",
                column: "studentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpisAkGodine");
        }
    }
}
