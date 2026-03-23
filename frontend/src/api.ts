const BASE = import.meta.env.PROD ? "/api" : "http://localhost:5000/api";

export interface TrainingWeek {
  id: number;
  week: number;
  phase: number;
  session1: string;
  session2: string;
  duration: string;
  note: string;
}

export interface PhaseInfo {
  phase: number;
  dotColor: string;
  label: string;
}

export interface ScheduleDay {
  id: number;
  order: number;
  day: string;
  icon: string;
  label: string;
  isRun: boolean;
}

export async function fetchSessions(): Promise<{ week: number; sessionNumber: number; completed: boolean }[]> {
  const res = await fetch(`${BASE}/sessions`);
  if (!res.ok) throw new Error("Failed to load sessions");
  return res.json();
}

export async function upsertSession(week: number, sessionNumber: number, completed: boolean) {
  const res = await fetch(`${BASE}/sessions/${week}/${sessionNumber}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ completed }),
  });
  if (!res.ok) throw new Error("Failed to save session");
  return res.json();
}

export async function fetchPlan(): Promise<TrainingWeek[]> {
  const res = await fetch(`${BASE}/plan`);
  if (!res.ok) throw new Error("Failed to load plan");
  return res.json();
}

export async function fetchPhases(): Promise<PhaseInfo[]> {
  const res = await fetch(`${BASE}/plan/phases`);
  if (!res.ok) throw new Error("Failed to load phases");
  return res.json();
}

export async function fetchSchedule(): Promise<ScheduleDay[]> {
  const res = await fetch(`${BASE}/plan/schedule`);
  if (!res.ok) throw new Error("Failed to load schedule");
  return res.json();
}
