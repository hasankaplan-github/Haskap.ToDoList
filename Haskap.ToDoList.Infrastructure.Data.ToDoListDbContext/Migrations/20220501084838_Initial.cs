using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Haskap.ToDoList.Infrastructure.Data.ToDoListDbContext.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDoList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    OwnerUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name_FirstName = table.Column<string>(type: "text", nullable: false),
                    Name_MiddleName = table.Column<string>(type: "text", nullable: true),
                    Name_LastName = table.Column<string>(type: "text", nullable: false),
                    UserName_Value = table.Column<string>(type: "text", nullable: false),
                    Password_Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ToDoItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    OwnerToDoListId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoItem_ToDoList_OwnerToDoListId",
                        column: x => x.OwnerToDoListId,
                        principalTable: "ToDoList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItem_OwnerToDoListId",
                table: "ToDoItem",
                column: "OwnerToDoListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDoItem");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ToDoList");
        }
    }
}
