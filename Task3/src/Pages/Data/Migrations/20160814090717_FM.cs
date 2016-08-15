using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Pages.Data.Migrations
{
    public partial class FM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Page",
                columns: table => new
                {
                    PageID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsSelected = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    UrlName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Page", x => x.PageID);
                });

            migrationBuilder.CreateTable(
                name: "NavLink",
                columns: table => new
                {
                    NLId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NavLinkTitle = table.Column<string>(nullable: true),
                    PageId = table.Column<int>(nullable: false),
                    ParentLinkId = table.Column<int>(nullable: false),
                    Position = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavLink", x => x.NLId);
                    table.ForeignKey(
                        name: "FK_NavLink_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "Page",
                        principalColumn: "PageID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelatedPages",
                columns: table => new
                {
                    RowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstPageId = table.Column<int>(nullable: false),
                    SecondPageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedPages", x => x.RowId);
                    table.ForeignKey(
                        name: "FK_RelatedPages_Page_FirstPageId",
                        column: x => x.FirstPageId,
                        principalTable: "Page",
                        principalColumn: "PageID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RelatedPages_Page_SecondPageId",
                        column: x => x.SecondPageId,
                        principalTable: "Page",
                        principalColumn: "PageID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NavLink_PageId",
                table: "NavLink",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedPages_FirstPageId",
                table: "RelatedPages",
                column: "FirstPageId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedPages_SecondPageId",
                table: "RelatedPages",
                column: "SecondPageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NavLink");

            migrationBuilder.DropTable(
                name: "RelatedPages");

            migrationBuilder.DropTable(
                name: "Page");
        }
    }
}
