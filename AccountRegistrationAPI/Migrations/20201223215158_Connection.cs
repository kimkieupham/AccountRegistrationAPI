using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountRegistrationAPI.Migrations
{
    public partial class Connection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountDetails",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    GmailAccount = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    UserPassword = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountDetails", x => x.AccountID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountDetails");
        }
    }
}
