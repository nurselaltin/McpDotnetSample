using ModelContextProtocol.Server;
using System.ComponentModel;
using Serilog;

namespace MCPServer.Tools
{

  public  class SpamClassifierTool
  {

    //private readonly AIAgent _agent;

    //public SpamClassifierTool(AIAgent agent)
    //{
    //  _agent = agent;
    //}
    
    public  Task<string> isSpam(string input)
    {
      //if (!_agent.CanUseTool("SpamClassifierTool"))
      //{
      //  Log.Warning("SpamClassifierTool sistem prompt tarafından engellendi.");
      //  return Task.FromResult("Bu tool sistem prompt tarafından engellendi.");
      //}

      //if (_agent.IsSuspiciousInput(input))
      //{
      //  Log.Warning("Prompt injection tespit edildi.");
      //  return Task.FromResult("Yemedim yavrum injectionını :)");
      //}

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
