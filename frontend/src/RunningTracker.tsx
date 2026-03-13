import { useState, useEffect, useCallback } from "react";
import {
  fetchSessions, upsertSession,
  fetchPlan, fetchPhases, fetchSchedule,
  TrainingWeek, PhaseInfo, ScheduleDay,
} from "./api";

type CompletedMap = Record<string, boolean>;

export default function RunningTracker() {
  const [plan, setPlan] = useState<TrainingWeek[]>([]);
  const [phases, setPhases] = useState<PhaseInfo[]>([]);
  const [schedule, setSchedule] = useState<ScheduleDay[]>([]);
  const [completed, setCompleted] = useState<CompletedMap>({});
  const [expanded, setExpanded] = useState<number | null>(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    Promise.all([fetchSessions(), fetchPlan(), fetchPhases(), fetchSchedule()])
      .then(([sessions, planData, phasesData, scheduleData]) => {
        const map: CompletedMap = {};
        sessions.forEach((s) => {
          if (s.completed) map[`${s.week}-s${s.sessionNumber}`] = true;
        });
        setCompleted(map);
        setPlan(planData);
        setPhases(phasesData);
        setSchedule(scheduleData);
      })
      .catch(() => setError("Could not reach the API. Is the backend running?"))
      .finally(() => setLoading(false));
  }, []);

  const toggle = useCallback(async (week: number, sessionNum: number) => {
    const key = `${week}-s${sessionNum}`;
    const newVal = !completed[key];
    setSaving(key);
    setCompleted((prev) => ({ ...prev, [key]: newVal }));
    try {
      await upsertSession(week, sessionNum, newVal);
    } catch {
      setCompleted((prev) => ({ ...prev, [key]: !newVal }));
      setError("Failed to save — please try again.");
    } finally {
      setSaving(null);
    }
  }, [completed]);

  const phaseMap = Object.fromEntries(phases.map((p) => [p.phase, p]));

  const totalSessions = plan.length * 2;
  const completedCount = Object.values(completed).filter(Boolean).length;
  const progress = totalSessions > 0 ? Math.round((completedCount / totalSessions) * 100) : 0;

  const currentWeek =
    plan.find((w) => !completed[`${w.week}-s1`] || !completed[`${w.week}-s2`])?.week ?? plan.length;

  if (loading) {
    return (
      <div style={{ fontFamily: "'Georgia', serif", background: "#0a0f1a", minHeight: "100vh", display: "flex", alignItems: "center", justifyContent: "center", color: "#7a9cc4" }}>
        Loading your progress…
      </div>
    );
  }

  return (
    <div style={{ fontFamily: "'Georgia', serif", background: "#0a0f1a", minHeight: "100vh", color: "#e8e0d4" }}>
      {/* Header */}
      <div style={{ background: "linear-gradient(135deg, #0d1f3c 0%, #0a0f1a 60%, #1a0f0d 100%)", borderBottom: "1px solid #2a3550", padding: "2.5rem 1.5rem 2rem" }}>
        <div style={{ maxWidth: 780, margin: "0 auto" }}>
          {error && (
            <div style={{ background: "#3a1010", border: "1px solid #7f2020", borderRadius: 8, padding: "0.6rem 1rem", marginBottom: "1rem", fontSize: "0.82rem", color: "#f87171", display: "flex", justifyContent: "space-between" }}>
              {error}
              <button onClick={() => setError(null)} style={{ background: "none", border: "none", color: "#f87171", cursor: "pointer" }}>✕</button>
            </div>
          )}
          <div style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", flexWrap: "wrap", gap: "1rem" }}>
            <div>
              <div style={{ fontSize: "0.7rem", letterSpacing: "0.25em", color: "#7a9cc4", textTransform: "uppercase", marginBottom: "0.4rem" }}>16-Week Program</div>
              <h1 style={{ fontSize: "2.2rem", fontWeight: "bold", color: "#f0e8d8", margin: 0, lineHeight: 1.1 }}>Road to 10K</h1>
              <p style={{ color: "#8a9ab5", marginTop: "0.4rem", fontSize: "0.9rem" }}>Beginner treadmill plan · 2 sessions/week</p>
            </div>
            <div style={{ textAlign: "right" }}>
              <div style={{ fontSize: "2rem", fontWeight: "bold", color: "#f0e8d8" }}>{progress}%</div>
              <div style={{ fontSize: "0.75rem", color: "#7a9cc4" }}>{completedCount} of {totalSessions} sessions done</div>
            </div>
          </div>

          <div style={{ marginTop: "1.5rem", height: 6, background: "#1e2d45", borderRadius: 99, overflow: "hidden" }}>
            <div style={{ height: "100%", width: `${progress}%`, background: "linear-gradient(90deg, #3b82f6, #f59e0b)", borderRadius: 99, transition: "width 0.4s ease" }} />
          </div>

          <div style={{ display: "flex", gap: "1.5rem", marginTop: "1rem", flexWrap: "wrap" }}>
            {phases.map((p) => (
              <div key={p.phase} style={{ display: "flex", alignItems: "center", gap: "0.4rem" }}>
                <div style={{ width: 8, height: 8, borderRadius: "50%", background: p.dotColor }} />
                <span style={{ fontSize: "0.72rem", color: "#8a9ab5" }}>{p.label}</span>
              </div>
            ))}
          </div>
        </div>
      </div>

      <div style={{ maxWidth: 780, margin: "0 auto", padding: "1.5rem 1rem" }}>
        {/* Weekly schedule */}
        <div style={{ background: "#111827", border: "1px solid #1e2d45", borderRadius: 12, padding: "1rem 1.25rem", marginBottom: "2rem" }}>
          <div style={{ fontSize: "0.7rem", letterSpacing: "0.2em", color: "#7a9cc4", textTransform: "uppercase", marginBottom: "0.75rem" }}>Weekly Schedule Template</div>
          <div style={{ display: "grid", gridTemplateColumns: `repeat(${schedule.length || 7}, 1fr)`, gap: "0.4rem" }}>
            {schedule.map((d) => (
              <div key={d.id} style={{ textAlign: "center", background: d.isRun ? "#1a2e4a" : "#0d1525", border: d.isRun ? "1px solid #3b6ea5" : "1px solid #1e2d45", borderRadius: 8, padding: "0.5rem 0.25rem" }}>
                <div style={{ fontSize: "1.1rem" }}>{d.icon}</div>
                <div style={{ fontSize: "0.6rem", color: d.isRun ? "#7ab4f5" : "#6b7a96", marginTop: "0.2rem", fontFamily: "monospace" }}>{d.day}</div>
                <div style={{ fontSize: "0.58rem", color: d.isRun ? "#93c5fd" : "#4a5568", marginTop: "0.1rem" }}>{d.label}</div>
              </div>
            ))}
          </div>
        </div>

        {/* Week cards */}
        <div style={{ display: "flex", flexDirection: "column", gap: "0.75rem" }}>
          {plan.map((w) => {
            const s1done = !!completed[`${w.week}-s1`];
            const s2done = !!completed[`${w.week}-s2`];
            const weekDone = s1done && s2done;
            const isCurrent = w.week === currentWeek;
            const isOpen = expanded === w.week;
            const phase = phaseMap[w.phase];

            return (
              <div key={w.week} style={{ border: `1px solid ${weekDone ? "#2d4a2d" : isCurrent ? "#3b6ea5" : "#1e2d45"}`, borderRadius: 12, overflow: "hidden", background: weekDone ? "#0d1a0d" : isCurrent ? "#0d1e35" : "#0d1220" }}>
                <div onClick={() => setExpanded(isOpen ? null : w.week)} style={{ display: "flex", alignItems: "center", gap: "1rem", padding: "0.9rem 1.1rem", cursor: "pointer", userSelect: "none" }}>
                  <div style={{ display: "flex", alignItems: "center", gap: "0.5rem", minWidth: 70 }}>
                    <div style={{ width: 8, height: 8, borderRadius: "50%", background: phase?.dotColor ?? "#888", flexShrink: 0 }} />
                    <span style={{ fontSize: "0.75rem", color: "#7a9cc4", fontFamily: "monospace", fontWeight: "bold" }}>Week {w.week}</span>
                  </div>

                  <div style={{ display: "flex", gap: "0.6rem", flex: 1 }}>
                    {([1, 2] as const).map((sNum) => {
                      const key = `${w.week}-s${sNum}`;
                      const done = !!completed[key];
                      const isSaving = saving === key;
                      return (
                        <button
                          key={sNum}
                          onClick={(e) => { e.stopPropagation(); toggle(w.week, sNum); }}
                          disabled={isSaving}
                          style={{ display: "flex", alignItems: "center", gap: "0.35rem", background: done ? "#1a3a1a" : "#151f30", border: `1px solid ${done ? "#4ade80" : "#2a3550"}`, borderRadius: 6, padding: "0.3rem 0.6rem", cursor: "pointer", fontSize: "0.72rem", color: done ? "#4ade80" : "#8a9ab5", opacity: isSaving ? 0.5 : 1, transition: "all 0.15s" }}
                        >
                          <span style={{ fontSize: "0.9rem" }}>{isSaving ? "…" : done ? "✓" : "○"}</span>
                          Run {sNum}
                        </button>
                      );
                    })}
                  </div>

                  <div style={{ display: "flex", alignItems: "center", gap: "0.6rem" }}>
                    {weekDone && <span style={{ fontSize: "0.68rem", color: "#4ade80", background: "#1a3a1a", border: "1px solid #4ade8040", borderRadius: 99, padding: "0.15rem 0.5rem" }}>Done ✓</span>}
                    {isCurrent && !weekDone && <span style={{ fontSize: "0.68rem", color: "#60a5fa", background: "#1a2e4a", border: "1px solid #3b6ea540", borderRadius: 99, padding: "0.15rem 0.5rem" }}>Current</span>}
                    <span style={{ color: "#4a5568", fontSize: "0.8rem", display: "inline-block", transform: isOpen ? "rotate(180deg)" : "none", transition: "transform 0.2s" }}>▾</span>
                  </div>
                </div>

                {isOpen && (
                  <div style={{ borderTop: "1px solid #1e2d45", padding: "0.9rem 1.1rem", display: "flex", flexDirection: "column", gap: "0.6rem" }}>
                    <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "0.5rem" }}>
                      {[{ label: "Session 1 (Wed)", text: w.session1 }, { label: "Session 2 (Sat)", text: w.session2 }].map((s) => (
                        <div key={s.label} style={{ background: "#0d1525", borderRadius: 8, padding: "0.6rem 0.75rem" }}>
                          <div style={{ fontSize: "0.62rem", color: "#7a9cc4", textTransform: "uppercase", letterSpacing: "0.1em", marginBottom: "0.25rem" }}>{s.label}</div>
                          <div style={{ fontSize: "0.82rem", color: "#e8e0d4" }}>{s.text}</div>
                        </div>
                      ))}
                    </div>
                    <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                      <div style={{ fontSize: "0.72rem", color: "#6b7a96" }}>⏱ {w.duration}</div>
                      <div style={{ fontSize: "0.75rem", color: w.phase === 1 ? "#6ee7b7" : w.phase === 2 ? "#7dd3fc" : "#fcd34d", background: "#0d1525", borderRadius: 6, padding: "0.25rem 0.6rem" }}>💡 {w.note}</div>
                    </div>
                  </div>
                )}
              </div>
            );
          })}
        </div>

        {/* Tips */}
        <div style={{ marginTop: "2rem", display: "grid", gridTemplateColumns: "1fr 1fr", gap: "0.75rem" }}>
          {[
            { icon: "🗣️", tip: "Always conversational pace — if you can't talk, slow down." },
            { icon: "🚶", tip: "5 min brisk walk warm-up before every single run." },
            { icon: "🔁", tip: "Week too hard? Repeat it. No shame — it's smart." },
            { icon: "🦵", tip: "Shin or knee pain? Stop immediately and rest 2–3 days." },
          ].map((t, i) => (
            <div key={i} style={{ background: "#0d1220", border: "1px solid #1e2d45", borderRadius: 10, padding: "0.8rem", display: "flex", gap: "0.6rem" }}>
              <span style={{ fontSize: "1.2rem" }}>{t.icon}</span>
              <p style={{ margin: 0, fontSize: "0.75rem", color: "#8a9ab5", lineHeight: 1.5 }}>{t.tip}</p>
            </div>
          ))}
        </div>

        <div style={{ textAlign: "center", marginTop: "1.5rem", fontSize: "0.7rem", color: "#3a4a5a" }}>
          Progress saved to SQLite · C# ASP.NET Core backend
        </div>
      </div>
    </div>
  );
}
