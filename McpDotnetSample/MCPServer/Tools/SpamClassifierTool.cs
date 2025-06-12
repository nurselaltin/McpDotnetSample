using ModelContextProtocol.Server;
using System.ComponentModel;

namespace MCPServer.Tools
{
  [McpServerToolType]
  public  class SpamClassifierTool
  {
    [McpServerTool, Description("Mailin spam olup olmadığını basit kurallarla kontrol eder.")]
    public static Task<bool> isSpam(string input)
    {
      var mail = input?.ToLowerInvariant() ?? "";
      var spamWords = new[] { "free", "win", "money", "offer", "click" };
      var isSpam = spamWords.Any(word => mail.Contains(word));
      return Task.FromResult(isSpam);
    }
  }
}
