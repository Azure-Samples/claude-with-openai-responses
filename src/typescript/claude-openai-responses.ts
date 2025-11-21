import "dotenv/config";
import OpenAI from "openai";
import { DefaultAzureCredential, getBearerTokenProvider } from "@azure/identity";

/**
 * Claude Sonnet 4.5 - Responses API with EntraID Authentication
 * This demonstrates using Azure Identity (EntraID) for keyless authentication.
 */

async function main(): Promise<void> {
    console.log("Claude Sonnet 4.5 - Responses API with EntraID\n");
    
    // Use DefaultAzureCredential for EntraID authentication
    // This automatically uses your Azure CLI login, Managed Identity, or other credential sources
    const credential = new DefaultAzureCredential();
    const scope = "https://ai.azure.com/.default";
    const tokenProvider = getBearerTokenProvider(credential, scope);

    const baseUrl = "https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai";
    
    // Initialize OpenAI client with Azure AI Foundry endpoint
    const client = new OpenAI({
        baseURL: baseUrl,
        apiKey: await tokenProvider(),
        defaultQuery: { "api-version": "2025-11-15-preview" }
    });
    
    // Create response using Claude model
    const response = await client.responses.create({
        model: "claude-sonnet-4-5",
        input: "Write a one-sentence bedtime story about a unicorn."
    });
    
    console.log(`Response from model: ${response.model}:\n`);
    console.log(response.output_text);
}

main().catch((error) => {
    console.error("Error:", error.message);
    process.exit(1);
});
