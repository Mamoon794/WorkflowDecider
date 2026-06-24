using System.Net.Http.Json;
using System.Text.Json;

public interface IAiOrchestratorService
{
    Task<List<String>> GenerateStepsAsync(string goal);
}

public class AiOrchestratorService : IAiOrchestratorService
{
    private readonly HttpClient _httpClient;
    public AiOrchestratorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<String>> GenerateStepsAsync(string goal)
    {
        string apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") 
        ?? throw new InvalidOperationException("GEMINI_API_KEY is missing from .env");
        
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";
        
        var requestBody = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = $"You are a workflow orchestrator. Break this goal into 3-5 sequential steps: '{goal}'. Return ONLY a raw JSON array of strings. No markdown formatting." } } }
            }
        };

        var response = await _httpClient.PostAsJsonAsync(url, requestBody);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();

        string aiText = jsonResponse
        .GetProperty("candidates")[0]
        .GetProperty("content")
        .GetProperty("parts")[0]
        .GetProperty("text").GetString()!;

        var steps = JsonSerializer.Deserialize<List<string>>(aiText);

        if (steps != null)
        {
            return steps;
        }

        return new List<string>
        {
            $"Step 1: Analyze the goal '{goal}' and break it down into smaller tasks.",
            $"Step 2: Prioritize the tasks based on their dependencies and importance.",
            $"Step 3: Assign resources and set deadlines for each task.",
            $"Step 4: Monitor progress and adjust the plan as needed.",
            $"Step 5: Review the completed tasks and evaluate the overall success of achieving the goal."
        };
    }
}