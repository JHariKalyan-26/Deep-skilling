# Microservices - JWT Authentication

This project implements all four hands-on exercises from the Microservices JWT module.

## Implemented

1. JWT authentication and login endpoint
2. Protected API endpoint using `[Authorize]`
3. Role-based authorization using `[Authorize(Roles = "Admin")]`
4. Token expiry handling and custom 401/403 responses

## Demo users

| Username | Password | Role |
|---|---|---|
| admin | Admin@123 | Admin |
| user | User@123 | User |

## Run

```powershell
dotnet run
```

Open the Swagger URL displayed in the terminal.

1. Call `POST /api/Auth/login`.
2. Copy the returned token.
3. Click **Authorize** in Swagger and paste the token.
4. Test `/api/Secure/data`.
5. Test `/api/Admin/dashboard`.

The normal user can access the secure endpoint but cannot access the Admin endpoint.
