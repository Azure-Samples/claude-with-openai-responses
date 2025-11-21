# Java - Claude Sonnet 4.5 Example

This demonstrates using the OpenAI Responses API with Claude models hosted in Microsoft Foundry using Azure Entra ID authentication.

## Prerequisites

- Java 17+
- Maven 3.6+
- Azure CLI (logged in) or other Azure credential

## Implementation

Uses the OpenAI Java SDK and Azure DefaultAzureCredential for authentication.

## Dependencies

- `azure-identity` (v1.18.1) - Azure authentication
- `openai-java` (v4.8.0) - OpenAI Java SDK with OkHttp client

## Configuration

Update the endpoint and API version in `ClaudeResponsesExample.java`:

```java
String baseUrl = "https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai";
String apiVersion = "2025-11-15-preview"; // Required query parameter
String model = "claude-sonnet-4-5";  // Your Claude deployment name
```

## Usage

The example uses Azure Entra ID authentication via DefaultAzureCredential. Ensure you are logged in with Azure CLI or have appropriate credentials configured.

## Example Code

```java
// ...existing code...
var azureCredential = new DefaultAzureCredentialBuilder().build();
Supplier<String> bearerTokenSupplier = AuthenticationUtil.getBearerTokenSupplier(azureCredential, "https://ai.azure.com/.default");
OpenAIClient client = OpenAIOkHttpClient.builder()
    .baseUrl(baseUrl)
    .credential(BearerTokenCredential.create(bearerTokenSupplier))
    .putQueryParam("api-version", apiVersion)
    .build();
ResponseCreateParams responseCreateParams = ResponseCreateParams.builder()
    .model(ResponsesModel.ofString(model))
    .input(ResponseCreateParams.Input.ofText("Explain quantum computing in simple terms"))
    .maxOutputTokens(1000)
    .build();
Response response = client.responses().create(responseCreateParams);
System.out.println("Response from model: " + model + " : " + response.output());
// ...existing code...
```

## Setup

```bash
mvn clean install
```

## Run

```bash
mvn clean compile exec:java
```

## Files

- `ClaudeResponsesExample.java` - Main Java code with direct HTTP and Azure Entra ID authentication
- `pom.xml` - Maven project configuration and dependencies
