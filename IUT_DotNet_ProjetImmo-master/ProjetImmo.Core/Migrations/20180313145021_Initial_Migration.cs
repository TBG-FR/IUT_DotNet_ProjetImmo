using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ProjetImmo.Core.Migrations
{
    public partial class Initial_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    City = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    PostalAddress = table.Column<string>(nullable: true),
                    ZIP = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Keyword",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keyword", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddressID = table.Column<int>(nullable: true),
                    Firstname = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Person_Address_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Address",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Estate",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddressID = table.Column<int>(nullable: true),
                    AnnualCharges = table.Column<double>(nullable: false),
                    FloorCount = table.Column<int>(nullable: false),
                    FloorNumber = table.Column<int>(nullable: false),
                    OwnerID = table.Column<int>(nullable: true),
                    PropertyTax = table.Column<double>(nullable: false),
                    ReferentID = table.Column<int>(nullable: true),
                    RoomsCount = table.Column<int>(nullable: false),
                    Surface = table.Column<double>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estate", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Estate_Address_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Address",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Estate_Person_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "Person",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Estate_Person_ReferentID",
                        column: x => x.ReferentID,
                        principalTable: "Person",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EstateKeyword",
                columns: table => new
                {
                    EstateID = table.Column<int>(nullable: false),
                    KeywordID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstateKeyword", x => new { x.EstateID, x.KeywordID });
                    table.ForeignKey(
                        name: "FK_EstateKeyword_Estate_EstateID",
                        column: x => x.EstateID,
                        principalTable: "Estate",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstateKeyword_Keyword_KeywordID",
                        column: x => x.KeywordID,
                        principalTable: "Keyword",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Base64 = table.Column<byte>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    RelatedEstateID = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Picture_Estate_RelatedEstateID",
                        column: x => x.RelatedEstateID,
                        principalTable: "Estate",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Furnished = table.Column<bool>(nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Fees = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    RelatedCustomerID = table.Column<int>(nullable: true),
                    RelatedEstateID = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    TransactionDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Transaction_Person_RelatedCustomerID",
                        column: x => x.RelatedCustomerID,
                        principalTable: "Person",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Estate_RelatedEstateID",
                        column: x => x.RelatedEstateID,
                        principalTable: "Estate",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estate_AddressID",
                table: "Estate",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Estate_OwnerID",
                table: "Estate",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Estate_ReferentID",
                table: "Estate",
                column: "ReferentID");

            migrationBuilder.CreateIndex(
                name: "IX_EstateKeyword_KeywordID",
                table: "EstateKeyword",
                column: "KeywordID");

            migrationBuilder.CreateIndex(
                name: "IX_Person_AddressID",
                table: "Person",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Picture_RelatedEstateID",
                table: "Picture",
                column: "RelatedEstateID");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_RelatedCustomerID",
                table: "Transaction",
                column: "RelatedCustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_RelatedEstateID",
                table: "Transaction",
                column: "RelatedEstateID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstateKeyword");

            migrationBuilder.DropTable(
                name: "Picture");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Keyword");

            migrationBuilder.DropTable(
                name: "Estate");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
