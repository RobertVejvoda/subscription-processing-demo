using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ODSService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "CustomerNumbers",
                schema: "dbo",
                startValue: 100L);

            migrationBuilder.CreateSequence<int>(
                name: "SubscriptionNumbers",
                schema: "dbo",
                startValue: 100L);

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    CustomerNo = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR CustomerNumbers"),
                    FirstName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Email = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    State = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    TotalLoanAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalInsuredAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    SubscriptionNo = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR SubscriptionNumbers"),
                    State = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    LoanAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    InsuredAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ProductId = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    ReceivedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UnderwritingResult = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: true),
                    ProcessInstanceKey = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    CustomerId = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Subscription_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "dbo",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_CustomerId",
                schema: "dbo",
                table: "Subscription",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscription",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "CustomerNumbers",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "SubscriptionNumbers",
                schema: "dbo");
        }
    }
}
