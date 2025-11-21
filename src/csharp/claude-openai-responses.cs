#!/usr/bin/dotnet run

#:package Azure.Identity@1.*
#:package OpenAI@2.*

// Claude Sonnet 4.5 - Responses API with EntraID Authentication
// This demonstrates using Azure Identity (EntraID) for keyless authentication with OpenAI SDK.
// Uses a custom PipelinePolicy to inject both the api-version query parameter and Azure auth token
// required by Microsoft Foundry.

using System.ClientModel.Primitives;
using Azure.Identity;
using OpenAI;
using OpenAI.Responses;

#nullable disable
#pragma warning disable OPENAI001 // Responses API is in preview

Console.WriteLine("Claude Sonnet 4.5 - Responses API with EntraID");
Console.WriteLine();

// Configure OpenAI client options with Foundry endpoint
Uri projectEndpoint = new("https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME");

OpenAIClientOptions clientOptions = new()
{
    Endpoint = new Uri($"{projectEndpoint.AbsoluteUri.TrimEnd('/')}/openai"),
};

// Add custom pipeline policy to inject api-version
clientOptions.AddPolicy(new ApiVersionPipelinePolicy(), PipelinePosition.PerCall);

// Create OpenAI Response client with token credential
OpenAIResponseClient client = new(
    model: "claude-sonnet-4-5",
    authenticationPolicy: new BearerTokenPolicy(new DefaultAzureCredential(), "https://ai.azure.com/.default"),
    options: clientOptions);

// Create input message
List<ResponseItem> inputItems =
[
    ResponseItem.CreateUserMessageItem("Write a one-sentence bedtime story about a unicorn."),
];

// Configure response options
ResponseCreationOptions responseOptions = new()
{
    MaxOutputTokenCount = 1000,
};

// Create response
OpenAIResponse response = client.CreateResponse(inputItems, responseOptions);

Console.WriteLine($"Response from model: {response.Model}:");
Console.WriteLine();
Console.WriteLine($"{response.GetOutputText()}");

// Custom pipeline policy to inject api-version query parameter
internal partial class ApiVersionPipelinePolicy : PipelinePolicy
{
    private readonly string _apiVersion;

    public ApiVersionPipelinePolicy(string apiVersion = "2025-11-15-preview")
    {
        _apiVersion = apiVersion;
    }

    public override void Process(PipelineMessage message, IReadOnlyList<PipelinePolicy> pipeline, int currentIndex)
    {
        AddApiVersion(message.Request);        
        ProcessNext(message, pipeline, currentIndex);
    }

    public override async ValueTask ProcessAsync(PipelineMessage message, IReadOnlyList<PipelinePolicy> pipeline, int currentIndex)
    {
        AddApiVersion(message.Request);
        await ProcessNextAsync(message, pipeline, currentIndex);
    }

    private void AddApiVersion(PipelineRequest request)
    {
        if (request?.Uri?.Query?.ToLowerInvariant()?.Contains("api-version=") == false)
        {
            UriBuilder builder = new(request.Uri);
            char separator = builder.Query?.Length > 0 ? '&' : '?';
            builder.Query = $"{builder.Query}{separator}api-version={_apiVersion}";
            request.Uri = builder.Uri;
        }
    }
}