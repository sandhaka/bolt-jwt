using Microsoft.EntityFrameworkCore.Migrations;

namespace BoltJwt.Migrations
{
    public partial class addConfigurationFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndpointFqdn",
                schema: "IdentityContext",
                table: "configuration",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmtpEmail",
                schema: "IdentityContext",
                table: "configuration",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndpointFqdn",
                schema: "IdentityContext",
                table: "configuration");

            migrationBuilder.DropColumn(
                name: "SmtpEmail",
                schema: "IdentityContext",
                table: "configuration");
        }
    }
}
