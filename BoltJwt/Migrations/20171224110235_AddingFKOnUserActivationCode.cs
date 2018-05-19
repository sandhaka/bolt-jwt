using Microsoft.EntityFrameworkCore.Migrations;

namespace BoltJwt.Migrations
{
    public partial class AddingFKOnUserActivationCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_user_activation_codes_UserId",
                schema: "IdentityContext",
                table: "user_activation_codes",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_user_activation_codes_users_UserId",
                schema: "IdentityContext",
                table: "user_activation_codes",
                column: "UserId",
                principalSchema: "IdentityContext",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_activation_codes_users_UserId",
                schema: "IdentityContext",
                table: "user_activation_codes");

            migrationBuilder.DropIndex(
                name: "IX_user_activation_codes_UserId",
                schema: "IdentityContext",
                table: "user_activation_codes");
        }
    }
}
