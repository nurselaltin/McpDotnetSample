using ModelContextProtocol.Server;
using System.ComponentModel;

namespace MCPServer.Tools
{
  [McpServerToolType]
  public class FileReaderTool
  {
    private readonly string[] allowedPaths = new[] { "/app/data/", "/app/logs/" };
    [McpServerTool, Description("")]
    public  Task<string> Execute(string path)
    {
      // 1. normalize et
      string normalizedPath = Path.GetFullPath(path);

      // 2. izin verilen path’lerle karşılaştır
      if (!allowedPaths.Any(allowed => normalizedPath.StartsWith(allowed)))
        throw new UnauthorizedAccessException("Bu klasöre erişim izni yok.");
      
      string path2 = File.ReadAllText(normalizedPath) ?? string.Empty; 

      return Task.FromResult(path2);
    }
  }
}
