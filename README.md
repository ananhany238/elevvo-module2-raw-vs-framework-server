# Module 2: Raw HTTP Server vs ASP.NET Core Minimal API

Two versions of the same small API, built in C#/.NET 8 for the Elevvo Backend Track.

## Routes

| Method | Path | Response |
|---|---|---|
| GET | `/` | Welcome message |
| GET | `/api/users` | List of users (id, name, role) |
| GET | `/api/health` | `status: "OK"` and a UTC timestamp |
| any | anything else | `404` with a JSON error |

## Version A: `RawServer` (HttpListener, no framework)

I wrote everything by hand:
- the loop that waits for each request
- reading the method and path and routing with `if / else`
- turning JSON text into bytes and setting the length
- setting the status code and content type
- closing the response, and a `try/catch` so one bad request can't stop the server

## Version B: `FrameworkServer` (ASP.NET Core minimal API)

Each route is one line with `app.MapGet(...)`. The framework handles:
- the request loop and the HTTP server itself (Kestrel)
- routing by method and path
- JSON serialization (camelCase by default) and the `application/json` content type
- the status code, bytes and closing the response

## What I learned

The raw version showed me what actually happens in an HTTP request: read the request, decide what to return, write bytes back, close. The framework does all of that for me, so my code only describes *what* each route returns. About 60 lines became about 20.

## Run

```powershell
cd FrameworkServer
dotnet run
```

Then open `http://localhost:3000/api/health`.