﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace survey_quiz_app.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedAgainUserParticipantListForQuestionBankmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParticipantIdListString",
                table: "QuestionBanks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParticipantIdListString",
                table: "QuestionBanks");
        }
    }
}
