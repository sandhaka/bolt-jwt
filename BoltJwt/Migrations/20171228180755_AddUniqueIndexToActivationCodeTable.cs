using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BoltJwt.Migrations
{
    public partial class AddUniqueIndexToActivationCodeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "IdentityContext",
                table: "user_activation_codes",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_user_activation_codes_Code",
                schema: "IdentityContext",
                table: "user_activation_codes",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_activation_codes_Code",
                schema: "IdentityContext",
                table: "user_activation_codes");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "IdentityContext",
                table: "user_activation_codes",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
