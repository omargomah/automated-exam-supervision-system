using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addTelemetryNodeAndAddTheTelemetryAlertAndRefactorSomeTablesToTPCAndAddNewAttributies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Evidences");

            migrationBuilder.DropTable(
                name: "AlertEvents");

            migrationBuilder.DropTable(
                name: "CameraDevices");

            migrationBuilder.CreateTable(
                name: "CameraAlerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConfidenceScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewerNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    VideoAlertType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CameraAlerts_ApplicationUsers_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CameraAlerts_ExamSessions_ExamSessionId",
                        column: x => x.ExamSessionId,
                        principalTable: "ExamSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CameraAlerts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cameras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassroomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentIpAddress = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    LastSeenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApiKeyHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    HostName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StreamUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GridRows = table.Column<int>(type: "int", nullable: true),
                    GridColumns = table.Column<int>(type: "int", nullable: true),
                    GridStartSeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CalibratedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cameras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cameras_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cameras_Seats_GridStartSeatId",
                        column: x => x.GridStartSeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TelemetryAlerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConfidenceScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewerNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RfLevel = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    SoundLevel = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    IsSpike = table.Column<bool>(type: "bit", nullable: false),
                    TelemetryAlertType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BleDeviceCount = table.Column<int>(type: "int", nullable: true),
                    SuspiciousApCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelemetryAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelemetryAlerts_ApplicationUsers_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TelemetryAlerts_ExamSessions_ExamSessionId",
                        column: x => x.ExamSessionId,
                        principalTable: "ExamSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TelemetryNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassroomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentIpAddress = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    LastSeenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApiKeyHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MacAddress = table.Column<string>(type: "varchar(17)", unicode: false, maxLength: 17, nullable: false),
                    FirmwareVersion = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelemetryNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelemetryNodes_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessions_ClassroomId_StartTime",
                table: "ExamSessions",
                columns: new[] { "ClassroomId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessions_Status_StartTime",
                table: "ExamSessions",
                columns: new[] { "Status", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_ApplicationUserId_CreatedAt",
                table: "AuditLogs",
                columns: new[] { "ApplicationUserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CameraAlerts_CreatedAt",
                table: "CameraAlerts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CameraAlerts_DeviceId",
                table: "CameraAlerts",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_CameraAlerts_ExamSessionId",
                table: "CameraAlerts",
                column: "ExamSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_CameraAlerts_ExamSessionId_CreatedAt",
                table: "CameraAlerts",
                columns: new[] { "ExamSessionId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CameraAlerts_ExamSessionId_Status",
                table: "CameraAlerts",
                columns: new[] { "ExamSessionId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_CameraAlerts_ReviewedById",
                table: "CameraAlerts",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_CameraAlerts_Status",
                table: "CameraAlerts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CameraAlerts_StudentId",
                table: "CameraAlerts",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_ApiKeyHash",
                table: "Cameras",
                column: "ApiKeyHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_ClassroomId",
                table: "Cameras",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_GridStartSeatId",
                table: "Cameras",
                column: "GridStartSeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_Status_LastSeenAt",
                table: "Cameras",
                columns: new[] { "Status", "LastSeenAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryAlerts_CreatedAt",
                table: "TelemetryAlerts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryAlerts_DeviceId",
                table: "TelemetryAlerts",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryAlerts_ExamSessionId",
                table: "TelemetryAlerts",
                column: "ExamSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryAlerts_ExamSessionId_CreatedAt",
                table: "TelemetryAlerts",
                columns: new[] { "ExamSessionId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryAlerts_ExamSessionId_Status",
                table: "TelemetryAlerts",
                columns: new[] { "ExamSessionId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryAlerts_ReviewedById",
                table: "TelemetryAlerts",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryAlerts_Status",
                table: "TelemetryAlerts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryNodes_ApiKeyHash",
                table: "TelemetryNodes",
                column: "ApiKeyHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryNodes_ClassroomId",
                table: "TelemetryNodes",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryNodes_MacAddress",
                table: "TelemetryNodes",
                column: "MacAddress",
                unique: true,
                filter: "[MacAddress] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryNodes_Status_LastSeenAt",
                table: "TelemetryNodes",
                columns: new[] { "Status", "LastSeenAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CameraAlerts");

            migrationBuilder.DropTable(
                name: "Cameras");

            migrationBuilder.DropTable(
                name: "TelemetryAlerts");

            migrationBuilder.DropTable(
                name: "TelemetryNodes");

            migrationBuilder.DropIndex(
                name: "IX_ExamSessions_ClassroomId_StartTime",
                table: "ExamSessions");

            migrationBuilder.DropIndex(
                name: "IX_ExamSessions_Status_StartTime",
                table: "ExamSessions");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_ApplicationUserId_CreatedAt",
                table: "AuditLogs");

            migrationBuilder.CreateTable(
                name: "CameraDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassroomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentIpAddress = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    LastSeenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MacAddress = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CameraDevices_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlertEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CameraDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlertType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConfidenceScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewerNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertEvents_ApplicationUsers_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlertEvents_CameraDevices_CameraDeviceId",
                        column: x => x.CameraDeviceId,
                        principalTable: "CameraDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlertEvents_ExamSessions_ExamSessionId",
                        column: x => x.ExamSessionId,
                        principalTable: "ExamSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertEvents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Evidences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlertEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FrameType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MediaPath = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evidences_AlertEvents_AlertEventId",
                        column: x => x.AlertEventId,
                        principalTable: "AlertEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertEvents_CameraDeviceId",
                table: "AlertEvents",
                column: "CameraDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertEvents_CreatedAt",
                table: "AlertEvents",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AlertEvents_ExamSessionId",
                table: "AlertEvents",
                column: "ExamSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertEvents_ReviewedById",
                table: "AlertEvents",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_AlertEvents_Status",
                table: "AlertEvents",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AlertEvents_StudentId",
                table: "AlertEvents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CameraDevices_ClassroomId",
                table: "CameraDevices",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_CameraDevices_MacAddress",
                table: "CameraDevices",
                column: "MacAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_AlertEventId",
                table: "Evidences",
                column: "AlertEventId");
        }
    }
}
