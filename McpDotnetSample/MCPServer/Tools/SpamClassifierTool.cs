using Serilog;

namespace MCPServer.Tools
{
  public  class SpamClassifierTool
  {
    public  Task<string> isSpam(string input)
    {
      Log.Information("SpamClassifierTool.isSpam çağrıldı. input: {Input}", input);
      string result = "";
      var mail = input?.ToLowerInvariant() ?? "";
      var spamWords = new[] { "free", "win", "money", "offer", "click" };
      var isSpam = spamWords.Any(word => mail.Contains(word));
      if (isSpam)
      {
        result = "Spam tespit edildi!";
        Log.Warning("Spam tespit edildi! input: {Input}, spamWords: {SpamWords}", input, string.Join(", ", spamWords));
      }
      else
      {
        result = "Spam tespit edilmedi.";
        Log.Information("Spam tespit edilmedi. input: {Input}", input);
      }
      return Task.FromResult(result);
    }
  }
}
