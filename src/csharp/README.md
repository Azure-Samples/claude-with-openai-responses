# C# - Claude Sonnet 4.5 Example

## Prerequisites

- .NET 8.0+
- Azure CLI (logged in)

## Implementation Details

This example uses:
- **BearerTokenPolicy** from Azure.Identity for authentication with DefaultAzureCredential
- **ApiVersionPipelinePolicy** - Custom pipeline policy to inject the api-version query parameter
- **OpenAI SDK** - Native .NET SDK for type-safe API access

The implementation separates concerns with dedicated policies for authentication and API versioning, making it clean and maintainable.

## Run

```bash
# Run with dotnet-repl (if installed)
dotnet run claude-openai-responses.cs

# Or compile and run
dotnet build
dotnet run
```

## Files

- `claude-openai-responses.cs` - Main C# code with EntraID authentication using BearerTokenPolicy and custom ApiVersionPipelinePolicy

## Note

This example uses the dotnet-repl style with `#:package` directives for easy execution.
For a full project, you would create a `.csproj` file.
