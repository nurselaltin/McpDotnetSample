using MCPServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

builder.Services
  .AddMcpServer()
  .WithStdioServerTransport()
  .WithToolsFromAssembly();

//var input = "Hot singles in your area want to meet you!  \nCreate your free profile and start chatting now.";
//AIAgent c = new AIAgent();
//var yy = c.IsSuspiciousInput(input);
//if (yy)
//{
//  var yyy = "";
//}

await builder.Build().RunAsync();


