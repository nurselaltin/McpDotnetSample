using MCPServer;
using MCPServer.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<AIAgent>();
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

//Api Key
var conf = new ConfigurationBuilder()
           .AddUserSecrets<Program>()
           .Build();
string apiKey = conf["OpenAI:ApiKey"];
var agent = new AIAgent(apiKey);

builder.Services
  .AddMcpServer()
  .WithStdioServerTransport()
  .WithToolsFromAssembly();





await builder.Build().RunAsync();


