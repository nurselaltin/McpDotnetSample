using ModelContextProtocol.Protocol;

namespace MCPServer.Tools
{
  public  class SpamClassifierTool : Tool
  {
    public string Name => "spamClassifierToll";
    public string Description => "Mailin spam olup olmadığını basit kurallarla kontrol eder.";
    public  Task<object?> InvokeAsync(object? input)
    {
       var mail = input?.ToString()?.ToLowerInvariant() ?? "";
       var spamWords = new List<string>() { "free", "win", "money","offer", "click"};
      var isSpam = spamWords.Any(word => mail.Contains(word));
      return Task.FromResult<object?>(new { isSpam });
    }
  }
}
