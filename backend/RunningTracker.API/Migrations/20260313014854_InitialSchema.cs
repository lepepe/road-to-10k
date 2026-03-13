using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RunningTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhaseInfos",
                columns: table => new
                {
                    Phase = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DotColor = table.Column<string>(type: "TEXT", nullable: false),
                    Label = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhaseInfos", x => x.Phase);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    Day = table.Column<string>(type: "TEXT", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: false),
                    Label = table.Column<string>(type: "TEXT", nullable: false),
                    IsRun = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Week = table.Column<int>(type: "INTEGER", nullable: false),
                    SessionNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Completed = table.Column<bool>(type: "INTEGER", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingWeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Week = table.Column<int>(type: "INTEGER", nullable: false),
                    Phase = table.Column<int>(type: "INTEGER", nullable: false),
                    Session1 = table.Column<string>(type: "TEXT", nullable: false),
                    Session2 = table.Column<string>(type: "TEXT", nullable: false),
                    Duration = table.Column<string>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingWeeks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PhaseInfos",
                columns: new[] { "Phase", "DotColor", "Label" },
                values: new object[,]
                {
                    { 1, "#10b981", "Phase 1 · Build the Habit" },
                    { 2, "#38bdf8", "Phase 2 · Build Endurance" },
                    { 3, "#f59e0b", "Phase 3 · Push to 10K" }
                });

            migrationBuilder.InsertData(
                table: "ScheduleDays",
                columns: new[] { "Id", "Day", "Icon", "IsRun", "Label", "Order" },
                values: new object[,]
                {
                    { 1, "Mon", "🏋️", false, "Strength", 1 },
                    { 2, "Tue", "🚴", false, "Bike", 2 },
                    { 3, "Wed", "🏃", true, "Run 1", 3 },
                    { 4, "Thu", "🏋️", false, "Strength / Rest", 4 },
                    { 5, "Fri", "🚴", false, "Bike", 5 },
                    { 6, "Sat", "🏃", true, "Run 2", 6 },
                    { 7, "Sun", "😴", false, "Rest", 7 }
                });

            migrationBuilder.InsertData(
                table: "TrainingWeeks",
                columns: new[] { "Id", "Duration", "Note", "Phase", "Session1", "Session2", "Week" },
                values: new object[,]
                {
                    { 1, "30 min each", "Keep it easy — conversational pace!", 1, "1 min run / 2 min walk × 8", "1 min run / 2 min walk × 8", 1 },
                    { 2, "30 min each", "Focus on breathing rhythm.", 1, "1.5 min run / 1.5 min walk × 10", "1.5 min run / 1.5 min walk × 10", 2 },
                    { 3, "30 min each", "You're running more than walking now!", 1, "2 min run / 1 min walk × 10", "2 min run / 1 min walk × 10", 3 },
                    { 4, "~28 min each", "End of Phase 1 — great work!", 1, "3 min run / 1 min walk × 7", "3 min run / 1 min walk × 7", 4 },
                    { 5, "30 min each", "Longer blocks — trust the process.", 2, "5 min run / 1 min walk × 5", "5 min run / 1 min walk × 5", 5 },
                    { 6, "~27 min each", "Speed is still irrelevant — just move!", 2, "8 min run / 1 min walk × 3", "8 min run / 1 min walk × 3", 6 },
                    { 7, "33 min each", "Almost continuous running.", 2, "10 min run / 1 min walk × 3", "10 min run / 1 min walk × 3", 7 },
                    { 8, "32 min each", "Halfway through the plan!", 2, "15 min run / 1 min walk × 2", "15 min run / 1 min walk × 2", 8 },
                    { 9, "20 min each", "First continuous run — milestone!", 2, "20 min continuous run", "20 min continuous run", 9 },
                    { 10, "25 min each", "That's roughly a 5K. You're a runner!", 2, "25 min continuous run", "25 min continuous run", 10 },
                    { 11, "53 min total", "Phase 3 — building toward 10K.", 3, "28 min easy", "25 min easy", 11 },
                    { 12, "58 min total", "+10% weekly rule — steady progress.", 3, "30 min easy", "28 min easy", 12 },
                    { 13, "65 min total", "Legs getting stronger every week.", 3, "35 min easy", "30 min easy", 13 },
                    { 14, "70 min total", "You can start picking up pace slightly.", 3, "40 min easy", "30 min easy", 14 },
                    { 15, "80 min total", "Almost there — one more week!", 3, "45 min easy", "35 min easy", 15 },
                    { 16, "~90 min total", "That long run IS your 10K. You did it!", 3, "55-60 min easy", "30 min easy", 16 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_Week_SessionNumber",
                table: "Sessions",
                columns: new[] { "Week", "SessionNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingWeeks_Week",
                table: "TrainingWeeks",
                column: "Week",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhaseInfos");

            migrationBuilder.DropTable(
                name: "ScheduleDays");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "TrainingWeeks");
        }
    }
}
