# ── Stage 1: Build React frontend ────────────────────────────────────────────
FROM node:22-alpine AS frontend-build
WORKDIR /app
COPY frontend/package*.json ./
RUN npm ci
COPY frontend/ ./
RUN npm run build

# ── Stage 2: Build .NET backend ──────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS backend-build
WORKDIR /app
COPY backend/RunningTracker.API/RunningTracker.API.csproj ./
RUN dotnet restore
COPY backend/RunningTracker.API/ ./
# Embed the React build into wwwroot so the API serves it
COPY --from=frontend-build /app/dist ./wwwroot
RUN dotnet publish -c Release -o /publish

# ── Stage 3: Runtime image ───────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=backend-build /publish ./
# Railway injects PORT; default to 8080 for local Docker runs
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "RunningTracker.API.dll"]
