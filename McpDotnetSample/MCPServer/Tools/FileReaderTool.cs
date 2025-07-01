using ModelContextProtocol.Server;
using System.ComponentModel;
using Serilog;

namespace MCPServer.Tools
{
  [McpServerToolType]
  public class FileReaderTool
  {
    private readonly string[] allowedPaths = new[] { "/app/data/", "/app/logs/" };
    [McpServerTool, Description("")]
    public  Task<string> Execute(string path)
    {
      Log.Information("FileReaderTool.Execute çağrıldı. path: {Path}", path);
      // 1. normalize et
      string normalizedPath = Path.GetFullPath(path);
      Log.Debug("Normalize edilmiş yol: {NormalizedPath}", normalizedPath);

      // 2. izin verilen path'lerle karşılaştır
      if (!allowedPaths.Any(allowed => normalizedPath.StartsWith(allowed)))
      {
        Log.Warning("Erişim reddedildi! path: {Path}, allowedPaths: {AllowedPaths}", normalizedPath, string.Join(", ", allowedPaths));
        throw new UnauthorizedAccessException("Bu klasöre erişim izni yok.");
      }
      
      if (!File.Exists(normalizedPath))
      {
        Log.Warning("Dosya bulunamadı: {Path}", normalizedPath);
        return Task.FromResult("Dosya bulunamadı.");
      }

      string path2 = File.ReadAllText(normalizedPath) ?? string.Empty; 
      Log.Information("Dosya okundu: {Path}, Boyut: {Length} karakter", normalizedPath, path2.Length);
      return Task.FromResult(path2);
    }
  }
}
