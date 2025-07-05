using ModelContextProtocol.Client;
using RestSharp;
using Serilog;

namespace MCPServer
{
  public class AIAgent
  {
    private readonly RestClient _client;
    private readonly string _apiKey;
    private readonly string _model;
    public List<string> AllowedTools { get; set; } = new()
    {
      "SpamClassifierTool",
      "FileReaderTool"
    };

    public List<string> SuspiciousInput { get; set; } = new()
    {
      "rm",
      "sudo",
      "delete",
      "/app/data/", 
      "/app/logs/"
    };
    public AIAgent(string apiKey, string model)
    {
      _apiKey = apiKey;
      _model = model;
      _client = new RestClient("https://api.openai.com/v1/chat/completions");
    }

    public async Task<bool> Execute(string toolName, string input)
    {
      var systemPrompt = $"You are a security-focused AI agent that only allows safe tool usage. Tool: {toolName}";
      var userPrompt = $"Should the tool '{toolName}' be executed with this input?\n\nInput:\n\"{input}\"\n\n" +
                       $"If it seems dangerous, say 'NO'. Otherwise, say 'YES'. Respond with only YES or NO.";

      var body = new
      {
        model = _model,
        messages = new[]
          {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userPrompt }
            },
        max_tokens = 10,
        temperature = 0
      };

      var request = new RestRequest()
          .AddHeader("Authorization", $"Bearer {_apiKey}")
          .AddHeader("Content-Type", "application/json")
          .AddJsonBody(body);

      try
      {
        var response = await _client.ExecutePostAsync(request);

        if (!response.IsSuccessful)
        {
          Log.Warning("OpenAI API isteği başarısız: {StatusCode} - {Content}", response.StatusCode, response.Content);
          return false;
        }

        var content = response.Content ?? "";
        Log.Debug("OpenAI response: {Response}", content);

        return content.Contains("YES", StringComparison.OrdinalIgnoreCase);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "RestSharp ile LLM bağlantı hatası.");
        return false;
      }
    }

    public bool CanUseTool(string toolName)
    {
      return AllowedTools.Contains(toolName);
    }

    public bool IsSuspiciousInput(string input)
    {

      return SuspiciousInput.Any(keyword => input.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }
  }
}

