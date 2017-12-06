using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

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
                name: "group_role",
                schema: "IdentityContext",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_role", x => new { x.GroupId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_group_role_groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "IdentityContext",
                        principalTable: "groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_group_role_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "IdentityContext",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "role_authorizations",
                schema: "IdentityContext",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                name: "user_authorizations",
                schema: "IdentityContext",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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

            migrationBuilder.CreateTable(
                name: "user_group",
                schema: "IdentityContext",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_group", x => new { x.GroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_user_group_groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "IdentityContext",
                        principalTable: "groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_group_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "IdentityContext",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                schema: "IdentityContext",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_role", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_user_role_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "IdentityContext",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_role_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "IdentityContext",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_def_authorizations_Name",
                schema: "IdentityContext",
                table: "def_authorizations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_group_role_RoleId",
                schema: "IdentityContext",
                table: "group_role",
                column: "RoleId");

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
                name: "IX_user_group_UserId",
                schema: "IdentityContext",
                table: "user_group",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_UserId",
                schema: "IdentityContext",
                table: "user_role",
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
                name: "group_role",
                schema: "IdentityContext");

            migrationBuilder.DropTable(
                name: "role_authorizations",
                schema: "IdentityContext");

            migrationBuilder.DropTable(
                name: "user_authorizations",
                schema: "IdentityContext");

            migrationBuilder.DropTable(
                name: "user_group",
                schema: "IdentityContext");

            migrationBuilder.DropTable(
                name: "user_role",
                schema: "IdentityContext");

            migrationBuilder.DropTable(
                name: "def_authorizations",
                schema: "IdentityContext");

            migrationBuilder.DropTable(
                name: "groups",
                schema: "IdentityContext");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "IdentityContext");

            migrationBuilder.DropTable(
                name: "users",
                schema: "IdentityContext");
        }
    }
}
