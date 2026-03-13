using Microsoft.EntityFrameworkCore;
using RunningTracker.API.Models;

namespace RunningTracker.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<TrainingWeek> TrainingWeeks => Set<TrainingWeek>();
    public DbSet<PhaseInfo> PhaseInfos => Set<PhaseInfo>();
    public DbSet<ScheduleDay> ScheduleDays => Set<ScheduleDay>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Session>()
            .HasIndex(s => new { s.Week, s.SessionNumber })
            .IsUnique();

        modelBuilder.Entity<TrainingWeek>()
            .HasIndex(w => w.Week)
            .IsUnique();

        modelBuilder.Entity<PhaseInfo>()
            .HasKey(p => p.Phase);

        // ── Seed data ────────────────────────────────────────────────────────

        modelBuilder.Entity<PhaseInfo>().HasData(
            new PhaseInfo { Phase = 1, DotColor = "#10b981", Label = "Phase 1 · Build the Habit" },
            new PhaseInfo { Phase = 2, DotColor = "#38bdf8", Label = "Phase 2 · Build Endurance" },
            new PhaseInfo { Phase = 3, DotColor = "#f59e0b", Label = "Phase 3 · Push to 10K" }
        );

        modelBuilder.Entity<ScheduleDay>().HasData(
            new ScheduleDay { Id = 1, Order = 1, Day = "Mon", Icon = "🏋️", Label = "Strength",         IsRun = false },
            new ScheduleDay { Id = 2, Order = 2, Day = "Tue", Icon = "🚴", Label = "Bike",              IsRun = false },
            new ScheduleDay { Id = 3, Order = 3, Day = "Wed", Icon = "🏃", Label = "Run 1",             IsRun = true  },
            new ScheduleDay { Id = 4, Order = 4, Day = "Thu", Icon = "🏋️", Label = "Strength / Rest",  IsRun = false },
            new ScheduleDay { Id = 5, Order = 5, Day = "Fri", Icon = "🚴", Label = "Bike",              IsRun = false },
            new ScheduleDay { Id = 6, Order = 6, Day = "Sat", Icon = "🏃", Label = "Run 2",             IsRun = true  },
            new ScheduleDay { Id = 7, Order = 7, Day = "Sun", Icon = "😴", Label = "Rest",              IsRun = false }
        );

        modelBuilder.Entity<TrainingWeek>().HasData(
            new TrainingWeek { Id = 1,  Week = 1,  Phase = 1, Session1 = "1 min run / 2 min walk × 8",      Session2 = "1 min run / 2 min walk × 8",      Duration = "30 min each",   Note = "Keep it easy — conversational pace!" },
            new TrainingWeek { Id = 2,  Week = 2,  Phase = 1, Session1 = "1.5 min run / 1.5 min walk × 10", Session2 = "1.5 min run / 1.5 min walk × 10", Duration = "30 min each",   Note = "Focus on breathing rhythm." },
            new TrainingWeek { Id = 3,  Week = 3,  Phase = 1, Session1 = "2 min run / 1 min walk × 10",     Session2 = "2 min run / 1 min walk × 10",     Duration = "30 min each",   Note = "You're running more than walking now!" },
            new TrainingWeek { Id = 4,  Week = 4,  Phase = 1, Session1 = "3 min run / 1 min walk × 7",      Session2 = "3 min run / 1 min walk × 7",      Duration = "~28 min each",  Note = "End of Phase 1 — great work!" },
            new TrainingWeek { Id = 5,  Week = 5,  Phase = 2, Session1 = "5 min run / 1 min walk × 5",      Session2 = "5 min run / 1 min walk × 5",      Duration = "30 min each",   Note = "Longer blocks — trust the process." },
            new TrainingWeek { Id = 6,  Week = 6,  Phase = 2, Session1 = "8 min run / 1 min walk × 3",      Session2 = "8 min run / 1 min walk × 3",      Duration = "~27 min each",  Note = "Speed is still irrelevant — just move!" },
            new TrainingWeek { Id = 7,  Week = 7,  Phase = 2, Session1 = "10 min run / 1 min walk × 3",     Session2 = "10 min run / 1 min walk × 3",     Duration = "33 min each",   Note = "Almost continuous running." },
            new TrainingWeek { Id = 8,  Week = 8,  Phase = 2, Session1 = "15 min run / 1 min walk × 2",     Session2 = "15 min run / 1 min walk × 2",     Duration = "32 min each",   Note = "Halfway through the plan!" },
            new TrainingWeek { Id = 9,  Week = 9,  Phase = 2, Session1 = "20 min continuous run",           Session2 = "20 min continuous run",           Duration = "20 min each",   Note = "First continuous run — milestone!" },
            new TrainingWeek { Id = 10, Week = 10, Phase = 2, Session1 = "25 min continuous run",           Session2 = "25 min continuous run",           Duration = "25 min each",   Note = "That's roughly a 5K. You're a runner!" },
            new TrainingWeek { Id = 11, Week = 11, Phase = 3, Session1 = "28 min easy",                     Session2 = "25 min easy",                     Duration = "53 min total",  Note = "Phase 3 — building toward 10K." },
            new TrainingWeek { Id = 12, Week = 12, Phase = 3, Session1 = "30 min easy",                     Session2 = "28 min easy",                     Duration = "58 min total",  Note = "+10% weekly rule — steady progress." },
            new TrainingWeek { Id = 13, Week = 13, Phase = 3, Session1 = "35 min easy",                     Session2 = "30 min easy",                     Duration = "65 min total",  Note = "Legs getting stronger every week." },
            new TrainingWeek { Id = 14, Week = 14, Phase = 3, Session1 = "40 min easy",                     Session2 = "30 min easy",                     Duration = "70 min total",  Note = "You can start picking up pace slightly." },
            new TrainingWeek { Id = 15, Week = 15, Phase = 3, Session1 = "45 min easy",                     Session2 = "35 min easy",                     Duration = "80 min total",  Note = "Almost there — one more week!" },
            new TrainingWeek { Id = 16, Week = 16, Phase = 3, Session1 = "55-60 min easy",                  Session2 = "30 min easy",                     Duration = "~90 min total", Note = "That long run IS your 10K. You did it!" }
        );
    }
}
