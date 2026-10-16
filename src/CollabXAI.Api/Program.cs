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

// runs once on startup: creates the sqlite file and the Smes table if they dont exist,
// so nobody has to set up a database by hand
// improvement: EnsureCreated wont update an existing table, so a real project should use ef migrations
using (var scope = app.Services.CreateScope())
{
    // the dbcontext is scoped (one per request) and there is no request at startup,
    // so we create a scope ourselves to get one
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
