using ModelContextProtocol.Client;
using Serilog;

// MCP Server ile bağlantı kurmak için transport tanımlıyoruz
var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
{
  Name = "MCP Server",
  Command = @"C:\..\MCPServer.exe"
});


//Logla
Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Debug()
  .WriteTo.Console()
  .WriteTo.File(@"C:\logs_mcp\logs_mcpclient\mcpclient-.log",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                fileSizeLimitBytes: 10 * 1024 * 1024, // 10MB
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1))
  .CreateLogger();

Log.Information("MCP Client başlatılıyor...");
try
{
  var client = await McpClientFactory.CreateAsync(clientTransport);
  Log.Information("MCP Client bağlantısı kuruldu.");

  var tools = await client.ListToolsAsync();
  Log.Information($"Server toplam tool sayısı: {tools.Count}");

  foreach (var tool in tools)
  {
    Console.WriteLine($"Tool: {tool.Name} - {tool.Description}");
    Log.Information($"Tool: {tool.Name} - {tool.Description}");
  }
  
  Console.WriteLine("Prompt giriniz: ");
  string prompt = Console.ReadLine();
  Log.Information($"Kullanıcı prompt: {prompt}");

  var toolForPrompt = tools.FirstOrDefault();
  var res = await toolForPrompt.CallAsync(new Dictionary<string, object?>
  {
    ["userPrompt"] = prompt
  });

  Log.Information($"Mcp Server response : {res.Content.FirstOrDefault().Text}");
}
catch (Exception ex)
{
  Log.Error(ex, "Mcp Server hatası.");
}
