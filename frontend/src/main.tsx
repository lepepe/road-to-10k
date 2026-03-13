import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import RunningTracker from "./RunningTracker";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <RunningTracker />
  </StrictMode>
);
