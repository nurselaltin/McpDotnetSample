using ModelContextProtocol.Server;
using RestSharp;
using Serilog;

namespace MCPServer.Tools
{
  [McpServerToolType]
  public class AIAgentTool
  {
    private readonly FileReaderTool _fileReader;
    private readonly SpamClassifierTool _spamClassifier;
    private readonly Dictionary<string, Func<string, Task<string>>> _toolMap;
    private readonly RestClient _client;
    private readonly string _apiKey;
    private readonly string _model;

    private readonly string _systemPrompt;

    public AIAgentTool(FileReaderTool fileReader, SpamClassifierTool spamClassifier)
    {
        string promptPath = Path.Combine(AppContext.BaseDirectory, "SystemPrompt.txt");
        _systemPrompt = File.ReadAllText(promptPath, System.Text.Encoding.UTF8);
        
          string key = File.ReadAllText(@"C:\..txt");
        _fileReader = fileReader;
        _spamClassifier = spamClassifier;
        _apiKey = key;
        _model = "gpt-3.5-turbo";
        _client = new RestClient("https://api.openai.com/v1/chat/completions");
        
          _toolMap = new Dictionary<string, Func<string, Task<string>>>(StringComparer.OrdinalIgnoreCase)
          {
            { "FileReaderTool", fileReader.Execute },
            { "SpamClassifierTool", spamClassifier.isSpam }
          };
    }
      
    [McpServerTool]
    public async Task<string> Execute(string userPrompt)
    {
      Log.Information("prompt: {userPrompt}", userPrompt);
      var body = new
      {
        model = _model,
        messages = new[]
        {
          new { role = "system", content = _systemPrompt },
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
          return "⚠️ OpenAI API hatası.";
        }

        var content = response.Content ?? "";
        Log.Debug("OpenAI response: {Response}", content);

        foreach (var toolName in _toolMap.Keys)
        {
          if (content.Contains(toolName, StringComparison.OrdinalIgnoreCase))
          {
            Log.Information("AI yönlendirmesiyle çalıştırılan tool: {Tool}", toolName);
            return await _toolMap[toolName](userPrompt);
          }
        }

        return "🚫 AI Agent: Uygun tool bulunamadı veya NONE döndü.";
      }
      catch (Exception ex)
      {
        Log.Error(ex, "RestSharp ile LLM bağlantı hatası.");
        return "❌ Tool çağrısı sırasında bir hata oluştu.";
      }
    }
  }
}
