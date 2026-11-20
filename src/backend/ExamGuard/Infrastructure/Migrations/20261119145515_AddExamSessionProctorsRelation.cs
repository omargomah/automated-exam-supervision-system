using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExamSessionProctorsRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExamSessionId",
                table: "ApplicationUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_ExamSessionId",
                table: "ApplicationUsers",
                column: "ExamSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_ExamSessions_ExamSessionId",
                table: "ApplicationUsers",
                column: "ExamSessionId",
                principalTable: "ExamSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_ExamSessions_ExamSessionId",
                table: "ApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_ExamSessionId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "ExamSessionId",
                table: "ApplicationUsers");
        }
    }
}
