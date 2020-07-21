using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentCardIssuers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Pattern = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCardIssuers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "date", nullable: false),
                    CardIssuerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentCards_PaymentCardIssuers_CardIssuerId",
                        column: x => x.CardIssuerId,
                        principalTable: "PaymentCardIssuers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Identifier = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    Method = table.Column<string>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    PaymentCardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentCards_PaymentCardId",
                        column: x => x.PaymentCardId,
                        principalTable: "PaymentCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "GBP", "Pound sterling" },
                    { 2, "USD", "United States Dollar" }
                });

            migrationBuilder.InsertData(
                table: "PaymentCardIssuers",
                columns: new[] { "Id", "Name", "Pattern" },
                values: new object[,]
                {
                    { 1, "AmericanExpress", "^3[47][0-9]{5,}$" },
                    { 2, "Visa", "^4[0-9]{6,}$" },
                    { 3, "Mastercard", "^5[1-5][0-9]{5,}|222[1-9][0-9]{3,}|22[3-9][0-9]{4,}|2[3-6][0-9]{5,}|27[01][0-9]{4,}|2720[0-9]{3,}$" }
                });

            migrationBuilder.InsertData(
                table: "PaymentCards",
                columns: new[] { "Id", "CardIssuerId", "ExpiryDate", "Number" },
                values: new object[] { 1, 1, new DateTime(2020, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "4111111111111111" });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "CurrencyId", "Date", "Identifier", "Method", "PaymentCardId", "Status" },
                values: new object[] { 1, 1243, 1, new DateTime(2001, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "63d46c20-61bf-4c47-bfdc-6f08bc217406", "Card", 1, "success" });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Code",
                table: "Currencies",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCards_CardIssuerId",
                table: "PaymentCards",
                column: "CardIssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCards_Number",
                table: "PaymentCards",
                column: "Number",
                unique: true,
                filter: "[Number] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CurrencyId",
                table: "Payments",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Identifier",
                table: "Payments",
                column: "Identifier",
                unique: true,
                filter: "[Identifier] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentCardId",
                table: "Payments",
                column: "PaymentCardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "PaymentCards");

            migrationBuilder.DropTable(
                name: "PaymentCardIssuers");
        }
    }
}
