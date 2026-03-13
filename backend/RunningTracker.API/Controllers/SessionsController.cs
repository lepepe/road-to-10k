using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunningTracker.API.Data;
using RunningTracker.API.Models;

namespace RunningTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionsController(AppDbContext db) : ControllerBase
{
    // GET api/sessions
    // Returns all sessions (completed or not) as a flat list
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sessions = await db.Sessions.ToListAsync();
        return Ok(sessions);
    }

    // PUT api/sessions/{week}/{sessionNumber}
    // Upserts a session's completed status
    [HttpPut("{week:int}/{sessionNumber:int}")]
    public async Task<IActionResult> Upsert(int week, int sessionNumber, [FromBody] UpsertRequest req)
    {
        if (week < 1 || week > 16 || sessionNumber < 1 || sessionNumber > 2)
            return BadRequest("Invalid week (1–16) or session number (1–2).");

        var session = await db.Sessions
            .FirstOrDefaultAsync(s => s.Week == week && s.SessionNumber == sessionNumber);

        if (session is null)
        {
            session = new Session { Week = week, SessionNumber = sessionNumber };
            db.Sessions.Add(session);
        }

        session.Completed = req.Completed;
        session.CompletedAt = req.Completed ? DateTime.UtcNow : null;

        await db.SaveChangesAsync();
        return Ok(session);
    }

    // DELETE api/sessions/reset  — wipe all progress (useful for testing)
    [HttpDelete("reset")]
    public async Task<IActionResult> Reset()
    {
        db.Sessions.RemoveRange(db.Sessions);
        await db.SaveChangesAsync();
        return NoContent();
    }
}

public record UpsertRequest(bool Completed);
