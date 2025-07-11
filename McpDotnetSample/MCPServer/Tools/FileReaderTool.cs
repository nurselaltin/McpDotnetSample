using Serilog;

namespace MCPServer.Tools
{
  public class FileReaderTool
  {
    public List<string> SuspiciousInput { get; set; } = new()
    {
      "rm",
      "sudo",
      "delete",
      "/app/data/",
      "/app/logs/"
    };
    public Task<string> Execute(string path)
    {
      Log.Information("FileReaderTool.Execute çağrıldı. path: {Path}", path);

      // Dosya mevcut mu kontrolü
      if (!File.Exists(path))
      {
        Log.Warning("Dosya bulunamadı: {Path}", path);
        return Task.FromResult("Dosya bulunamadı.");
      }

      // Dosya okuma işlemi
      try
      {
        string content = File.ReadAllText(path) ?? string.Empty;
        Log.Information("Dosya başarıyla okundu. path: {Path}, Boyut: {Length} karakter", path, content.Length);
        return Task.FromResult(content);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Dosya okunurken hata oluştu. path: {Path}", path);
        return Task.FromResult("Dosya okunamadı. Hata: " + ex.Message);
      }
    }
  }
}
