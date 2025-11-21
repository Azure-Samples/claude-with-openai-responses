#!/usr/bin/dotnet run

#:package Azure.Identity@1.*

// Claude Sonnet 4.5 - Responses API with EntraID Authentication
// This demonstrates using Azure Identity (EntraID) for keyless authentication.
// Note: OpenAI C# SDK doesn't support query parameters for Azure AI Foundry.
// This implementation uses HttpClient directly, similar to the Java approach.

using System.Text;
using System.Text.Json;
using Azure.Core;
using Azure.Identity;

Console.WriteLine("Claude Sonnet 4.5 - Responses API with EntraID\n");

// Azure AI Foundry endpoint with api-version
var endpoint = "https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai/responses?api-version=2025-11-15-preview";

// Use DefaultAzureCredential for EntraID authentication
var credential = new DefaultAzureCredential();
var tokenRequestContext = new TokenRequestContext(["https://ai.azure.com/.default"]);
var token = await credential.GetTokenAsync(tokenRequestContext);

// Create HTTP client
using var httpClient = new HttpClient();

// Create request body as JSON string
var jsonBody = """
{
    "model": "claude-sonnet-4-5",
    "input": "Write a one-sentence bedtime story about a unicorn.",
    "max_output_tokens": 1000
}
""";

var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

// Add authorization header
httpClient.DefaultRequestHeaders.Authorization = 
    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);

// Make the request
var response = await httpClient.PostAsync(endpoint, content);

if (response.IsSuccessStatusCode)
{
    var responseBody = await response.Content.ReadAsStringAsync();
    
    // Parse JSON response
    using var doc = JsonDocument.Parse(responseBody);
    var root = doc.RootElement;
    
    Console.WriteLine($"Response from model: {root.GetProperty("model").GetString()}:\n");
    
    // Navigate: output[0].content[0].text
    var output = root.GetProperty("output");
    if (output.GetArrayLength() > 0)
    {
        var message = output[0];
        var contentArray = message.GetProperty("content");
        if (contentArray.GetArrayLength() > 0)
        {
            var contentItem = contentArray[0];
            var text = contentItem.GetProperty("text").GetString();
            Console.WriteLine(text);
        }
    }
}
else
{
    Console.WriteLine($"Error: {response.StatusCode}");
    Console.WriteLine(await response.Content.ReadAsStringAsync());
}
