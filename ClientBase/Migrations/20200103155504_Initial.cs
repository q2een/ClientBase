using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClientBase.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxpayerId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsIndividual = table.Column<bool>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Founders",
                columns: table => new
                {
                    FounderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxpayerId = table.Column<long>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Founders", x => x.FounderId);
                });

            migrationBuilder.CreateTable(
                name: "CompanyFounder",
                columns: table => new
                {
                    CompanyId = table.Column<int>(nullable: false),
                    FounderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyFounder", x => new { x.FounderId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_CompanyFounder_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyFounder_Founders_FounderId",
                        column: x => x.FounderId,
                        principalTable: "Founders",
                        principalColumn: "FounderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_TaxpayerId",
                table: "Companies",
                column: "TaxpayerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyFounder_CompanyId",
                table: "CompanyFounder",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Founders_TaxpayerId",
                table: "Founders",
                column: "TaxpayerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyFounder");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Founders");
        }
    }
}
