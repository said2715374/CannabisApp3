using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CannabisApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "provenances",
                columns: table => new
                {
                    id_provenance = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ville = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pays = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provenances", x => x.id_provenance);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id_role = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id_role);
                });

            migrationBuilder.CreateTable(
                name: "plantes",
                columns: table => new
                {
                    id_plante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    emplacement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    code_qr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_provenance = table.Column<int>(type: "int", nullable: false),
                    etat_sante = table.Column<int>(type: "int", nullable: false),
                    nombre_plantes_actives = table.Column<int>(type: "int", nullable: false),
                    date_expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cree_le = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plantes", x => x.id_plante);
                    table.ForeignKey(
                        name: "FK_plantes_provenances_id_provenance",
                        column: x => x.id_provenance,
                        principalTable: "provenances",
                        principalColumn: "id_provenance",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "utilisateurs",
                columns: table => new
                {
                    id_utilisateur = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_utilisateur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mot_de_passe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_utilisateurs", x => x.id_utilisateur);
                    table.ForeignKey(
                        name: "FK_utilisateurs_roles_id_role",
                        column: x => x.id_role,
                        principalTable: "roles",
                        principalColumn: "id_role",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "inventaire",
                columns: table => new
                {
                    id_inventaire = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_plante = table.Column<int>(type: "int", nullable: false),
                    quantite = table.Column<int>(type: "int", nullable: false),
                    derniere_verification = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventaire", x => x.id_inventaire);
                    table.ForeignKey(
                        name: "FK_inventaire_plantes_id_plante",
                        column: x => x.id_plante,
                        principalTable: "plantes",
                        principalColumn: "id_plante",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "historique_plantes",
                columns: table => new
                {
                    id_historique = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_plante = table.Column<int>(type: "int", nullable: false),
                    action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    id_utilisateur = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historique_plantes", x => x.id_historique);
                    table.ForeignKey(
                        name: "FK_historique_plantes_plantes_id_plante",
                        column: x => x.id_plante,
                        principalTable: "plantes",
                        principalColumn: "id_plante",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_historique_plantes_utilisateurs_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "utilisateurs",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_historique_plantes_id_plante",
                table: "historique_plantes",
                column: "id_plante");

            migrationBuilder.CreateIndex(
                name: "IX_historique_plantes_id_utilisateur",
                table: "historique_plantes",
                column: "id_utilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_inventaire_id_plante",
                table: "inventaire",
                column: "id_plante");

            migrationBuilder.CreateIndex(
                name: "IX_plantes_id_provenance",
                table: "plantes",
                column: "id_provenance");

            migrationBuilder.CreateIndex(
                name: "IX_utilisateurs_id_role",
                table: "utilisateurs",
                column: "id_role");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "historique_plantes");

            migrationBuilder.DropTable(
                name: "inventaire");

            migrationBuilder.DropTable(
                name: "utilisateurs");

            migrationBuilder.DropTable(
                name: "plantes");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "provenances");
        }
    }
}
