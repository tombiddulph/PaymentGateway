using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway.Application.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cards",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Cvv = table.Column<string>(nullable: true),
                    ExpiryMonth = table.Column<string>(nullable: true),
                    ExpiryYear = table.Column<string>(nullable: true),
                    HolderName = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cards", x => x.Key);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cards");
        }
    }
}
