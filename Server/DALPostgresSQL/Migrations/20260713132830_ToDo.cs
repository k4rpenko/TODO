using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DALPostgresSQL.Migrations
{
    /// <inheritdoc />
    public partial class ToDo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ToDoList");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "ToDoList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Salt = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<List<string>>(type: "text[]", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ToDoCards",
                schema: "ToDoList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Collor = table.Column<string>(type: "text", nullable: false),
                    Design = table.Column<int>(type: "integer", nullable: false),
                    Hashtags = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoCards_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "ToDoList",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToDoItems",
                schema: "ToDoList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ToDoCardId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoItems_ToDoCards_ToDoCardId",
                        column: x => x.ToDoCardId,
                        principalSchema: "ToDoList",
                        principalTable: "ToDoCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoCards_UserId",
                schema: "ToDoList",
                table: "ToDoCards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_ToDoCardId",
                schema: "ToDoList",
                table: "ToDoItems",
                column: "ToDoCardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDoItems",
                schema: "ToDoList");

            migrationBuilder.DropTable(
                name: "ToDoCards",
                schema: "ToDoList");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "ToDoList");
        }
    }
}
