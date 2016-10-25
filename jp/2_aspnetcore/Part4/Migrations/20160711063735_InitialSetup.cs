using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCorehandson.Migrations
{
    public partial class InitialSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    au_id = table.Column<string>(maxLength: 11, nullable: false),
                    address = table.Column<string>(maxLength: 40, nullable: true),
                    au_fname = table.Column<string>(maxLength: 20, nullable: false),
                    au_lname = table.Column<string>(maxLength: 40, nullable: false),
                    city = table.Column<string>(maxLength: 20, nullable: true),
                    contract = table.Column<bool>(nullable: false),
                    phone = table.Column<string>(maxLength: 12, nullable: false),
                    rowversion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    state = table.Column<string>(maxLength: 2, nullable: true),
                    zip = table.Column<string>(maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authors", x => x.au_id);
                });

            migrationBuilder.CreateTable(
                name: "publishers",
                columns: table => new
                {
                    pub_id = table.Column<string>(maxLength: 4, nullable: false),
                    city = table.Column<string>(maxLength: 20, nullable: true),
                    country = table.Column<string>(maxLength: 30, nullable: true),
                    pub_name = table.Column<string>(maxLength: 40, nullable: true),
                    state = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publishers", x => x.pub_id);
                });

            migrationBuilder.CreateTable(
                name: "stores",
                columns: table => new
                {
                    stor_id = table.Column<string>(maxLength: 4, nullable: false),
                    stor_addr = table.Column<string>(maxLength: 40, nullable: false),
                    city = table.Column<string>(maxLength: 20, nullable: false),
                    state = table.Column<string>(maxLength: 22, nullable: false),
                    stor_name = table.Column<string>(maxLength: 40, nullable: false),
                    zip = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stores", x => x.stor_id);
                });

            migrationBuilder.CreateTable(
                name: "titles",
                columns: table => new
                {
                    title_id = table.Column<string>(maxLength: 6, nullable: false),
                    advance = table.Column<decimal>(nullable: true),
                    notes = table.Column<string>(maxLength: 200, nullable: true),
                    price = table.Column<decimal>(nullable: true),
                    pubdate = table.Column<DateTime>(nullable: false),
                    pub_id = table.Column<string>(maxLength: 4, nullable: false),
                    royalty = table.Column<int>(nullable: true),
                    title = table.Column<string>(maxLength: 80, nullable: false),
                    type = table.Column<string>(maxLength: 12, nullable: false),
                    ytd_sales = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_titles", x => x.title_id);
                    table.ForeignKey(
                        name: "FK_titles_publishers_pub_id",
                        column: x => x.pub_id,
                        principalTable: "publishers",
                        principalColumn: "pub_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sales",
                columns: table => new
                {
                    stor_id = table.Column<string>(maxLength: 4, nullable: false),
                    ord_num = table.Column<string>(maxLength: 20, nullable: false),
                    title_id = table.Column<string>(maxLength: 6, nullable: false),
                    ord_date = table.Column<DateTime>(nullable: false),
                    payterms = table.Column<string>(maxLength: 12, nullable: false),
                    qty = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sales", x => new { x.stor_id, x.ord_num, x.title_id });
                    table.ForeignKey(
                        name: "FK_sales_stores_stor_id",
                        column: x => x.stor_id,
                        principalTable: "stores",
                        principalColumn: "stor_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sales_titles_title_id",
                        column: x => x.title_id,
                        principalTable: "titles",
                        principalColumn: "title_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "titleauthor",
                columns: table => new
                {
                    au_id = table.Column<string>(nullable: false),
                    title_id = table.Column<string>(nullable: false),
                    au_ord = table.Column<byte>(nullable: false),
                    royaltyper = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_titleauthor", x => new { x.au_id, x.title_id });
                    table.ForeignKey(
                        name: "FK_titleauthor_authors_au_id",
                        column: x => x.au_id,
                        principalTable: "authors",
                        principalColumn: "au_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_titleauthor_titles_title_id",
                        column: x => x.title_id,
                        principalTable: "titles",
                        principalColumn: "title_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sales_stor_id",
                table: "sales",
                column: "stor_id");

            migrationBuilder.CreateIndex(
                name: "IX_sales_title_id",
                table: "sales",
                column: "title_id");

            migrationBuilder.CreateIndex(
                name: "IX_titles_pub_id",
                table: "titles",
                column: "pub_id");

            migrationBuilder.CreateIndex(
                name: "IX_titleauthor_au_id",
                table: "titleauthor",
                column: "au_id");

            migrationBuilder.CreateIndex(
                name: "IX_titleauthor_title_id",
                table: "titleauthor",
                column: "title_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sales");

            migrationBuilder.DropTable(
                name: "titleauthor");

            migrationBuilder.DropTable(
                name: "stores");

            migrationBuilder.DropTable(
                name: "authors");

            migrationBuilder.DropTable(
                name: "titles");

            migrationBuilder.DropTable(
                name: "publishers");
        }
    }
}
