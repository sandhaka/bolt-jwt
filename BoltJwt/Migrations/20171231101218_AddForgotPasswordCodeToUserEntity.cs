using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BoltJwt.Migrations
{
    public partial class AddForgotPasswordCodeToUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForgotPasswordAuthCode",
                schema: "IdentityContext",
                table: "users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForgotPasswordAuthCode",
                schema: "IdentityContext",
                table: "users");
        }
    }
}
