using Ca.WebApi;
using Ca.WebApi.Extensions;
using Ca.WebApi.Middlewares;
using Ca.WebApi.Startup;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Add services to the container.

builder.Services.AddWebApiServices(builder.Configuration, builder.Environment);

#endregion

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

app.UseAppServices(); // Swagger, HealthChecks, etc
await app.PerformAppSeeder();

app.UseHttpsRedirection();

app.UseMiddleware<ConcurrencyConflictMiddleware>(); // Idempotency helper

app.UseCors();

app.UseAuthentication();

app.UseRateLimiter(); // Following Authentication, now safely access the user's identity or claims for user-based rate limiting.

app.UseAuthorization();

app.MapControllers();

app.Run();
