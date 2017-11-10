using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BoltJwt.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "IdentityContext");

            migrationBuilder.CreateTable(
                name: "def_authorizations",
                schema: "IdentityContext",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_def_authorizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "groups",
                schema: "IdentityContext",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "IdentityContext",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "IdentityContext",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Root = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupRole",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRole", x => new { x.GroupId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_GroupRole_groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "IdentityContext",
                        principalTable: "groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupRole_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "IdentityContext",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_authorizations",
                schema: "IdentityContext",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthorizationName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DefAuthorizationId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_authorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_role_authorizations_def_authorizations_DefAuthorizationId",
                        column: x => x.DefAuthorizationId,
                        principalSchema: "IdentityContext",
                        principalTable: "def_authorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_role_authorizations_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "IdentityContext",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => new { x.GroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserGroup_groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "IdentityContext",
                        principalTable: "groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroup_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "IdentityContext",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRole_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "IdentityContext",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "IdentityContext",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_authorizations",
                schema: "IdentityContext",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthorizationName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DefAuthorizationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_authorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_authorizations_def_authorizations_DefAuthorizationId",
                        column: x => x.DefAuthorizationId,
                        principalSchema: "IdentityContext",
                        principalTable: "def_authorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_authorizations_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "IdentityContext",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupRole_RoleId",
                table: "GroupRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_UserId",
                table: "UserGroup",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_def_authorizations_Name",
                schema: "IdentityContext",
                table: "def_authorizations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_authorizations_AuthorizationName",
                schema: "IdentityContext",
                table: "role_authorizations",
                column: "AuthorizationName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_authorizations_DefAuthorizationId",
                schema: "IdentityContext",
                table: "role_authorizations",
                column: "DefAuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_role_authorizations_RoleId",
                schema: "IdentityContext",
                table: "role_authorizations",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_user_authorizations_AuthorizationName",
                schema: "IdentityContext",
                table: "user_authorizations",
                column: "AuthorizationName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_authorizations_DefAuthorizationId",
                schema: "IdentityContext",
                table: "user_authorizations",
                column: "DefAuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_user_authorizations_UserId",
                schema: "IdentityContext",
                table: "user_authorizations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                schema: "IdentityContext",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_UserName",
                schema: "IdentityContext",
                table: "users",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupRole");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "role_authorizations",
                schema: "IdentityContext");

            migrationBuilder.DropTable(
                name: "user_authorizations",
                schema: "IdentityContext");

            migrationBuilder.DropTable(
                name: "groups",
                schema: "IdentityContext");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "IdentityContext");

            migrationBuilder.DropTable(
                name: "def_authorizations",
                schema: "IdentityContext");

            migrationBuilder.DropTable(
                name: "users",
                schema: "IdentityContext");
        }
    }
}
