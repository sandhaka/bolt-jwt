using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BoltJwt.Migrations
{
    public partial class RefactoringForValueObjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_user_authorizations",
                schema: "IdentityContext",
                table: "user_authorizations");

            migrationBuilder.DropIndex(
                name: "IX_user_authorizations_DefAuthorizationId_UserId",
                schema: "IdentityContext",
                table: "user_authorizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_authorizations",
                schema: "IdentityContext",
                table: "role_authorizations");

            migrationBuilder.DropIndex(
                name: "IX_role_authorizations_DefAuthorizationId_RoleId",
                schema: "IdentityContext",
                table: "role_authorizations");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "IdentityContext",
                table: "user_authorizations");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "IdentityContext",
                table: "role_authorizations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_authorizations",
                schema: "IdentityContext",
                table: "user_authorizations",
                columns: new[] { "DefAuthorizationId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_authorizations",
                schema: "IdentityContext",
                table: "role_authorizations",
                columns: new[] { "DefAuthorizationId", "RoleId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_user_authorizations",
                schema: "IdentityContext",
                table: "user_authorizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_authorizations",
                schema: "IdentityContext",
                table: "role_authorizations");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "IdentityContext",
                table: "user_authorizations",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "IdentityContext",
                table: "role_authorizations",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_authorizations",
                schema: "IdentityContext",
                table: "user_authorizations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_authorizations",
                schema: "IdentityContext",
                table: "role_authorizations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_user_authorizations_DefAuthorizationId_UserId",
                schema: "IdentityContext",
                table: "user_authorizations",
                columns: new[] { "DefAuthorizationId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_authorizations_DefAuthorizationId_RoleId",
                schema: "IdentityContext",
                table: "role_authorizations",
                columns: new[] { "DefAuthorizationId", "RoleId" },
                unique: true);
        }
    }
}
