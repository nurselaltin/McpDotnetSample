// Client nesnesi oluştur
using ModelContextProtocol.Client;

//Client connect to Server adresini belirle
 var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
 {
   Name = "MCP Server",
   Command = @"C:\Users\nursel.altin\source\repos\McpDotnetSample\McpDotnetSample\MCPServer\bin\Debug\net7.0\MCPServer.exe"
 });
 
//Client Nesnesi oluştur
var client =  await McpClientFactory.CreateAsync(clientTransport);

//Server tools sınıflarını çek
var tools = await client.ListToolsAsync();

foreach (var item in tools)
{
  Console.WriteLine(item);  
}


