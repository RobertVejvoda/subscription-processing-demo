using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerBusinessAPI.Migrations
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
                name: "SubscriptionRequestNumbers",
                schema: "dbo",
                startValue: 100L);

            migrationBuilder.CreateTable(
                name: "Subscription",
                schema: "dbo",
                columns: table => new
                {
                    ProcessInstanceKey = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    SubscriptionRequestNo = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR SubscriptionRequestNumbers"),
                    CustomerId = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Email = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SubscriptionId = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    LoanAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    InsuredAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ProductId = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    SubscriptionState = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: true),
                    ReceivedOn = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    UnderwritingResultMessage = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.ProcessInstanceKey)
                        .Annotation("SqlServer:Clustered", true);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscription",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "SubscriptionRequestNumbers",
                schema: "dbo");
        }
    }
}
