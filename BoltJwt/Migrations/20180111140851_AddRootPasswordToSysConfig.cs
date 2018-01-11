using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BoltJwt.Migrations
{
    public partial class AddRootPasswordToSysConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RootPassword",
                schema: "IdentityContext",
                table: "configuration",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RootPassword",
                schema: "IdentityContext",
                table: "configuration");
        }
    }
}
