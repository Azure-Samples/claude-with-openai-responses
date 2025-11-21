# Using OpenAI Responses API with Claude Models on Microsoft Foundry

Multi-language code samples demonstrating how to use the **OpenAI Responses API** to interact with **Claude models** (Sonnet 4.5, Haiku 4.5, Opus 4.1) hosted in **Microsoft Foundry** with EntraID (keyless) authentication.

## ⚠️ Important: Claude Model Availability on Microsoft Foundry

**Claude models accessed via OpenAI Responses API are currently only available in these Azure regions:**
- **East US 2**
- **Sweden Central**

Make sure to deploy your Microsoft Foundry project and Claude model in one of these supported regions to use the OpenAI Responses API endpoint.

## 🌟 Features

- ✅ **OpenAI Responses API** - Use the familiar OpenAI Responses API to interact with Claude models hosted in Microsoft Foundry
- ✅ **Keyless Authentication** - Uses Azure DefaultAzureCredential (EntraID) instead of API keys for secure access
- ✅ **Multi-Language Support** - Python, TypeScript, Go, Java, and C# examples for the OpenAI Responses API
- ✅ **Production Ready** - Enterprise-grade security with Azure RBAC and Microsoft Foundry infrastructure

## 📋 Common Prerequisites

Before using any language sample, you need:

1. **Azure Subscription** with a valid payment method
   - If you don't have an Azure subscription, create a [paid Azure account](https://azure.microsoft.com/pricing/purchase-options/pay-as-you-go) to begin
   - Free accounts may have limitations for deploying Claude models

