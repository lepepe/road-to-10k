using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunningTracker.API.Data;

namespace RunningTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlanController(AppDbContext db) : ControllerBase
{
    // GET api/plan
    [HttpGet]
    public async Task<IActionResult> GetPlan() =>
        Ok(await db.TrainingWeeks.OrderBy(w => w.Week).ToListAsync());

    // GET api/plan/phases
    [HttpGet("phases")]
    public async Task<IActionResult> GetPhases() =>
        Ok(await db.PhaseInfos.OrderBy(p => p.Phase).ToListAsync());

    // GET api/plan/schedule
    [HttpGet("schedule")]
    public async Task<IActionResult> GetSchedule() =>
        Ok(await db.ScheduleDays.OrderBy(d => d.Order).ToListAsync());
}
