using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlfaBank.WebApi.Migrations
{
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
                columns: new[] { "Id", "Birthday", "Firstname", "Password", "Surname", "UserName" },
                values: new object[] { 1, null, null, "AQAAAAEAACcQAAAAEMIIwi1XzayiOoJ+kTDVTmiGQx0ikUcE78oVHg2qxz4xo/WXUz33uYJUaJ9mmJt52A==", null, "alice@alfabank.ru" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Birthday", "Firstname", "Password", "Surname", "UserName" },
                values: new object[] { 2, null, null, "AQAAAAEAACcQAAAAEOLUkP2taPnRbqiXbS6jjQKlbl/gfDehjMS1Pi9XIo5SjDt7qlU44AKMPXzXht2HBw==", null, "bob@alfabank.ru" });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "CardName", "CardNumber", "CardType", "Currency", "DtOpenCard", "UserId", "ValidityYear" },
                values: new object[] { 1, "my salary", "4083966267580714", 2, 0, new DateTime(2017, 6, 21, 0, 0, 0, 0, DateTimeKind.Local), 1, 3 });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "CardName", "CardNumber", "CardType", "Currency", "DtOpenCard", "UserId", "ValidityYear" },
                values: new object[] { 2, "my salary", "6271197881674397", 3, 0, new DateTime(2017, 6, 21, 0, 0, 0, 0, DateTimeKind.Local), 1, 3 });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "CardName", "CardNumber", "CardType", "Currency", "DtOpenCard", "UserId", "ValidityYear" },
                values: new object[] { 3, "my debt", "4083966485519163", 2, 2, new DateTime(2017, 6, 21, 0, 0, 0, 0, DateTimeKind.Local), 1, 3 });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "CardName", "CardNumber", "CardType", "Currency", "DtOpenCard", "UserId", "ValidityYear" },
                values: new object[] { 4, "for my family", "5101264449620920", 1, 1, new DateTime(2017, 6, 21, 0, 0, 0, 0, DateTimeKind.Local), 1, 3 });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "CardName", "CardNumber", "CardType", "Currency", "DtOpenCard", "UserId", "ValidityYear" },
                values: new object[] { 5, "my salary", "4083964697129640", 2, 0, new DateTime(2017, 6, 21, 0, 0, 0, 0, DateTimeKind.Local), 2, 3 });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "CardName", "CardNumber", "CardType", "Currency", "DtOpenCard", "UserId", "ValidityYear" },
                values: new object[] { 6, "my salary", "6271190527962680", 3, 0, new DateTime(2017, 6, 21, 0, 0, 0, 0, DateTimeKind.Local), 2, 3 });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "CardName", "CardNumber", "CardType", "Currency", "DtOpenCard", "UserId", "ValidityYear" },
                values: new object[] { 7, "my debt", "4083965831519208", 2, 2, new DateTime(2017, 6, 21, 0, 0, 0, 0, DateTimeKind.Local), 2, 3 });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "CardName", "CardNumber", "CardType", "Currency", "DtOpenCard", "UserId", "ValidityYear" },
                values: new object[] { 8, "for my family", "5101263592852637", 1, 1, new DateTime(2017, 6, 21, 0, 0, 0, 0, DateTimeKind.Local), 2, 3 });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "CardFromNumber", "CardId", "CardToNumber", "DateTime", "Sum" },
                values: new object[] { 1, null, 1, "4083966267580714", new DateTime(2019, 6, 21, 21, 2, 48, 829, DateTimeKind.Local).AddTicks(2740), 10m });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "CardFromNumber", "CardId", "CardToNumber", "DateTime", "Sum" },
                values: new object[] { 2, null, 2, "6271197881674397", new DateTime(2019, 6, 21, 21, 2, 48, 829, DateTimeKind.Local).AddTicks(6630), 10m });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "CardFromNumber", "CardId", "CardToNumber", "DateTime", "Sum" },
                values: new object[] { 3, null, 3, "4083966485519163", new DateTime(2019, 6, 21, 21, 2, 48, 829, DateTimeKind.Local).AddTicks(6670), 0.1376651982378854625550660793m });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "CardFromNumber", "CardId", "CardToNumber", "DateTime", "Sum" },
                values: new object[] { 4, null, 4, "5101264449620920", new DateTime(2019, 6, 21, 21, 2, 48, 830, DateTimeKind.Local).AddTicks(4340), 0.1595405232929164007657945118m });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "CardFromNumber", "CardId", "CardToNumber", "DateTime", "Sum" },
                values: new object[] { 5, null, 5, "4083964697129640", new DateTime(2019, 6, 21, 21, 2, 48, 830, DateTimeKind.Local).AddTicks(4350), 10m });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "CardFromNumber", "CardId", "CardToNumber", "DateTime", "Sum" },
                values: new object[] { 6, null, 6, "6271190527962680", new DateTime(2019, 6, 21, 21, 2, 48, 830, DateTimeKind.Local).AddTicks(4370), 10m });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "CardFromNumber", "CardId", "CardToNumber", "DateTime", "Sum" },
                values: new object[] { 7, null, 7, "4083965831519208", new DateTime(2019, 6, 21, 21, 2, 48, 830, DateTimeKind.Local).AddTicks(4370), 0.1376651982378854625550660793m });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "CardFromNumber", "CardId", "CardToNumber", "DateTime", "Sum" },
                values: new object[] { 8, null, 8, "5101263592852637", new DateTime(2019, 6, 21, 21, 2, 48, 830, DateTimeKind.Local).AddTicks(4370), 0.1595405232929164007657945118m });

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
