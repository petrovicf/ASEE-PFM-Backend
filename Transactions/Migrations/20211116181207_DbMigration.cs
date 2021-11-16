using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Transactions.Migrations
{
    public partial class DbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ParentCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    BeneficiaryName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Direction = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Currency = table.Column<string>(type: "character(3)", fixedLength: true, maxLength: 3, nullable: false),
                    Mcc = table.Column<int>(type: "integer", nullable: true),
                    Kind = table.Column<string>(type: "text", nullable: false),
                    Catcode = table.Column<string>(type: "character varying(32)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transactions_categories_Catcode",
                        column: x => x.Catcode,
                        principalTable: "categories",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transactions_Catcode",
                table: "transactions",
                column: "Catcode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
