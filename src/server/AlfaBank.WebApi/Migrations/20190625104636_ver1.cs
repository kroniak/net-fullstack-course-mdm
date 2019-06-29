using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable UnusedMember.Global
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Surname = table.Column<string>(nullable: true),
                    Firstname = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Sum = table.Column<decimal>(type: "decimal(16, 2)", nullable: false),
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
                columns: new[] { "Id", "Birthday", "Firstname", "Password", "Surname", "UserName" },
                values: new object[] { 1, null, null, "AQAAAAEAACcQAAAAEL5EJYCdN4s5drio+R0ZHNwBGdE32w9FmcYWRnorikhSQQ+DeLs1eA/AkMG9sUbY7w==", null, "alice@alfabank.ru" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Birthday", "Firstname", "Password", "Surname", "UserName" },
                values: new object[] { 2, null, null, "AQAAAAEAACcQAAAAEOmCkrmo4bS4QMrbVf0j3t0/bmgfDW96E7yTnOJh8ZbTvshRgKf/cstvbtSHg49HxQ==", null, "bob@alfabank.ru" });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "CardName", "CardNumber", "CardType", "Currency", "DtOpenCard", "UserId", "ValidityYear" },
                values: new object[,]
                {
                    { 1, "my salary", "4083969601038878", 2, 0, new DateTime(2017, 6, 25, 0, 0, 0, 0, DateTimeKind.Local), 1, 3 },
                    { 2, "my salary", "6271195459697261", 3, 0, new DateTime(2017, 6, 25, 0, 0, 0, 0, DateTimeKind.Local), 1, 3 },
                    { 3, "my debt", "4083969671288296", 2, 2, new DateTime(2017, 6, 25, 0, 0, 0, 0, DateTimeKind.Local), 1, 3 },
                    { 4, "for my family", "5101266390203309", 1, 1, new DateTime(2017, 6, 25, 0, 0, 0, 0, DateTimeKind.Local), 1, 3 },
                    { 5, "my salary", "4083961558623794", 2, 0, new DateTime(2017, 6, 25, 0, 0, 0, 0, DateTimeKind.Local), 2, 3 },
                    { 6, "my salary", "6271196413417853", 3, 0, new DateTime(2017, 6, 25, 0, 0, 0, 0, DateTimeKind.Local), 2, 3 },
                    { 7, "my debt", "4083967519051926", 2, 2, new DateTime(2017, 6, 25, 0, 0, 0, 0, DateTimeKind.Local), 2, 3 },
                    { 8, "for my family", "5101265282206347", 1, 1, new DateTime(2017, 6, 25, 0, 0, 0, 0, DateTimeKind.Local), 2, 3 }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "CardFromNumber", "CardId", "CardToNumber", "DateTime", "Sum" },
                values: new object[,]
                {
                    { 1, null, 1, "4083969601038878", new DateTime(2019, 6, 25, 13, 46, 36, 234, DateTimeKind.Local).AddTicks(3390), 10m },
                    { 2, null, 2, "6271195459697261", new DateTime(2019, 6, 25, 13, 46, 36, 234, DateTimeKind.Local).AddTicks(7910), 10m },
                    { 3, null, 3, "4083969671288296", new DateTime(2019, 6, 25, 13, 46, 36, 234, DateTimeKind.Local).AddTicks(7950), 0.1376651982378854625550660793m },
                    { 4, null, 4, "5101266390203309", new DateTime(2019, 6, 25, 13, 46, 36, 235, DateTimeKind.Local).AddTicks(7890), 0.1595405232929164007657945118m },
                    { 5, null, 5, "4083961558623794", new DateTime(2019, 6, 25, 13, 46, 36, 235, DateTimeKind.Local).AddTicks(7900), 10m },
                    { 6, null, 6, "6271196413417853", new DateTime(2019, 6, 25, 13, 46, 36, 235, DateTimeKind.Local).AddTicks(7920), 10m },
                    { 7, null, 7, "4083967519051926", new DateTime(2019, 6, 25, 13, 46, 36, 235, DateTimeKind.Local).AddTicks(7930), 0.1376651982378854625550660793m },
                    { 8, null, 8, "5101265282206347", new DateTime(2019, 6, 25, 13, 46, 36, 235, DateTimeKind.Local).AddTicks(7930), 0.1595405232929164007657945118m }
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
