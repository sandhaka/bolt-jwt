using Microsoft.EntityFrameworkCore.Migrations;

namespace BoltJwt.Migrations
{
    public partial class AddingUniqueIndexOnUserAndRoleAuths : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_authorizations_DefAuthorizationId",
                schema: "IdentityContext",
                table: "user_authorizations");

            migrationBuilder.DropIndex(
                name: "IX_role_authorizations_DefAuthorizationId",
                schema: "IdentityContext",
                table: "role_authorizations");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_authorizations_DefAuthorizationId_UserId",
                schema: "IdentityContext",
                table: "user_authorizations");

            migrationBuilder.DropIndex(
                name: "IX_role_authorizations_DefAuthorizationId_RoleId",
                schema: "IdentityContext",
                table: "role_authorizations");

            migrationBuilder.CreateIndex(
                name: "IX_user_authorizations_DefAuthorizationId",
                schema: "IdentityContext",
                table: "user_authorizations",
                column: "DefAuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_role_authorizations_DefAuthorizationId",
                schema: "IdentityContext",
                table: "role_authorizations",
                column: "DefAuthorizationId");
        }
    }
}
