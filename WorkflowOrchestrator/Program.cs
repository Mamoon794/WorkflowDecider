using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=workflows.db"));
builder.Services.AddHttpClient<IAiOrchestratorService, AiOrchestratorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("/api/workflows", async (string goal, IAiOrchestratorService aiService, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(goal)) return Results.BadRequest("Goal cannot be empty.");

    Console.WriteLine($"\n---> Received request to create workflow: '{goal}'");

    // 1. Get structured sequence from AI service
    var stepDescriptions = await aiService.GenerateStepsAsync(goal);

    // 2. Map actions to database entities
    var workflow = new Workflow { Title = goal };
    for (int i = 0; i < stepDescriptions.Count; i++)
    {
        workflow.Steps.Add(new WorkflowStep
        {
            Description = stepDescriptions[i],
            OrderIndex = i
        });
    }

    // 3. Persist transaction
    db.Workflows.Add(workflow);
    await db.SaveChangesAsync();

    Console.WriteLine($"---> Successfully saved Workflow ID {workflow.Id} to database.");

    return Results.Created($"/api/workflows/{workflow.Id}", workflow);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

