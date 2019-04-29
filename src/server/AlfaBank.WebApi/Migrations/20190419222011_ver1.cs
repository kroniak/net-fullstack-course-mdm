using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable ArgumentsStyleStringLiteral
#pragma warning disable 1591

namespace AlfaBank.WebApi.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class ver1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(nullable: false),
                    Surname = table.Column<string>(nullable: true),
                    Firstname = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(nullable: false),
                    CardNumber = table.Column<string>(maxLength: 19, nullable: false),
                    CardName = table.Column<string>(nullable: false),
                    Currency = table.Column<int>(nullable: false),
                    CardType = table.Column<int>(nullable: false),
                    DtOpenCard = table.Column<DateTime>(nullable: false),
                    ValidityYear = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Sum = table.Column<decimal>(nullable: false),
                    CardFromNumber = table.Column<string>(nullable: true),
                    CardToNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] {"Id", "Birthday", "Firstname", "Surname", "UserName"},
                values: new object[] {1, null, null, null, "admin@admin.ru"});

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] {"Id", "Birthday", "Firstname", "Surname", "UserName"},
                values: new object[] {2, null, null, null, "user@user.ru"});

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[]
                    {"Id", "CardName", "CardNumber", "CardType", "Currency", "DtOpenCard", "UserId", "ValidityYear"},
                values: new object[]
                {
                    1, "my salary", "6271190189011743", 2, 0, new DateTime(2017, 4, 20, 0, 0, 0, 0, DateTimeKind.Local),
                    1, 3
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[]
                    {"Id", "CardName", "CardNumber", "CardType", "Currency", "DtOpenCard", "UserId", "ValidityYear"},
                values: new object[]
                {
                    2, "my salary", "6762302693240520", 3, 0, new DateTime(2017, 4, 20, 0, 0, 0, 0, DateTimeKind.Local),
                    1, 3
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[]
                    {"Id", "CardName", "CardNumber", "CardType", "Currency", "DtOpenCard", "UserId", "ValidityYear"},
                values: new object[]
                {
                    3, "my debt", "4083967629457310", 2, 2, new DateTime(2017, 4, 20, 0, 0, 0, 0, DateTimeKind.Local),
                    1, 3
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[]
                    {"Id", "CardName", "CardNumber", "CardType", "Currency", "DtOpenCard", "UserId", "ValidityYear"},
                values: new object[]
                {
                    4, "for my lovely wife", "5101265622568232", 1, 1,
                    new DateTime(2017, 4, 20, 0, 0, 0, 0, DateTimeKind.Local), 1, 3
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] {"Id", "CardFromNumber", "CardId", "CardToNumber", "DateTime", "Sum"},
                values: new object[]
                {
                    1, null, 1, "6271190189011743",
                    new DateTime(2019, 4, 20, 1, 20, 10, 239, DateTimeKind.Local).AddTicks(6500), 10m
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] {"Id", "CardFromNumber", "CardId", "CardToNumber", "DateTime", "Sum"},
                values: new object[]
                {
                    2, null, 2, "6762302693240520",
                    new DateTime(2019, 4, 20, 1, 20, 10, 240, DateTimeKind.Local).AddTicks(6250), 10m
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] {"Id", "CardFromNumber", "CardId", "CardToNumber", "DateTime", "Sum"},
                values: new object[]
                {
                    3, null, 3, "4083967629457310",
                    new DateTime(2019, 4, 20, 1, 20, 10, 240, DateTimeKind.Local).AddTicks(6300),
                    0.1376651982378854625550660793m
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] {"Id", "CardFromNumber", "CardId", "CardToNumber", "DateTime", "Sum"},
                values: new object[]
                {
                    4, null, 4, "5101265622568232",
                    new DateTime(2019, 4, 20, 1, 20, 10, 244, DateTimeKind.Local).AddTicks(8420),
                    0.1595405232929164007657945118m
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardNumber",
                table: "Cards",
                column: "CardNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UserId",
                table: "Cards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CardId",
                table: "Transactions",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}