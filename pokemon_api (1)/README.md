# PokemonAPI
ASP.NET Core Web API scaffold for the Pokemon project (SQLite).

## How to run
1. Install .NET SDK 8.0+.
2. From the project folder, restore and build:
   ```bash
   dotnet restore
   dotnet build
   ```
3. Create EF migrations and update the SQLite DB (requires dotnet-ef):
   ```bash
   dotnet tool install --global dotnet-ef
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
4. Run the API:
   ```bash
   dotnet run
   ```
5. Swagger UI will be available at `https://localhost:5001/swagger` (or the http port shown).

## Notes
- API key for protected endpoints (if implemented) is in `appsettings.json` under `ApiKey`.
- This scaffold includes models, DbContext, a controller and a simple AuthService.
