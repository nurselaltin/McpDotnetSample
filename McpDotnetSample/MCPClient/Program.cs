using ModelContextProtocol.Client;

// MCP Server ile bağlantı kurmak için transport tanımlıyoruz
var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
{
  Name = "MCP Server",
  Command = @"\McpDotnetSample\McpDotnetSample\MCPServer\bin\Debug\net7.0\MCPServer.exe"
});

// MCP Client nesnesini oluşturuyoruz ve server'a bağlanıyoruz
var client = await McpClientFactory.CreateAsync(clientTransport);

// Server'da tanımlı tüm tool'ları listeliyoruz
var tools = await client.ListToolsAsync();

foreach (var tool in tools)
{
  Console.WriteLine($"Tool: {tool.Name} - {tool.Description}");
  if (tool.Name == "Execute")
    continue;
  // Tool'a input gönder ve sonucu bekle
  var result = await tool.CallAsync(new Dictionary<string, object?>
  {
    ["input"] = "URGENT: Your PayPal account has been suspended. Please login to verify your identity."
  });

  Console.WriteLine($"Sonuç: {result}");
}
