using Microsoft.EntityFrameworkCore.Migrations;

namespace BoltJwt.Migrations
{
    public partial class addConfigurationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "configuration",
                schema: "IdentityContext",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    SmtpHostName = table.Column<string>(nullable: true),
                    SmtpPassword = table.Column<string>(nullable: true),
                    SmtpPort = table.Column<int>(nullable: false),
                    SmtpUserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_configuration", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "configuration",
                schema: "IdentityContext");
        }
    }
}
