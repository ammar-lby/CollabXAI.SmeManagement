using CollabXAI.Api.Middleware;
using CollabXAI.Application.Interfaces;
using CollabXAI.Application.Services;
using CollabXAI.Infrastructure;
using CollabXAI.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// dependency injection: when a class asks for an interface, it gets the class registered here
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("Default")!);
builder.Services.AddScoped<ISmeService, SmeService>();

var app = builder.Build();

// create the database and tables on startup if they dont exist yet
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

// makes Program visible to the test project
public partial class Program { }
