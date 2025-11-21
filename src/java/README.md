# Java - Claude Sonnet 4.5 Example

This demonstrates using the OpenAI Responses API with Claude models hosted in Microsoft Foundry via direct HTTP requests.

## Prerequisites

- Java 17+
- Maven 3.6+
- Azure CLI (logged in) or other Azure credential

## Implementation

Uses direct HTTP with OkHttp and Azure DefaultAzureCredential for authentication.

**Note**: The OpenAI Java SDK has compatibility issues with Microsoft Foundry's Responses API endpoint, so this implementation uses direct HTTP as a reliable workaround.

## Dependencies

- `azure-identity` (v1.15.1) - Azure authentication
- `okhttp` (v4.12.0) - HTTP client
- `jackson-databind` (v2.18.2) - JSON parsing

## Configuration

Update the endpoint in `ClaudeResponsesExample.java`:

```java
String baseUrl = "https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai";
String model = "claude-sonnet-4-5";  // Your Claude deployment name
```

## Setup

```bash
# Install dependencies
mvn clean install
```

## Run

```bash
# Run the example
mvn clean compile exec:java
```

## Files

- `ClaudeResponsesExample.java` - Main Java code with direct HTTP implementation
- `pom.xml` - Maven project configuration and dependencies
