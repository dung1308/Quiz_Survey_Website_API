using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace survey_quiz_app.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedUserForUsermigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNightMode",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNightMode",
                table: "Users");
        }
    }
}
