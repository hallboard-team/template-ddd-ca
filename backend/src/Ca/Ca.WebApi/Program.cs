using Ca.WebApi;
using Ca.WebApi.Startup;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Add services to the container.

builder.Services.AddWebApiServices(builder.Configuration, builder.Environment);

#endregion

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "Ca.WebApi"); }
    );
}

await app.PerformAppSeeder();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();