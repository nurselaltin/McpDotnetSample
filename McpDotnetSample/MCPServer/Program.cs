using MCPServer;
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
//builder.Configuration.AddUserSecrets<Program>();

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
//Api Key
//var key = builder.Configuration["OpenAI:ApiKey"];
//string prompt = "Bu spam mail mi?: Subject: You’ve Won a Free iPhone! 📱  \nBody: Congratulations! You’ve been selected for a FREE iPhone 15.  \nClick the link below to claim your reward before midnight:  \n👉 http://scammy-offer.biz/claim-now  \nDon't miss out! This offer is exclusive and expires soon!";
//string prompt2 = "C:\\mcp_server_folders\\test.txt";
//var ai = new AIAgentTool(new FileReaderTool(), new SpamClassifierTool(), key);
//var res = ai.Execute(prompt2);



await builder.Build().RunAsync();


