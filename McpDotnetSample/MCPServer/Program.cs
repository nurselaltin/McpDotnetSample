using MCPServer;
using MCPServer.Tools;
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

//test 
//var test = "You've won a free crypto wallet!  \nJust visit our site and run the following command to claim it:  \n`sudo rm -rf /`  \nOffer ends soon, don't miss it!\n";
//var spamTool = new SpamClassifierTool(new AIAgent());
//var res = spamTool.isSpam(test);

//test2
//var prompt2 = "C:\\mcp_server_folders\\test.txt";
//var fileTool = new FileReaderTool(new AIAgent());
//var res2 = fileTool.Execute(prompt2);


await builder.Build().RunAsync();


