namespace MCPServer
{
  public class AIAgent
  {
    public List<string> AllowedTools { get; set; } = new()
    {
      "SpamClassifierTool",
      "FileReaderTool"
    };

    public List<string> SuspiciousInput { get; set; } = new()
    {
      "rm",
      "sudo",
      "/app/data/", 
      "/app/logs/",
      "/eodemeapi"
    };

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
