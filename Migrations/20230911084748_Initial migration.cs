using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace survey_quiz_app.Migrations
{
    /// <inheritdoc />
    public partial class Initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionBankInteracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResultScores = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionBankInteracts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Permission = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionBanks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    SurveyCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SurveyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Owner = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Timer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StartDate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EndDate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EnableStatus = table.Column<bool>(type: "bit", nullable: false),
                    CategoryListId = table.Column<int>(type: "int", nullable: true),
                    QuestionBankInteractId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionBanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionBanks_CategoryList_CategoryListId",
                        column: x => x.CategoryListId,
                        principalTable: "CategoryList",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuestionBanks_QuestionBankInteracts_QuestionBankInteractId",
                        column: x => x.QuestionBankInteractId,
                        principalTable: "QuestionBankInteracts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionBankInteractId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_QuestionBankInteracts_QuestionBankInteractId",
                        column: x => x.QuestionBankInteractId,
                        principalTable: "QuestionBankInteracts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChoicesString = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AnswersString = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OnAnswersString = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false),
                    QuestionBankId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionBanks_QuestionBankId",
                        column: x => x.QuestionBankId,
                        principalTable: "QuestionBanks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ResultShows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    OnAnswer = table.Column<double>(type: "float", nullable: true),
                    QuestionBankInteractId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultShows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultShows_QuestionBankInteracts_QuestionBankInteractId",
                        column: x => x.QuestionBankInteractId,
                        principalTable: "QuestionBankInteracts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ResultShows_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionBanks_CategoryListId",
                table: "QuestionBanks",
                column: "CategoryListId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionBanks_QuestionBankInteractId",
                table: "QuestionBanks",
                column: "QuestionBankInteractId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionBanks_SurveyCode",
                table: "QuestionBanks",
                column: "SurveyCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionBankId",
                table: "Questions",
                column: "QuestionBankId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultShows_QuestionBankInteractId",
                table: "ResultShows",
                column: "QuestionBankInteractId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultShows_QuestionId",
                table: "ResultShows",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_QuestionBankInteractId",
                table: "Users",
                column: "QuestionBankInteractId",
                unique: true,
                filter: "[QuestionBankInteractId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultShows");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "QuestionBanks");

            migrationBuilder.DropTable(
                name: "CategoryList");

            migrationBuilder.DropTable(
                name: "QuestionBankInteracts");
        }
    }
}