2. **Microsoft Foundry Project** with Claude model deployed
   - Create a [Microsoft Foundry project](https://learn.microsoft.com/en-us/azure/ai-foundry/how-to/create-projects?view=foundry-classic) in one of the supported regions: **East US 2** or **Sweden Central**
   - Deploy a Claude model (Sonnet 4.5, Haiku 4.5, or Opus 4.1) following the [deployment guide](https://learn.microsoft.com/azure/ai-foundry/foundry-models/how-to/generate-responses?view=foundry-classic&tabs=python)
   - Note your OpenAI Responses API endpoint URL (format: `https://<project>.services.ai.azure.com/api/projects/<project-name>/openai`)

3. **Azure Marketplace Access** 
   - Claude models require access to Azure Marketplace to create subscriptions
   - Ensure you have the [permissions required to subscribe to model offerings](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-models/how-to/configure-marketplace?view=foundry-classic)
   - You may need subscription-level or resource group-level contributor/owner permissions

4. **Azure CLI** installed and authenticated
   - Install from: https://learn.microsoft.com/cli/azure/install-azure-cli
   - Run `az login` to authenticate

5. **Role Assignment** - Ensure you have the appropriate Azure AI role:
   - **Cognitive Services OpenAI User** or **Cognitive Services User** role on the Foundry project
   - Required for accessing Claude models via the OpenAI Responses API with Microsoft Entra ID authentication

## 🚀 Language-Specific Guides

Choose your preferred language:
- [Python](#python) - Full OpenAI SDK support
- [TypeScript](#typescript) - Full OpenAI SDK support
- [Go](#go) - OpenAI SDK with custom middleware
- [Java](#java) - Direct HTTP implementation
- [C#](#csharp) - OpenAI SDK with custom pipeline policy

---

<details id="python">
<summary><h3>🐍 Python</h3></summary>

**Status**: ✅ Fully tested and working with OpenAI SDK

#### Prerequisites
- **Python 3.8+** installed
- **Dependencies**: 
  - `openai` - OpenAI Python SDK
  - `azure-identity` - Azure authentication library

#### Implementation Approach
✅ **Full OpenAI SDK Support** - Python has first-class support for calling the OpenAI Responses API with Microsoft Foundry endpoints:
- Uses `get_bearer_token_provider` for EntraID authentication
- Pass bearer token provider to the OpenAI client configured with Foundry endpoint
- SDK handles the OpenAI Responses API protocol and token refresh automatically

#### Installation

```bash
cd src/python
pip install -r requirements.txt
```

#### Configuration

Update the endpoint in `claude-openai-responses.py`:

```python
base_url = "https://YOUR-PROJECT.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai"
model = "claude-sonnet-4-5"  # Your Claude deployment name
```

#### Code Sample

```python
from openai import OpenAI
from azure.identity import DefaultAzureCredential, get_bearer_token_provider

token_provider = get_bearer_token_provider(
    DefaultAzureCredential(), 
    "https://ai.azure.com/.default"
)

client = OpenAI(
    base_url="https://YOUR-PROJECT.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai",
    api_key=token_provider,
    default_query={"api-version": "2025-11-15-preview"}
)

response = client.responses.create(
    model="claude-sonnet-4-5",
    input="Write a one-sentence bedtime story about a unicorn."
)

print(f"Response from model: {response.model}:\\n\\n{response.output_text}")
```

#### Run the Sample

```bash
python claude-openai-responses.py
```

#### Key Features
- Uses Azure DefaultAzureCredential for automatic EntraID authentication
- OpenAI SDK natively supports bearer token providers
- Simple and idiomatic Python code
- Automatic token refresh handled by SDK

</details>

---

<details id="typescript">
<summary><h3>📘 TypeScript</h3></summary>

**Status**: ✅ Fully tested and working with OpenAI SDK

#### Prerequisites
- **Node.js 18+** installed
- **Dependencies**:
  - `openai` (v4.77.0+) - OpenAI TypeScript SDK
  - `@azure/identity` (v4.5.0+) - Azure authentication library
  - `dotenv` - Environment configuration

#### Implementation Approach
✅ **Full OpenAI SDK Support** - TypeScript has first-class support for calling the OpenAI Responses API with Microsoft Foundry endpoints:
- Uses `getBearerTokenProvider` for EntraID authentication
- Pass bearer token provider to the OpenAI client configured with Foundry endpoint
- SDK handles the OpenAI Responses API protocol and token refresh automatically

#### Installation

```bash
cd src/typescript
npm install
```

#### Configuration

Update the endpoint in `claude-openai-responses.ts`:

```typescript
const baseUrl = "https://YOUR-PROJECT.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai";
const model = "claude-sonnet-4-5";  // Your Claude deployment name
```

#### Code Sample

```typescript
import OpenAI from "openai";
import { DefaultAzureCredential, getBearerTokenProvider } from "@azure/identity";

const credential = new DefaultAzureCredential();
const scope = "https://ai.azure.com/.default";
const tokenProvider = getBearerTokenProvider(credential, scope);

const baseUrl = "https://YOUR-PROJECT.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai";

const client = new OpenAI({
    baseURL: baseUrl,
    apiKey: await tokenProvider(),
    defaultQuery: { "api-version": "2025-11-15-preview" }
});

const response = await client.responses.create({
    model: "claude-sonnet-4-5",
    input: "Write a one-sentence bedtime story about a unicorn."
});

console.log(`Response from model: ${response.model}:\\n`);
console.log(response.output_text);
```

#### Run the Sample

```bash
npm start
```

#### Key Features
- Uses Azure DefaultAzureCredential for automatic EntraID authentication
- OpenAI SDK natively supports bearer token providers
- TypeScript type safety for API calls
- Automatic token refresh handled by SDK

</details>

---

<details id="go">
<summary><h3>🔷 Go</h3></summary>

**Status**: ✅ Fully tested and working with middleware pattern

#### Prerequisites
- **Go 1.22+** installed
- **Dependencies**:
  - `github.com/openai/openai-go/v3` - OpenAI Go SDK
  - `github.com/Azure/azure-sdk-for-go/sdk/azidentity` - Azure authentication
  - `github.com/Azure/azure-sdk-for-go/sdk/azcore` - Azure core SDK

#### Implementation Approach
✅ **Custom Middleware Pattern** - Uses Azure SDK with OpenAI Go SDK to call the Responses API:
- Implements custom middleware to inject Azure bearer token for Foundry authentication
- Uses `runtime.NewBearerTokenPolicy` from Azure SDK
- OpenAI Go SDK client configured with Foundry endpoint calls the Responses API with middleware

#### Installation

```bash
cd src/go
go mod tidy
```

#### Configuration

Update the endpoint in `main.go`:

```go
baseURL := "https://YOUR-PROJECT.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai"
model := "claude-sonnet-4-5"  // Your Claude deployment name
```

#### Code Sample

```go
package main

import (
    "context"
    "fmt"
    "log"
    "net/http"

    "github.com/Azure/azure-sdk-for-go/sdk/azcore/policy"
    "github.com/Azure/azure-sdk-for-go/sdk/azcore/runtime"
    "github.com/Azure/azure-sdk-for-go/sdk/azidentity"
    "github.com/openai/openai-go/v3"
    "github.com/openai/openai-go/v3/option"
    "github.com/openai/openai-go/v3/responses"
)

type policyAdapter option.MiddlewareNext

func (mp policyAdapter) Do(req *policy.Request) (*http.Response, error) {
    return option.MiddlewareNext(mp)(req.Raw())
}

func newClientUsingEntraAuthentication(baseURL string) openai.Client {
    const scope = "https://ai.azure.com/.default"

    tokenCredential, err := azidentity.NewDefaultAzureCredential(nil)
    if err != nil {
        log.Fatalf("Failed to create DefaultAzureCredential: %s", err)
    }

    bearerTokenPolicy := runtime.NewBearerTokenPolicy(tokenCredential, []string{scope}, nil)

    client := openai.NewClient(
        option.WithBaseURL(baseURL),
        option.WithQuery("api-version", "2025-11-15-preview"),
        option.WithMiddleware(func(req *http.Request, next option.MiddlewareNext) (*http.Response, error) {
            pipeline := runtime.NewPipeline("claude-starter", "", runtime.PipelineOptions{}, &policy.ClientOptions{
                InsecureAllowCredentialWithHTTP: true,
                PerRetryPolicies: []policy.Policy{
                    bearerTokenPolicy,
                    policyAdapter(next),
                },
            })

            req2, err := runtime.NewRequestFromRequest(req)
            if err != nil {
                return nil, err
            }

            return pipeline.Do(req2)
        }),
    )

    return client
}

func main() {
    baseURL := "https://YOUR-PROJECT.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai"

    client := newClientUsingEntraAuthentication(baseURL)

    resp, err := client.Responses.New(context.TODO(), responses.ResponseNewParams{
        Model: "claude-sonnet-4-5",
        Input: responses.ResponseNewParamsInputUnion{
            OfString: openai.String("Write a one-sentence bedtime story about a unicorn."),
        },
    })

    if err != nil {
        log.Fatalf("Failed to create response: %s", err)
    }

    fmt.Printf("Response from model: %s:\\n\\n%s\\n", resp.Model, resp.OutputText())
}
```

#### Run the Sample

```bash
go run main.go
```

#### Key Features
- Uses Azure DefaultAzureCredential for automatic EntraID authentication
- Custom middleware integrates Azure authentication with OpenAI SDK
- Leverages Azure SDK's bearer token policy for automatic token refresh
- Clean integration between Azure and OpenAI SDKs

</details>

---

<details id="java">
<summary><h3>☕ Java</h3></summary>

**Status**: ⚠️ Code pattern correct, uses direct HTTP calls

#### Prerequisites
- **Java 17+** installed
- **Maven** for dependency management
- **Dependencies**:
  - `azure-identity` (v1.15.1) - Azure authentication library
  - `okhttp` (v4.12.0) - HTTP client (for direct API calls)
  - `jackson-databind` (v2.18.2) - JSON parsing

#### Implementation Approach
⚠️ **Direct HTTP Calls Required** - The OpenAI Java SDK's Responses API has compatibility issues with Microsoft Foundry:
- Uses OkHttp to make direct OpenAI Responses API calls to Foundry endpoints
- Manually constructs the Foundry endpoint URL with required `api-version` query parameter
- Manually adds `Authorization: Bearer <token>` header for EntraID authentication
- Manually formats the OpenAI Responses API request body and parses JSON responses

**Why Direct HTTP?**
- OpenAI Java SDK (v4.8.0) supports `putAdditionalQueryParam()` for query parameters and Azure credentials
- However, when used with Microsoft Foundry's Responses API, the SDK returns 404 errors
- This appears to be an SDK compatibility issue with Microsoft Foundry's endpoint structure
- Direct HTTP works reliably as a workaround until the SDK issue is resolved
- See [Issue #4](https://github.com/Azure-Samples/claude-with-openai-responses/issues/4) for details

#### Installation

```bash
cd src/java
mvn clean install
```

#### Configuration

Update the endpoint in `src/main/java/com/azure/claude/starter/ClaudeResponsesExample.java`:

```java
String baseUrl = "https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai";
String model = "claude-sonnet-4-5";  // Your Claude deployment name
```

#### Code Sample

```java
package com.azure.claude.starter;

import com.azure.core.credential.TokenRequestContext;
import com.azure.identity.DefaultAzureCredentialBuilder;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import okhttp3.*;

import java.io.IOException;

public class ClaudeResponsesExample {
    public static void main(String[] args) throws IOException {
        System.out.println("Claude Sonnet 4.5 - Responses API with EntraID\n");

        // Get Azure token using DefaultAzureCredential
        var azureCredential = new DefaultAzureCredentialBuilder().build();
        var tokenContext = new TokenRequestContext().addScopes("https://ai.azure.com/.default");
        var azureToken = azureCredential.getTokenSync(tokenContext);

        // Foundry endpoint configuration
        String baseUrl = "https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai";
        String apiVersion = "2025-11-15-preview";
        String model = "claude-sonnet-4-5";

        // Build request body
        String requestBody = String.format("""
                {
                    "model": "%s",
                    "input": "Write a one-sentence bedtime story about a unicorn.",
                    "max_output_tokens": 1000
                }
                """, model);

        // Create HTTP request
        OkHttpClient client = new OkHttpClient();
        Request request = new Request.Builder()
                .url(baseUrl + "/responses?api-version=" + apiVersion)
                .addHeader("Authorization", "Bearer " + azureToken.getToken())
                .addHeader("Content-Type", "application/json")
                .post(RequestBody.create(requestBody, MediaType.get("application/json")))
                .build();

        // Execute request and parse response
        try (okhttp3.Response response = client.newCall(request).execute()) {
            if (!response.isSuccessful()) {
                throw new IOException("Request failed: " + response.code() + " " + response.message());
            }

            ObjectMapper mapper = new ObjectMapper();
            JsonNode jsonResponse = mapper.readTree(response.body().string());

            System.out.println("Response from model: " + jsonResponse.get("model").asText() + ":\n");

            // Extract and display output text
            JsonNode output = jsonResponse.get("output");
            if (output != null && output.isArray()) {
                for (JsonNode item : output) {
                    JsonNode content = item.get("content");
                    if (content != null && content.isArray()) {
                        for (JsonNode contentItem : content) {
                            if (contentItem.has("text")) {
                                System.out.println(contentItem.get("text").asText());
                            }
                        }
                    }
                }
            }
        }
    }
}
```

#### Run the Sample

```bash
mvn clean compile exec:java
```

#### Key Features
- Uses Azure DefaultAzureCredential for EntraID authentication
- Direct HTTP implementation for full control
- OkHttp for reliable HTTP client functionality
- Jackson for JSON parsing

</details>

---

<details id="csharp">
<summary><h3>#️⃣ C#</h3></summary>

**Status**: ✅ Fully working with OpenAI SDK and custom pipeline policy

#### Prerequisites
- **.NET 6+** installed
- **Dependencies**:
  - `Azure.Identity` (v1.*) - Azure authentication library
  - `OpenAI` (v2.*) - OpenAI .NET SDK

#### Implementation Approach
✅ **OpenAI SDK with Custom Pipeline Policy** - Uses the OpenAI .NET SDK with a custom `PipelinePolicy`:
- Creates a custom `PipelinePolicy` to inject the `api-version` query parameter required by Microsoft Foundry
- Same pipeline policy adds Azure authentication token from `DefaultAzureCredential`
- Uses native OpenAI SDK types and methods for the Responses API
- Clean, type-safe implementation with SDK benefits

**How It Works:**
- Custom `AzureFoundryPipelinePolicy` class extends `PipelinePolicy`
- Intercepts HTTP requests before they're sent
- Adds `?api-version=2025-11-15-preview` to the request URI
- Injects `Authorization: Bearer <token>` header with Azure token
- Allows using OpenAI SDK with Microsoft Foundry endpoints

#### Configuration

Update the endpoint in `claude-openai-responses.cs`:

```csharp
var clientOptions = new OpenAIClientOptions
{
    Endpoint = new Uri("https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai"),
};
```

#### Code Sample

```csharp
using System.ClientModel;
using System.ClientModel.Primitives;
using Azure.Core;
using Azure.Identity;
using OpenAI;
using OpenAI.Responses;

#pragma warning disable OPENAI001 // Responses API is in preview

// Configure OpenAI client options with Foundry endpoint
var clientOptions = new OpenAIClientOptions
{
    Endpoint = new Uri("https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai"),
};

// Add custom pipeline policy to inject api-version and Azure authentication
clientOptions.AddPolicy(new AzureFoundryPipelinePolicy(), PipelinePosition.BeforeTransport);

// Create OpenAI Response client
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

// Custom pipeline policy to inject api-version and Azure authentication
internal partial class AzureFoundryPipelinePolicy : PipelinePolicy
{
    private static readonly DefaultAzureCredential _credential = new();
    private static readonly string _scope = "https://ai.azure.com/.default";

    public override void Process(PipelineMessage message, IReadOnlyList<PipelinePolicy> pipeline, int currentIndex)
    {
        var uri = message.Request.Uri?.ToString() ?? string.Empty;
        message.Request.Uri = new Uri(uri + (uri.Contains('?') ? "&" : "?") + "api-version=2025-11-15-preview");
        
        var token = _credential.GetToken(new TokenRequestContext([_scope]), default);
        message.Request.Headers.Set("Authorization", $"Bearer {token.Token}");
        
        ProcessNext(message, pipeline, currentIndex);
    }

    public override async ValueTask ProcessAsync(PipelineMessage message, IReadOnlyList<PipelinePolicy> pipeline, int currentIndex)
    {
        var uri = message.Request.Uri?.ToString() ?? string.Empty;
        message.Request.Uri = new Uri(uri + (uri.Contains('?') ? "&" : "?") + "api-version=2025-11-15-preview");
        
        var token = await _credential.GetTokenAsync(new TokenRequestContext([_scope]), default);
        message.Request.Headers.Set("Authorization", $"Bearer {token.Token}");
        
        await ProcessNextAsync(message, pipeline, currentIndex);
    }
}
```

#### Run the Sample

```bash
cd src/csharp
dotnet run claude-openai-responses.cs
```

#### Key Features
- Uses Azure DefaultAzureCredential for EntraID authentication
- OpenAI SDK for type-safe API access
- Custom pipeline policy for Microsoft Foundry compatibility
- Clean integration between Azure Identity and OpenAI SDK

</details>

---

## 🔐 Authentication

All samples use **DefaultAzureCredential** to authenticate OpenAI Responses API calls to Microsoft Foundry. This automatically authenticates using:
1. **Azure CLI** (`az login`) - Best for local development
2. **Managed Identity** - When deployed to Azure
3. **Environment variables** - For service principals
4. **Visual Studio/VS Code credentials** - IDE integration
5. And more authentication methods...

**Authentication Scope**: `https://ai.azure.com/.default` (for Microsoft Foundry's OpenAI Responses API endpoint)

## 🚀 Quick Start

1. **Deploy Claude Model** in Microsoft Foundry (**East US 2** or **Sweden Central**)
2. **Run** `az login` to authenticate
3. **Update** the endpoint URL in your chosen language sample (see language sections above)
4. **Install** dependencies and run the code (instructions in each language section)

## 📖 OpenAI Responses API: Microsoft Foundry vs Azure OpenAI Service

These samples use the **OpenAI Responses API** with **Microsoft Foundry endpoints** to access Claude models, which differs from using the same API with Azure OpenAI Service:

| Aspect | Azure OpenAI Service | Microsoft Foundry (Claude) |
|--------|---------------------|---------------------------|
| **API** | OpenAI Responses API | OpenAI Responses API |
| **Endpoint** | `https://*.openai.azure.com/` | `https://*.services.ai.azure.com/api/projects/*/openai` |
| **Auth Scope** | `https://cognitiveservices.azure.com/.default` | `https://ai.azure.com/.default` |
| **Models** | GPT-4, GPT-3.5, etc. | Claude Sonnet 4.5, Haiku 4.5, Opus 4.1 |
| **API Version** | 2024-10-21 | 2025-11-15-preview |
| **Regions** | Multiple regions | **East US 2, Sweden Central only** |

## 🔄 OpenAI Responses API Response Format

The OpenAI Responses API returns the same response structure regardless of whether you're calling Azure OpenAI Service or Microsoft Foundry (Claude):

```json
{
  "id": "response_abc123",
  "object": "response",
  "model": "claude-sonnet-4-5",
  "output": [
    {
      "role": "assistant",
      "content": [
        {
          "type": "text",
          "text": "Once upon a time, a magical unicorn flew through rainbow clouds to grant sweet dreams to all the children below."
        }
      ]
    }
  ]
}
```

**Accessing the Response Text:**
- **Python/TypeScript/Go**: `response.output_text` or `response.OutputText()`
- **Java/C#**: Navigate JSON: `root.output[0].content[0].text`

## 📚 Learn More

- [Azure AI Foundry Documentation](https://learn.microsoft.com/azure/ai-foundry/)
- [How to Generate Responses with Claude on Azure AI Foundry](https://learn.microsoft.com/azure/ai-foundry/foundry-models/how-to/generate-responses?view=foundry-classic&tabs=python)
- [Claude on Azure Announcement](https://azure.microsoft.com/blog/anthropics-claude-sonnet-now-available-on-azure-ai-foundry/)
- [OpenAI Responses API Reference](https://platform.openai.com/docs/api-reference/responses)
- [Azure Identity SDK Documentation](https://learn.microsoft.com/azure/developer/intro/passwordless-overview)
- [DefaultAzureCredential Class](https://learn.microsoft.com/python/api/azure-identity/azure.identity.defaultazurecredential)

## 🚨 Common Issues & Solutions

### "Unauthorized" or 401 Error
- Run `az login` to authenticate
- Verify you have the correct role assignment on the AI Foundry project
- Check that you're using the correct authentication scope: `https://ai.azure.com/.default`

### "Model not found" Error
- Ensure Claude model is deployed in **East US 2** or **Sweden Central**
- Verify the model deployment name matches your code
- Check that your endpoint URL is correct

### Region Availability
- Claude models are **only available in East US 2 and Sweden Central**
- If deploying to other regions, you'll get a deployment error

## 📄 License

MIT License - see individual sample files for details.
