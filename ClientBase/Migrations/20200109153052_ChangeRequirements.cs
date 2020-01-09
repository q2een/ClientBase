using Microsoft.EntityFrameworkCore.Migrations;

namespace ClientBase.Migrations
{
    public partial class ChangeRequirements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyFounder_Companies_CompanyId",
                table: "CompanyFounder");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyFounder_Founders_FounderId",
                table: "CompanyFounder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Founders",
                table: "Founders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "FounderId",
                table: "Founders");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Founders");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Founders",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Founders",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Founders",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Patronymic",
                table: "Founders",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Companies",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsIndividual",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Companies",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Founders",
                table: "Founders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyFounder_Companies_CompanyId",
                table: "CompanyFounder",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyFounder_Founders_FounderId",
                table: "CompanyFounder",
                column: "FounderId",
                principalTable: "Founders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyFounder_Companies_CompanyId",
                table: "CompanyFounder");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyFounder_Founders_FounderId",
                table: "CompanyFounder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Founders",
                table: "Founders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Founders");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Founders");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Founders");

            migrationBuilder.DropColumn(
                name: "Patronymic",
                table: "Founders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "FounderId",
                table: "Founders",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Founders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<bool>(
                name: "IsIndividual",
                table: "Companies",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Founders",
                table: "Founders",
                column: "FounderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyFounder_Companies_CompanyId",
                table: "CompanyFounder",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyFounder_Founders_FounderId",
                table: "CompanyFounder",
                column: "FounderId",
                principalTable: "Founders",
                principalColumn: "FounderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
