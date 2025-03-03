using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OriginSolutions.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    UBK = table.Column<string>(type: "nvarchar(22)", maxLength: 22, nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    NID = table.Column<long>(type: "bigint", nullable: false),
                    Balance = table.Column<long>(type: "bigint", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.UBK);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Number = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    NID = table.Column<long>(type: "bigint", nullable: false),
                    LoginFails = table.Column<byte>(type: "tinyint", nullable: false),
                    Blocked = table.Column<bool>(type: "bit", nullable: false),
                    Pin = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Cvv = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Expire = table.Column<DateOnly>(type: "date", nullable: false),
                    FK_Account = table.Column<string>(type: "nvarchar(22)", maxLength: 22, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Number);
                    table.ForeignKey(
                        name: "FK_Cards_Accounts_FK_Account",
                        column: x => x.FK_Account,
                        principalTable: "Accounts",
                        principalColumn: "UBK",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OperationEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    OperationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperationType = table.Column<byte>(type: "tinyint", nullable: false),
                    FK_Card = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    FK_Account = table.Column<string>(type: "nvarchar(22)", maxLength: 22, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationEntries_Accounts_FK_Account",
                        column: x => x.FK_Account,
                        principalTable: "Accounts",
                        principalColumn: "UBK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationEntries_Cards_FK_Card",
                        column: x => x.FK_Card,
                        principalTable: "Cards",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Token = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    FK_Card = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Token);
                    table.ForeignKey(
                        name: "FK_Sessions_Cards_FK_Card",
                        column: x => x.FK_Card,
                        principalTable: "Cards",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operations_AtmWithdraws",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_Account = table.Column<string>(type: "nvarchar(22)", maxLength: 22, nullable: false),
                    FK_Card = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    NewAccountBalance = table.Column<long>(type: "bigint", nullable: false),
                    FK_Entry = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations_AtmWithdraws", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_AtmWithdraws_Accounts_FK_Account",
                        column: x => x.FK_Account,
                        principalTable: "Accounts",
                        principalColumn: "UBK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operations_AtmWithdraws_Cards_FK_Card",
                        column: x => x.FK_Card,
                        principalTable: "Cards",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operations_AtmWithdraws_OperationEntries_FK_Entry",
                        column: x => x.FK_Entry,
                        principalTable: "OperationEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operations_BalanceQueries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_Account = table.Column<string>(type: "nvarchar(22)", maxLength: 22, nullable: false),
                    FK_Card = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Balance = table.Column<long>(type: "bigint", nullable: false),
                    FK_Entry = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations_BalanceQueries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_BalanceQueries_Accounts_FK_Account",
                        column: x => x.FK_Account,
                        principalTable: "Accounts",
                        principalColumn: "UBK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operations_BalanceQueries_Cards_FK_Card",
                        column: x => x.FK_Card,
                        principalTable: "Cards",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operations_BalanceQueries_OperationEntries_FK_Entry",
                        column: x => x.FK_Entry,
                        principalTable: "OperationEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operations_Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_OriginAccount = table.Column<string>(type: "nvarchar(22)", maxLength: 22, nullable: false),
                    FK_DestinationnAccount = table.Column<string>(type: "nvarchar(22)", maxLength: 22, nullable: false),
                    FK_AuthorCard = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    OriginNewBalance = table.Column<long>(type: "bigint", nullable: false),
                    DestinationNewBalance = table.Column<long>(type: "bigint", nullable: false),
                    FK_Entry = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_Transactions_Accounts_FK_DestinationnAccount",
                        column: x => x.FK_DestinationnAccount,
                        principalTable: "Accounts",
                        principalColumn: "UBK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operations_Transactions_Accounts_FK_OriginAccount",
                        column: x => x.FK_OriginAccount,
                        principalTable: "Accounts",
                        principalColumn: "UBK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operations_Transactions_Cards_FK_AuthorCard",
                        column: x => x.FK_AuthorCard,
                        principalTable: "Cards",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operations_Transactions_OperationEntries_FK_Entry",
                        column: x => x.FK_Entry,
                        principalTable: "OperationEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Alias",
                table: "Accounts",
                column: "Alias",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UBK_Alias_NID",
                table: "Accounts",
                columns: new[] { "UBK", "Alias", "NID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_FK_Account",
                table: "Cards",
                column: "FK_Account");

            migrationBuilder.CreateIndex(
                name: "IX_OperationEntries_FK_Account",
                table: "OperationEntries",
                column: "FK_Account");

            migrationBuilder.CreateIndex(
                name: "IX_OperationEntries_FK_Card_OperationType",
                table: "OperationEntries",
                columns: new[] { "FK_Card", "OperationType" });

            migrationBuilder.CreateIndex(
                name: "IX_Operations_AtmWithdraws_FK_Account",
                table: "Operations_AtmWithdraws",
                column: "FK_Account");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_AtmWithdraws_FK_Card",
                table: "Operations_AtmWithdraws",
                column: "FK_Card");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_AtmWithdraws_FK_Entry",
                table: "Operations_AtmWithdraws",
                column: "FK_Entry");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_BalanceQueries_FK_Account",
                table: "Operations_BalanceQueries",
                column: "FK_Account");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_BalanceQueries_FK_Card",
                table: "Operations_BalanceQueries",
                column: "FK_Card");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_BalanceQueries_FK_Entry",
                table: "Operations_BalanceQueries",
                column: "FK_Entry");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_Transactions_FK_AuthorCard",
                table: "Operations_Transactions",
                column: "FK_AuthorCard");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_Transactions_FK_DestinationnAccount",
                table: "Operations_Transactions",
                column: "FK_DestinationnAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_Transactions_FK_Entry",
                table: "Operations_Transactions",
                column: "FK_Entry");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_Transactions_FK_OriginAccount",
                table: "Operations_Transactions",
                column: "FK_OriginAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_FK_Card",
                table: "Sessions",
                column: "FK_Card");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operations_AtmWithdraws");

            migrationBuilder.DropTable(
                name: "Operations_BalanceQueries");

            migrationBuilder.DropTable(
                name: "Operations_Transactions");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "OperationEntries");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
