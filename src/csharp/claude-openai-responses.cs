#!/usr/bin/dotnet run

#:package Azure.Identity@1.*
#:package OpenAI@2.*

// Claude Sonnet 4.5 - Responses API with EntraID Authentication
// This demonstrates using Azure Identity (EntraID) for keyless authentication with OpenAI SDK.
// Uses a custom PipelinePolicy to inject both the api-version query parameter and Azure auth token
// required by Microsoft Foundry.

using System.ClientModel;
using System.ClientModel.Primitives;
using Azure.Core;
using Azure.Identity;
using OpenAI;
using OpenAI.Responses;

#pragma warning disable OPENAI001 // Responses API is in preview

Console.WriteLine("Claude Sonnet 4.5 - Responses API with EntraID\n");

// Configure OpenAI client options with Foundry endpoint
var clientOptions = new OpenAIClientOptions
{
    Endpoint = new Uri("https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai"),
};

// Add custom pipeline policy to inject api-version and Azure authentication
clientOptions.AddPolicy(new AzureFoundryPipelinePolicy(), PipelinePosition.BeforeTransport);

// Create OpenAI Response client with placeholder credential (auth handled by pipeline)
var client = new OpenAIResponseClient(
    model: "claude-sonnet-4-5",
    credential: new ApiKeyCredential("not-used"), // Placeholder - actual auth in pipeline
    options: clientOptions);

// Create input message
var inputItems = new List<ResponseItem>
{
    ResponseItem.CreateUserMessageItem("Write a one-sentence bedtime story about a unicorn."),
};

// Configure response options
var responseOptions = new ResponseCreationOptions
{
    MaxOutputTokenCount = 1000,
};

// Create response
var result = client.CreateResponse(inputItems, responseOptions);
var response = result.Value;

Console.WriteLine($"Response from model: {response.Model}:\n");
Console.WriteLine($"{response.GetOutputText()}");

#pragma warning restore OPENAI001

// Custom pipeline policy to inject api-version query parameter and Azure authentication
internal partial class AzureFoundryPipelinePolicy : PipelinePolicy
{
    private static readonly DefaultAzureCredential _credential = new();
    private static readonly string _scope = "https://ai.azure.com/.default";

    public override void Process(PipelineMessage message, IReadOnlyList<PipelinePolicy> pipeline, int currentIndex)
    {
        // Add api-version query parameter
        var uri = message.Request.Uri?.ToString() ?? string.Empty;
        message.Request.Uri = new Uri(uri + (uri.Contains('?') ? "&" : "?") + "api-version=2025-11-15-preview");
        
        // Add Azure authentication token
        var token = _credential.GetToken(new TokenRequestContext([_scope]), default);
        message.Request.Headers.Set("Authorization", $"Bearer {token.Token}");
        
        ProcessNext(message, pipeline, currentIndex);
    }

    public override async ValueTask ProcessAsync(PipelineMessage message, IReadOnlyList<PipelinePolicy> pipeline, int currentIndex)
    {
        // Add api-version query parameter
        var uri = message.Request.Uri?.ToString() ?? string.Empty;
        message.Request.Uri = new Uri(uri + (uri.Contains('?') ? "&" : "?") + "api-version=2025-11-15-preview");
        
        // Add Azure authentication token
        var token = await _credential.GetTokenAsync(new TokenRequestContext([_scope]), default);
        message.Request.Headers.Set("Authorization", $"Bearer {token.Token}");
        
        await ProcessNextAsync(message, pipeline, currentIndex);
    }
}
