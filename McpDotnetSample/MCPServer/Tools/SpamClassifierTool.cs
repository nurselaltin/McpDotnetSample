using ModelContextProtocol.Server;
using System.ComponentModel;

namespace MCPServer.Tools
{
  [McpServerToolType]
  public  class SpamClassifierTool
  {
    [McpServerTool, Description("Mailin spam olup olmadığını basit kurallarla kontrol eder.")]
    public  bool  isSpam(string input)
    {
       var mail = input?.ToString()?.ToLowerInvariant() ?? "";
       var spamWords = new List<string>() { "free", "win", "money","offer", "click"};
      var isSpam = spamWords.Any(word => mail.Contains(word));
      return isSpam;
    }
  }
}
