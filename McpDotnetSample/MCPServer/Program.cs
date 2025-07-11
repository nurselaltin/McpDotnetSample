using MCPServer.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<AIAgentTool>();
builder.Services.AddSingleton<FileReaderTool>();
builder.Services.AddSingleton<SpamClassifierTool>();
builder.Services.AddSingleton<AIAgentTool>(sp =>
{
  var fileReader = sp.GetRequiredService<FileReaderTool>();
  var spam = sp.GetRequiredService<SpamClassifierTool>();
  var config = sp.GetRequiredService<IConfiguration>();

  return new AIAgentTool(fileReader, spam);
});

//Serilog ayarları
Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Information()
  .WriteTo.Console()
  .WriteTo.File(@"C:\logs_Server\mcpserver-.log", 
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                fileSizeLimitBytes: 10 * 1024 * 1024, // 10MB
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1))
  .CreateLogger();

// Serilog'u Host'a ekle
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services
  .AddMcpServer()
  .WithStdioServerTransport()
  .WithToolsFromAssembly();

builder.Build().Run();


