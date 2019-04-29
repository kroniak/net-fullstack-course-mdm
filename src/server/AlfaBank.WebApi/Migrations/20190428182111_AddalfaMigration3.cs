using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlfaBank.WebApi.Migrations
{
    public partial class AddalfaMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersDb",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(maxLength: 100, nullable: false),
                    Surname = table.Column<string>(nullable: true),
                    Firstname = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDb", x => x.Id);
                    table.UniqueConstraint("AK_UsersDb_UserName", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "CardsDb",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CardNumber = table.Column<string>(maxLength: 50, nullable: false),
                    CardName = table.Column<string>(nullable: false),
                    CardType = table.Column<int>(nullable: false),
                    DtOpenCard = table.Column<DateTime>(nullable: false),
                    Balance = table.Column<decimal>(nullable: false),
                    UserDbId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardsDb", x => x.Id);
                    table.UniqueConstraint("AK_CardsDb_CardNumber", x => x.CardNumber);
                    table.UniqueConstraint("AK_CardsDb_CardNumber_CardName", x => new { x.CardNumber, x.CardName });
                    table.ForeignKey(
                        name: "FK_CardsDb_UsersDb_UserDbId",
                        column: x => x.UserDbId,
                        principalTable: "UsersDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionsDb",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CardDbId = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Sum = table.Column<decimal>(nullable: false),
                    CardFromNumber = table.Column<string>(nullable: true),
                    CardToNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionsDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionsDb_CardsDb_CardDbId",
                        column: x => x.CardDbId,
                        principalTable: "CardsDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UsersDb",
                columns: new[] { "Id", "Birthday", "Firstname", "Surname", "UserName" },
                values: new object[] { 1, null, "Firstname", "surname", "iocsha" });

            migrationBuilder.InsertData(
                table: "CardsDb",
                columns: new[] { "Id", "Balance", "CardName", "CardNumber", "CardType", "DtOpenCard", "UserDbId" },
                values: new object[] { 1, 0m, "CardName", "6271190189011743", 0, new DateTime(2019, 4, 28, 21, 20, 54, 366, DateTimeKind.Local).AddTicks(3046), 1 });

            migrationBuilder.InsertData(
                table: "TransactionsDb",
                columns: new[] { "Id", "CardDbId", "CardFromNumber", "CardToNumber", "DateTime", "Sum" },
                values: new object[] { 1, 1, "6271190189011743", "6271190189011743", new DateTime(2019, 4, 28, 21, 20, 54, 367, DateTimeKind.Local).AddTicks(3886), 10m });

            migrationBuilder.CreateIndex(
                name: "IX_CardsDb_UserDbId",
                table: "CardsDb",
                column: "UserDbId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsDb_CardDbId",
                table: "TransactionsDb",
                column: "CardDbId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsDb_DateTime",
                table: "TransactionsDb",
                column: "DateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionsDb");

            migrationBuilder.DropTable(
                name: "CardsDb");

            migrationBuilder.DropTable(
                name: "UsersDb");
        }
    }
}
