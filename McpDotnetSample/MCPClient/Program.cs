using ModelContextProtocol.Client;

// MCP Server ile bağlantı kurmak için transport tanımlıyoruz
var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
{
  Name = "MCP Server",
  Command = @"C:\Users\nursel.altin\source\repos\McpDotnetSample\McpDotnetSample\MCPServer\bin\Debug\net7.0\MCPServer.exe"
});

// MCP Client nesnesini oluşturuyoruz ve server'a bağlanıyoruz
var client = await McpClientFactory.CreateAsync(clientTransport);

// Server'da tanımlı tüm tool'ları listeliyoruz
var tools = await client.ListToolsAsync();

foreach (var tool in tools)
{
  Console.WriteLine(tool);
}
