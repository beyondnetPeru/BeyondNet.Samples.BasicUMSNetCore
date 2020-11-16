using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FargoSecurity.Api.Migrations
{
    public partial class FargoSecurityApi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Systems",
                columns: table => new
                {
                    SystemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    TimeSpan = table.Column<string>(nullable: true),
                    SystemType = table.Column<string>(maxLength: 3, nullable: false),
                    Code = table.Column<string>(maxLength: 6, nullable: false),
                    Name = table.Column<string>(maxLength: 300, nullable: false),
                    Path = table.Column<string>(maxLength: 1500, nullable: false),
                    SecretToken = table.Column<string>(nullable: true),
                    PeriodTypeToken = table.Column<string>(nullable: true),
                    ExpirationTimeToken = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems", x => x.SystemId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    TimeSpan = table.Column<string>(nullable: true),
                    UserType = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 1000, nullable: false),
                    UserName = table.Column<string>(maxLength: 300, nullable: false),
                    Password = table.Column<string>(maxLength: 3000, nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    ModuleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    TimeSpan = table.Column<string>(nullable: true),
                    SystemId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 300, nullable: false),
                    Path = table.Column<string>(maxLength: 1500, nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.ModuleId);
                    table.ForeignKey(
                        name: "FK_Modules_Systems_SystemId",
                        column: x => x.SystemId,
                        principalTable: "Systems",
                        principalColumn: "SystemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RolId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    TimeSpan = table.Column<string>(nullable: true),
                    SystemId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 300, nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RolId);
                    table.ForeignKey(
                        name: "FK_Roles_Systems_SystemId",
                        column: x => x.SystemId,
                        principalTable: "Systems",
                        principalColumn: "SystemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    OptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    TimeSpan = table.Column<string>(nullable: true),
                    ModuleId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 300, nullable: false),
                    Path = table.Column<string>(maxLength: 1500, nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.OptionId);
                    table.ForeignKey(
                        name: "FK_Options_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    TimeSpan = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_Profiles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Profiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    TimeSpan = table.Column<string>(nullable: true),
                    ProfileId = table.Column<int>(nullable: true),
                    ModuleId = table.Column<int>(nullable: true),
                    CanRead = table.Column<bool>(nullable: false, defaultValue: true),
                    Status = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_Assignments_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    TokenId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    TimeSpan = table.Column<string>(nullable: true),
                    ProfileId = table.Column<int>(nullable: true),
                    PublicToken = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.TokenId);
                    table.ForeignKey(
                        name: "FK_Tokens_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentItems",
                columns: table => new
                {
                    AssignmentItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    TimeSpan = table.Column<string>(nullable: true),
                    AssignmentId = table.Column<int>(nullable: true),
                    OptionId = table.Column<int>(nullable: true),
                    CanRead = table.Column<bool>(nullable: false, defaultValue: true),
                    Status = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentItems", x => x.AssignmentItemId);
                    table.ForeignKey(
                        name: "FK_AssignmentItems_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "AssignmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssignmentItems_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "OptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentItems_AssignmentId",
                table: "AssignmentItems",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentItems_OptionId",
                table: "AssignmentItems",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ModuleId",
                table: "Assignments",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ProfileId",
                table: "Assignments",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_SystemId",
                table: "Modules",
                column: "SystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Options_ModuleId",
                table: "Options",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_RoleId",
                table: "Profiles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_SystemId",
                table: "Roles",
                column: "SystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_ProfileId",
                table: "Tokens",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentItems");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Systems");
        }
    }
}
