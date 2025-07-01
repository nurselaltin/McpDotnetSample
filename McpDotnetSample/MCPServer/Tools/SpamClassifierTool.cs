using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Serilog;

namespace MCPServer.Tools
{
  [McpServerToolType]
  public  class SpamClassifierTool
  {
    [McpServerTool, Description("Mailin spam olup olmadığını basit kurallarla kontrol eder.")]
    public static Task<bool> isSpam(string input)
    {
      Log.Information("SpamClassifierTool.isSpam çağrıldı. input: {Input}", input);
      var mail = input?.ToLowerInvariant() ?? "";
      var spamWords = new[] { "free", "win", "money", "offer", "click" };
      var isSpam = spamWords.Any(word => mail.Contains(word));
      if (isSpam)
        Log.Warning("Spam tespit edildi! input: {Input}, spamWords: {SpamWords}", input, string.Join(", ", spamWords));
      else
        Log.Information("Spam tespit edilmedi. input: {Input}", input);
      return Task.FromResult(isSpam);
    }
  }
}
