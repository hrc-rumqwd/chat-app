---
project_name: 'chat-app'
user_name: 'LINHLTN'
date: '2026-04-14'
sections_completed: ['technology_stack']
existing_patterns_found: 8
---

# Project Context for AI Agents

_This file contains critical rules and patterns that AI agents must follow when implementing code in this project. Focus on unobvious details that agents might otherwise miss._

---

## Technology Stack & Versions

- .NET 10 (`net10.0`) across all projects
- ASP.NET Core MVC + Razor Runtime Compilation
- SignalR (`Microsoft.AspNetCore.SignalR.Client` 10.0.0)
- MediatR 14.0.0 with a broker abstraction (`IBroker`)
- Entity Framework Core 10.0.0 + Npgsql 10.0.0 (PostgreSQL)
- FluentValidation 12.1.1
- Redis (`StackExchange.Redis` 2.12.4)
- Mapster 7.4.0
- Swagger / OpenAPI (`Swashbuckle.AspNetCore.*` 10.1.5)

## Critical Implementation Rules

- Keep all projects on `net10.0`, `Nullable=enable`, and `ImplicitUsings=enable`.
- Route application-layer requests through `IBroker` (`CommandAsync`/`QueryAsync`) rather than calling `IMediator` directly from Web controllers.
- Register services via extension methods (`AddApplication()`, `AddInfrastructure()`, `RegisterSignalR()`) in `Program.cs` to keep startup composition consistent.
- Follow namespace boundaries by layer: `ChatApp.Web.*`, `ChatApp.Application.*`, `ChatApp.Infrastructure.*`, `ChatApp.Data.*`, `ChatApp.Shared.*`.
- For persistence work, use `ApplicationDbContext` with Npgsql and preserve configured interceptors (such as `UpdateAuditableInterceptor`).
- Keep authentication cookie behavior aligned with current setup (`/login`, sliding expiration, `HttpOnly`, `SameSite=Strict`) unless a requirement explicitly changes auth policy.
- Use existing controller pattern: constructor injection with primary constructors and result-based HTTP responses (`Ok`, `Created`, `BadRequest`) from `Result<T>`.
